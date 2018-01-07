namespace ClashRoyale.Messages.Client.Avatar
{
    using ClashRoyale.Extensions;

    public class AvatarNameCheckRequestMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarNameCheckRequestMessage"/> class.
        /// </summary>
        public AvatarNameCheckRequestMessage()
        {
            // AvatarNameCheckRequestMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarNameCheckRequestMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public AvatarNameCheckRequestMessage(ByteStream Stream) : base(Stream)
        {
            // AvatarNameCheckRequestMessage.
        }
    }
}