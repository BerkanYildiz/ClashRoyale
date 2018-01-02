namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;

    internal class AvatarNameCheckResponseMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 20300;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Avatar;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarNameCheckResponseMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public AvatarNameCheckResponseMessage(Device Device) : base(Device)
        {
            // AvatarNameCheckResponseMessage.
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteByte(0);
            this.Stream.WriteByte(0);
            this.Stream.WriteString(string.Empty);
        }
    }
}