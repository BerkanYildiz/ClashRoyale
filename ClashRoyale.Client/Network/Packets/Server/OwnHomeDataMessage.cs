namespace ClashRoyale.Client.Network.Packets.Server
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Extensions;

    internal class OwnHomeDataMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnHomeDataMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Stream">The stream.</param>
        public OwnHomeDataMessage(Device Device, ByteStream Stream) : base(Device, Stream)
        {
            // OwnHomeDataMessage.
        }
    }
}