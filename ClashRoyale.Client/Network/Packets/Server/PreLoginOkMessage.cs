namespace ClashRoyale.Client.Network.Packets.Server
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Network.Packets.Client;
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    using State = ClashRoyale.Client.Logic.Enums.State;

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
        /// <param name="Device">The device.</param>
        /// <param name="Stream">The stream.</param>
        public PreLoginOkMessage(Device Device, ByteStream Stream) : base(Device, Stream)
        {
            this.Device.State   = State.SESSION_OK;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.Device.PepperInit.SessionKey = this.Stream.ReadBytes();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            this.Device.Client.Gateway.Send(new LoginMessage(this.Device));
        }
    }
}