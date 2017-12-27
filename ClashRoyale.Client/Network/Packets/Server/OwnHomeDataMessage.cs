namespace ClashRoyale.Client.Network.Packets.Server
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Network.Packets.Client;
    using ClashRoyale.Extensions;

    internal class OwnHomeDataMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnHomeDataMessage"/> class.
        /// </summary>
        /// <param name="Bot">The bot.</param>
        /// <param name="Stream">The stream.</param>
        public OwnHomeDataMessage(Bot Bot, ByteStream Stream) : base(Bot, Stream)
        {
            // OwnHomeDataMessage.
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            this.Bot.Network.SendMessage(new RequestApiKeyMessage(this.Bot));
            this.Bot.Network.SendMessage(new ChangeAvatarNameMessage(this.Bot));
        }
    }
}