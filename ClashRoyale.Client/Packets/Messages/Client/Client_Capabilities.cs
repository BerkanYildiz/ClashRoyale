namespace ClashRoyale.Client.Packets.Messages.Client
{
    using ClashRoyale.Client.Logic;

    internal class Client_Capabilities : Message
    {
        private int Ping;
        private string Interface;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client_Capabilities"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public Client_Capabilities(Device Device) : base(Device)
        {
            this.Identifier = 10107;
        }

        /// <summary>
        /// Encodes the <see cref="Message" />, using the <see cref="Writer" /> instance.
        /// </summary>
        internal override void Encode()
        {
            this.Data.AddVInt(this.Ping);
            this.Data.AddString(this.Interface);
        }
    }
}