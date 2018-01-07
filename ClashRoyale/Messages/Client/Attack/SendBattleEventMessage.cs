namespace ClashRoyale.Messages.Client.Attack
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Battle.Event;

    public class SendBattleEventMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 12951;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Attack;
            }
        }

        public BattleEvent BattleEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendBattleEventMessage"/> class.
        /// </summary>
        public SendBattleEventMessage()
        {
            // SendBattleEventMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SendBattleEventMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public SendBattleEventMessage(ByteStream Stream) : base(Stream)
        {
            // SendBattleEventMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.BattleEvent = new BattleEvent();
            this.BattleEvent.Decode(this.Stream);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.BattleEvent.Encode(this.Stream);
        }
    }
}