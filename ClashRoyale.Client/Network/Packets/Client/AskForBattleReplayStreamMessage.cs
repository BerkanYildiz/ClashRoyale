namespace ClashRoyale.Client.Network.Packets.Client
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Maths;

    internal class AskForBattleReplayStreamMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 15827;
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

        private LogicLong PlayerId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForBattleReplayStreamMessage"/> class.
        /// </summary>
        /// <param name="Bot">The bot.</param>
        public AskForBattleReplayStreamMessage(Bot Bot) : base(Bot)
        {
            // AskForBattleReplayStreamMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.PlayerId = this.Stream.DecodeLogicLong();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteLong(this.PlayerId);
        }
    }
}