namespace ClashRoyale.Server.Network.Packets.Client
{
    using System.Collections.Generic;

    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Alliance;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Commands;
    using ClashRoyale.Server.Logic.Commands.Manager;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.Player;

    using ClashRoyale.Server.Network.Packets.Server;

    internal class EndClientTurnMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 18688;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Home;
            }
        }

        private int Tick;
        private int Checksum;
        private int Count;

        private List<Command> Commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndClientTurnMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public EndClientTurnMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // End_Client_Turn_Message.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.Tick       = this.Stream.ReadVInt();
            this.Checksum   = this.Stream.ReadVInt();
            this.Count      = this.Stream.ReadVInt();

            if (this.Count > 0)
            {
                if (this.Count <= 512)
                {
                    this.Commands = new List<Command>(this.Count);

                    for (int I = 0; I < this.Count; I++)
                    {
                        Command Command = CommandManager.DecodeCommand(this.Stream);

                        if (Command == null)
                        {
                            break;
                        }

                        this.Commands.Add(Command);
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "Decode() - Command count is invalid (" + this.Count + ")");
                }
            }
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        internal override async void Process()
        {
            if (this.Commands != null)
            {
                this.Commands.ForEach(Command =>
                {
                    if (Command.ExecuteTick <= this.Tick)
                    {
                        this.Device.GameMode.CommandManager.AddCommand(Command);
                    }
                });
            }

            for (int I = this.Device.GameMode.Time; I < this.Tick; I++)
            {
                this.Device.GameMode.UpdateOneTick();
            }

            Player Player = this.Device.GameMode.Player;

            if (Player.IsInAlliance)
            {
                Clan Clan = await Clans.Get(Player.ClanHighId, Player.ClanLowId);

                if (Clan != null)
                {
                    if (Clan.Members.TryGetValue(Player.PlayerId, out var Member))
                    {
                        Member.Update(Player);
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "Player can't be updated, alliance instance was null.");
                }
            }

            /* 
            if (this.Device.GameMode.State == HomeState.Home)
            {
                if (this.Checksum != this.Device.GameMode.Checksum)
                {
                    Logging.Error(this.GetType(), "Player is out of sync (S: " + this.Device.GameMode.Checksum + ", C: " + this.Checksum + ").");
                    this.Device.NetworkManager.SendMessage(new OutOfSyncMessage(this.Device, this.Checksum, this.Device.GameMode.Checksum));
                }
            } */
        }
    }
}