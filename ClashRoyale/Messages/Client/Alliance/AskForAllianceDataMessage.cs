namespace ClashRoyale.Messages.Client.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Alliance;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Messages.Server.Alliance;

    public class AskForAllianceDataMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 10609;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Alliance;
            }
        }

        private int AllianceHighId;
        private int AllianceLowId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForAllianceDataMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public AskForAllianceDataMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // AskForAllianceDataMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.AllianceHighId = this.Stream.ReadInt();
            this.AllianceLowId  = this.Stream.ReadInt();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        public override async void Process()
        {
            Clan Clan = await Clans.Get(this.AllianceHighId, this.AllianceLowId);

            Logging.Info(this.GetType(), "Trying to retrieve a clan from the database (" + this.AllianceHighId + "-" + this.AllianceLowId + ").");

            if (Clan != null)
            {
                this.Device.NetworkManager.SendMessage(new AllianceDataMessage(this.Device, Clan));
            }
            else
            {
                Logging.Warning(this.GetType(), "Tried to retrieve a clan from the database, null value returned.");
            }
        }
    }
}