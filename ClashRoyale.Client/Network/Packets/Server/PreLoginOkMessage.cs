namespace ClashRoyale.Client.Network.Packets.Server
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Network.Packets.Client;
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    internal class PreLoginOkMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 20100;
            }
        }

        /// <summary>
        /// The service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        private byte[] SessionKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreLoginOkMessage"/> class.
        /// </summary>
        /// <param name="Bot">The bot.</param>
        /// <param name="Stream">The stream.</param>
        public PreLoginOkMessage(Bot Bot, ByteStream Stream) : base(Bot, Stream)
        {
            this.Bot.State = State.SessionOk;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.Bot.Network.PepperInit.SessionKey = this.Stream.ReadBytes();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            this.Bot.Network.SendMessage(new LoginMessage(this.Bot));
        }
    }
}