namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class AvatarNameChangeFailedMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 20205;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Avatar;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarNameChangeFailedMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public AvatarNameChangeFailedMessage(Device Device) : base(Device)
        {
            // AvatarNameChangeFailedMessage.
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteInt(0);
        }
    }
}