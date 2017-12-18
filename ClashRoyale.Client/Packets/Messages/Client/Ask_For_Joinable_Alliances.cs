namespace ClashRoyale.Client.Packets.Messages.Client
{
    using ClashRoyale.Client.Logic;

    internal class Ask_For_Joinable_Alliances : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ask_For_Joinable_Alliances"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public Ask_For_Joinable_Alliances(Device Device) : base(Device)
        {
            this.Identifier = 14303;
        }
    }
}