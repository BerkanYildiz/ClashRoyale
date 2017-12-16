namespace ClashRoyale.Server.Network.Packets.Client.Avatar
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Network.Packets.Server.Avatar;

    internal class AskForAvatarStreamMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 14405;
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
        /// Initializes a new instance of the <see cref="AskForAvatarStreamMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public AskForAvatarStreamMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // Ask_For_Avatar_Stream_Message.
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        internal override void Process()
        {
            this.Device.NetworkManager.SendMessage(new AvatarStreamMessage(this.Device));
        }
    }
}