namespace ClashRoyale.Messages.Server.Account
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class DeviceReloadMessage : Message
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
        public DeviceReloadMessage()
        {
            // DeviceReloadMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceReloadMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public DeviceReloadMessage(ByteStream Stream) : base(Stream)
        {
            // DeviceReloadMessage.
        }
    }
}