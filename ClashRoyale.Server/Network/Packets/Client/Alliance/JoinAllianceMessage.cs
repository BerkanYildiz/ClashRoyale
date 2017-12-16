namespace ClashRoyale.Server.Network.Packets.Client.Alliance
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Commands.Server;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.Stream;
    using ClashRoyale.Server.Network.Packets.Server.Alliance;

    internal class JoinAllianceMessage : Message
    {
        private int HighId;
        private int LowId;

        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 14305;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinAllianceMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public JoinAllianceMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // JoinAllianceMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.HighId = this.Stream.ReadInt();
            this.LowId  = this.Stream.ReadInt();

            this.Stream.ReadInt();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override async void Process()
        {
            Logging.Info(this.GetType(), "Player is joining a clan.");

            if (!this.Device.GameMode.CommandManager.WaitJoinAllianceTurn)
            {
                if (!this.Device.GameMode.Player.IsInAlliance)
                {
                    Clan Clan = await Clans.Get(this.HighId, this.LowId);

                    if (Clan != null)
                    {
                        if (await Clan.Members.TryAdd(this.Device.GameMode.Player, false))
                        {
                            var Entry = new AllianceEventStreamEntry(this.Device.GameMode.Player, this.Device.GameMode.Player);
                            Entry.SetJoinEvent();

                            Clan.Messages.AddEntry(Entry);

                            this.Device.NetworkManager.SendMessage(new AllianceDataMessage(this.Device, Clan));
                            this.Device.NetworkManager.SendMessage(new AllianceStreamMessage(this.Device, Clan.Messages.ToArray()));

                            this.Device.GameMode.CommandManager.WaitJoinAllianceTurn = true;
                            this.Device.GameMode.CommandManager.AddAvailableServerCommand(new JoinAllianceCommand(Clan.HighId, Clan.LowId, Clan.HeaderEntry.Name, Clan.HeaderEntry.Badge, false));
                        }
                        else
                        {
                            Logging.Error(this.GetType(), "Player tried to join an alliance but TryAdd(Player, false) returned false.");
                        }
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "Player tried to join an alliance but the database returned a null value.");
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "Player is already in an alliance.");
                }
            }
            else
            {
                Logging.Info(this.GetType(), "Player is already joining an alliance, aborting.");
            }
        }
    }
}