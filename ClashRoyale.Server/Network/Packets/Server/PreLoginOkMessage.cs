namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class PreLoginOkMessage : Message
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
        /// Initializes a new instance of the <see cref="PreLoginOkMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        internal PreLoginOkMessage(Device Device) : base(Device)
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