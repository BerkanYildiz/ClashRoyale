namespace ClashRoyale.Client.Network.Packets.Server
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Logic.Enums;
    using ClashRoyale.Extensions;

    internal class LoginOkMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginOkMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Stream">The stream.</param>
        public LoginOkMessage(Device Device, ByteStream Stream) : base(Device, Stream)
        {
            this.Device.State = State.LOGGED;
        }
    }
}