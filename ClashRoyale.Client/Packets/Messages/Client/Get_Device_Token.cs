namespace ClashRoyale.Client.Packets.Messages.Client
{
    using ClashRoyale.Client.Logic;

    internal class Get_Device_Token : Message
    {
        internal string Password;

        /// <summary>
        /// Initializes a new instance of the <see cref="Get_Device_Token"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public Get_Device_Token(Device Device) : base(Device)
        {
            this.Identifier = 10113;
        }

        /// <summary>
        /// Encodes the <see cref="Message" />, using the <see cref="Writer" /> instance.
        /// </summary>
        internal override void Encode()
        {
            this.Data.AddString(this.Password);
            this.Data.AddBool(true);
        }
    }
}