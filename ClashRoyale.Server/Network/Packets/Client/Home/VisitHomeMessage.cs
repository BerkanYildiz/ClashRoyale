namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.Player;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class VisitHomeMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 14113;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Home;
            }
        }

        private int HighId;
        private int LowId;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisitHomeMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public VisitHomeMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // VisitHomeMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.HighId = this.Stream.ReadInt();
            this.LowId  = this.Stream.ReadInt();
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        internal override async void Process()
        {
            Player Player = await Players.Get(this.HighId, this.LowId, false);

            Logging.Info(this.GetType(), "Player is requesting a profile.");

            if (Player != null)
            {
                this.Device.NetworkManager.SendMessage(new VisitedHomeDataMessage(this.Device, Player));
            }
            else
            {
                Logging.Error(this.GetType(), "Player(" + this.HighId + "-" + this.LowId + ") == null at Process().");
            }
        }
    }
}