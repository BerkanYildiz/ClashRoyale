namespace ClashRoyale.Messages.Client.Attack
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Commands;
    using ClashRoyale.Logic.Commands.Manager;

    public class SectorCommandMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 12904;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Attack;
            }
        }

        public int Tick;
        public int Checksum;

        public Command Command;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorCommandMessage"/> class.
        /// </summary>
        public SectorCommandMessage()
        {
            // SectorCommandMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorCommandMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public SectorCommandMessage(ByteStream Stream) : base(Stream)
        {
            // SectorCommandMessage   
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Checksum = this.Stream.ReadVInt();
            this.Tick     = this.Stream.ReadVInt();

            if (!this.Stream.EndOfStream)
            {
                if (this.Stream.ReadBoolean())
                {
                    this.Command = CommandManager.DecodeCommand(this.Stream);
                }
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.Checksum);
            this.Stream.WriteVInt(this.Tick);

            this.Stream.WriteBoolean(this.Command != null);

            if (this.Command != null)
            {
                this.Command.Encode(this.Stream);
            }
        }
    }
}