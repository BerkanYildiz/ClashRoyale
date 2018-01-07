namespace ClashRoyale.Messages.Client.Avatar
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Maths;

    public class AskForBattleReplayStreamMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 15827;
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

        public LogicLong PlayerId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForBattleReplayStreamMessage"/> class.
        /// </summary>
        public AskForBattleReplayStreamMessage()
        {
            // AskForBattleReplayStreamMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForBattleReplayStreamMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public AskForBattleReplayStreamMessage(ByteStream Stream) : base(Stream)
        {
            // AskForBattleReplayStreamMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.PlayerId = this.Stream.DecodeLogicLong();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteLong(this.PlayerId);
        }
    }
}