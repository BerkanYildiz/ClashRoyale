namespace ClashRoyale.Client.Packets.Messages.Server
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Logic.Enums;

    internal class Authentification_OK : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Authentification_OK"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public Authentification_OK(Device Device, Reader Reader) : base(Device, Reader)
        {
            this.Device.State = State.LOGGED;
        }
    }
}