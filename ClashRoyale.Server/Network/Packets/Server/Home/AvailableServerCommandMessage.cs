namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Commands;
    using ClashRoyale.Server.Logic.Commands.Manager;
    using ClashRoyale.Server.Logic.Enums;

    internal class AvailableServerCommandMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24111;
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

        private readonly Command Command;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableServerCommandMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Command">The command.</param>
        public AvailableServerCommandMessage(Device Device, Command Command) : base(Device)
        {
            this.Command    = Command;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            CommandManager.EncodeCommand(this.Command, this.Stream);
        }
    }
}