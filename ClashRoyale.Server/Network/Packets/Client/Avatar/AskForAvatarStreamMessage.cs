namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class AskForAvatarStreamMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 17101;
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
        /// Initializes a new instance of the <see cref="AskForAvatarStreamMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public AskForAvatarStreamMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // AskForAvatarStreamMessage.
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        public override void Process()
        {
            this.Device.NetworkManager.SendMessage(new AvatarStreamMessage(this.Device));
        }
    }
}