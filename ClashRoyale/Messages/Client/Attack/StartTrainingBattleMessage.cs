namespace ClashRoyale.Messages.Client.Attack
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;

    public class StartTrainingBattleMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 12393;
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

        public NpcData NpcData;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartTrainingBattleMessage"/> class.
        /// </summary>
        public StartTrainingBattleMessage()
        {
            // StartTrainingBattleMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartTrainingBattleMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public StartTrainingBattleMessage(ByteStream Stream) : base(Stream)
        {
            // StartTrainingBattleMessage   
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.NpcData = this.Stream.DecodeData<NpcData>();
        }
        
        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.EncodeData(this.NpcData);
        }
    }
}