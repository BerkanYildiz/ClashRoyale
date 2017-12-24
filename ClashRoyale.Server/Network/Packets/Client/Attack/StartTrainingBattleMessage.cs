namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Server.Logic;

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
        public StartTrainingBattleMessage(Device Device, ByteStream Stream) : base(Device, Stream)
        {
            // StartTrainingBattleMessage   
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.NpcData = this.Stream.DecodeData<NpcData>();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            if (this.Device.GameMode.State == HomeState.Home)
            {
                this.Device.GameMode.SectorManager.SendSectorState();
            }
            else
            {
                Logging.Info(this.GetType(), "State != HomeState.Home at Process().");
            }
        }
    }
}