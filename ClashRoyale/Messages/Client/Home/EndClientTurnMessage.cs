namespace ClashRoyale.Messages.Client.Home
{
    using System.Collections.Generic;

    using ClashRoyale.Enums;
    using ClashRoyale.Exceptions;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Commands;
    using ClashRoyale.Logic.Commands.Manager;

    public class EndClientTurnMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 18688;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Home;
            }
        }

        public int Tick;
        public int Checksum;
        public int Count;

        public List<Command> Commands;

        public byte[] Debug;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndClientTurnMessage"/> class.
        /// </summary>
        public EndClientTurnMessage()
        {
            // EndClientTurnMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndClientTurnMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public EndClientTurnMessage(ByteStream Stream) : base(Stream)
        {
            // EndClientTurnMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Tick       = this.Stream.ReadVInt();
            this.Checksum   = this.Stream.ReadVInt();
            this.Count      = this.Stream.ReadVInt();

            if (this.Count < 0)
            {
                throw new LogicException(this.GetType(), "Count < 0 at Decode().");
            }

            if (this.Count > 512)
            {
                throw new LogicException(this.GetType(), "Count > 512 at Decode().");
            }

            this.Commands   = new List<Command>(this.Count);

            if (this.Count > 0)
            {
                for (int I = 0; I < this.Count; I++)
                {
                    Command Command = CommandManager.DecodeCommand(this.Stream);

                    if (Command == null)
                    {
                        break;
                    }

                    Logging.Info(this.GetType(), " - " + Command.GetType().Name + ".");

                    this.Commands.Add(Command);
                }
            }
            else
            {
                this.Debug = this.Stream.ReadBytes();
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.Tick);
            this.Stream.WriteVInt(this.Checksum);
            this.Stream.WriteVInt(this.Count);

            foreach (var Command in this.Commands)
            {
                Command.Encode(this.Stream);
            }

            this.Stream.WriteBytes(this.Debug);
        }
    }
}