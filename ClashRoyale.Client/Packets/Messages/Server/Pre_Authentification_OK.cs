namespace ClashRoyale.Client.Packets.Messages.Server
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Logic.Enums;
    using ClashRoyale.Client.Packets.Messages.Client;

    internal class Pre_Authentification_OK : Message
    {
        private byte[] SessionKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pre_Authentification_OK"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public Pre_Authentification_OK(Device Device, Reader Reader) : base(Device, Reader)
        {
            this.Device.State   = State.SESSION_OK;
        }

        /// <summary>
        /// Decodes the <see cref="Message" />, using the <see cref="Reader" /> instance.
        /// </summary>
        internal override void Decode()
        {
            this.Device.PepperInit.SessionKey = this.Reader.ReadArray();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            this.Device.Client.Gateway.Send(new Authentification(this.Device));
        }
    }
}