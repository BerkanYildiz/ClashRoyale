namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Server.Logic;

    internal class ServerHelloMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 20100;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerHelloMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        internal ServerHelloMessage(Device Device) : base(Device)
        {
            this.Device.State = State.SessionOk;
            this.Device.NetworkManager.PepperInit.SessionKey = new byte[24];
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteBytes(this.Device.NetworkManager.PepperInit.SessionKey);
        }
    }
}