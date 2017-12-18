namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Commands;
    using ClashRoyale.Server.Logic.Commands.Manager;

    internal class SectorCommandMessage : Message
    {
        internal int ClientTick;
        internal int ClientChecksum;

        internal Command Command;

        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 12904;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Attack;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorCommandMessage"/> class.
        /// </summary>
        public SectorCommandMessage(Device Device, ByteStream Stream) : base(Device, Stream)
        {
            // SectorCommandMessage   
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.ClientChecksum = this.Stream.ReadVInt();
            this.ClientTick     = this.Stream.ReadVInt();

            if (!this.Stream.EndOfStream)
            {
                if (this.Stream.ReadBoolean())
                {
                    this.Command = CommandManager.DecodeCommand(this.Stream);
                }
            }
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            if (this.Device.GameMode.State == HomeState.Attack)
            {
                this.Device.GameMode.SectorManager.ReceiveSectorCommand(this.ClientTick, this.ClientChecksum, this.Command);
            }
            else
            {
                Logging.Info(this.GetType(), "SectorCommandMessage sendend in invalid state. (current state: " + this.Device.GameMode.State + ")");
            }
        }
    }
}