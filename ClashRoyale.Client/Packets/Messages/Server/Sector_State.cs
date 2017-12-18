namespace ClashRoyale.Client.Packets.Messages.Server
{
    using ClashRoyale.Client.Logic;

    internal class Sector_State : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sector_State"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public Sector_State(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Sector_State.
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            // this.Device.Client.Gateway.Send(new Go_Home(this.Device));
        }
    }
}