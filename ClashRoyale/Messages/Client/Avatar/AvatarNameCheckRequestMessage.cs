namespace ClashRoyale.Messages.Client.Avatar
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages.Server.Avatar;

    public class AvatarNameCheckRequestMessage : Message
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
        public override void Process()
        {
            this.Device.NetworkManager.SendMessage(new AvatarNameCheckResponseMessage(this.Device));
        }
    }
}