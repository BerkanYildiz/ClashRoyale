namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class AvatarNameCheckRequestMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarNameCheckRequestMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public AvatarNameCheckRequestMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // AvatarNameCheckRequestMessage.
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        internal override void Process()
        {
            this.Device.NetworkManager.SendMessage(new AvatarNameCheckResponseMessage(this.Device));
        }
    }
}