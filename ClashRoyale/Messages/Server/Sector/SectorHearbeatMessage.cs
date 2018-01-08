namespace ClashRoyale.Messages.Server.Sector
{
    using ClashRoyale.Enums;
    using ClashRoyale.Exceptions;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Commands;
    using ClashRoyale.Logic.Commands.Manager;

    public class SectorHearbeatMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 21443;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Sector;
            }
        }

        public int ServerTurn;
        public int Checksum;

        public Command[] Commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorHearbeatMessage"/> class.
        /// </summary>
        public SectorHearbeatMessage()
        {
            // SectorHearbeatMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorHearbeatMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public SectorHearbeatMessage(ByteStream Stream) : base(Stream)
        {
            // SectorHearbeatMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorHearbeatMessage"/> class.
        /// </summary>
        /// <param name="ServerTurn">The server turn.</param>
        /// <param name="Checksum">The checksum.</param>
        /// <param name="Commands">The commands.</param>
        public SectorHearbeatMessage(int ServerTurn, int Checksum, Command[] Commands)
        {
            this.ServerTurn = ServerTurn;
            this.Checksum   = Checksum;
            this.Commands   = Commands;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.ServerTurn = this.Stream.ReadVInt();
            this.Checksum   = this.Stream.ReadVInt();

            this.Commands   = new Command[this.Stream.ReadVInt()];

            for (int i = 0; i < this.Commands.Length; i++)
            {
                Command Command = CommandManager.DecodeCommand(this.Stream);

                if (Command != null)
                {
                    this.Commands[i] = Command;
                }
                else
                {
                    throw new LogicException(this.GetType(), "Command == null at Decode().");
                }
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.ServerTurn);
            this.Stream.WriteVInt(this.Checksum);

            this.Stream.WriteVInt(this.Commands.Length);

            if (this.Commands.Length > 0)
            {
                ChecksumEncoder Encoder = new ChecksumEncoder(this.Stream);

                foreach (Command Command in this.Commands)
                {
                    if (Command != null)
                    {
                        CommandManager.EncodeCommand(Command, Encoder);
                    }
                    else
                    {
                        throw new LogicException(this.GetType(), "Command == null at Encode().");
                    }
                }
            }
        }
    }
}