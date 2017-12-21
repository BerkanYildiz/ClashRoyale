namespace ClashRoyale.Client.Network.Packets.Server
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    internal class LoginOkMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginOkMessage"/> class.
        /// </summary>
        /// <param name="Bot">The bot.</param>
        /// <param name="Stream">The stream.</param>
        public LoginOkMessage(Bot Bot, ByteStream Stream) : base(Bot, Stream)
        {
            this.Bot.State = State.Logged;
        }
    }
}