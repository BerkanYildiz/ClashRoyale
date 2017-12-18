namespace ClashRoyale.Client.Packets.Messages.Client
{
    using ClashRoyale.Client.Logic;

    internal class Go_Home : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Go_Home"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public Go_Home(Device Device) : base(Device)
        {
            this.Identifier = 14101;
        }

        /// <summary>
        /// Encodes the <see cref="Message" />, using the <see cref="Writer" /> instance.
        /// </summary>
        internal override void Encode()
        {
            this.Data.AddInt(0);
            this.Data.AddInt(0);
        }
    }
}