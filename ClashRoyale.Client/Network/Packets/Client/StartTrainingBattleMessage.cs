namespace ClashRoyale.Client.Network.Packets.Client
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv;
    using ClashRoyale.Files.Csv.Logic;

    internal class StartTrainingBattleMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 12393;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Attack;
            }
        }

        private NpcData NpcData;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartTrainingBattleMessage"/> class.
        /// </summary>
        public StartTrainingBattleMessage(Bot Bot) : base(Bot)
        {
            this.NpcData = CsvFiles.Get(Gamefile.Npcs).GetData<NpcData>("Npc47");
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.NpcData = this.Stream.DecodeData<NpcData>();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.EncodeData(this.NpcData);
        }
    }
}