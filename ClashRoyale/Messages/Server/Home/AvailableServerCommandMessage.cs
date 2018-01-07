namespace ClashRoyale.Messages.Server.Home
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Commands;
    using ClashRoyale.Logic.Commands.Manager;

    public class AvailableServerCommandMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 23394;
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

        public Command Command;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableServerCommandMessage"/> class.
        /// </summary>
        public AvailableServerCommandMessage()
        {
            // AvailableServerCommandMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableServerCommandMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public AvailableServerCommandMessage(ByteStream Stream) : base(Stream)
        {
            // AvailableServerCommandMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableServerCommandMessage"/> class.
        /// </summary>
        /// <param name="Command">The command.</param>
        public AvailableServerCommandMessage(Command Command)
        {
            this.Command = Command;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Command = CommandManager.DecodeCommand(this.Stream);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            CommandManager.EncodeCommand(this.Command, this.Stream);
        }
    }
}