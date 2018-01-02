namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;

    internal class DeviceReloadMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 20132;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceReloadMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public DeviceReloadMessage(Device Device) : base(Device)
        {
            // DeviceReloadMessage.
        }
    }
}