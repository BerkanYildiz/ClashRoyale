namespace ClashRoyale.Server.Network.Packets.Client
{
    using System;
    using System.Threading.Tasks;

    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Game;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Alliance;
    using ClashRoyale.Server.Logic.Alliance.Stream;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.Player;

    internal class ChatToAllianceStreamMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 14308;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Alliance;
            }
        }

        private string Message;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatToAllianceStreamMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public ChatToAllianceStreamMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // ChatToAllianceStreamMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.Message = this.Stream.ReadString();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override async void Process()
        {
            Player Player = this.Device.GameMode.Player;

            Logging.Info(this.GetType(), "Player is sending a clan chat message.");

            if (Player.IsInAlliance)
            {
                Task<Clan> RetrieveClan = Clans.Get(Player.ClanHighId, Player.ClanLowId);

                if (DateTime.UtcNow.AddSeconds(-2) > this.Device.NetworkManager.LastChatMessage)
                {
                    if (!string.IsNullOrEmpty(this.Message))
                    {
                        if (this.Message.Length <= Globals.MaxMessageLength)
                        {
                            this.Message = this.Message.Trim();

                            if (this.Message.Length > 0)
                            {
                                Clan Clan = await RetrieveClan; 

                                if (Clan != null)
                                {
                                    Clan.Messages.AddEntry(new ChatStreamEntry(Player, this.Message));
                                }
                                else
                                {
                                    Logging.Error(this.GetType(), "Player tried to send a chat message but the clan was null.");
                                }
                            }
                            else
                            {
                                Logging.Warning(this.GetType(), "Player tried to send a chat message filled with blank characters.");
                            }
                        }
                        else
                        {
                            Logging.Error(this.GetType(), "Player tried to send a chat message longer than the authorized length.");
                        }
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "Player sent an either null or empty chat message.");
                    }
                }
                else
                {
                    Logging.Warning(this.GetType(), "Player is spamming the chat, cancelling the message.");
                }
            }
            else
            {
                Logging.Error(this.GetType(), "Player is not in a clan, therefore this message shouldn't have been sent.");
            }
        }
    }
}