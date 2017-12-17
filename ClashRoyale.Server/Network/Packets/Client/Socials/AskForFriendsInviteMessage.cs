namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class AskForFriendsInviteMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 10503;
            }
        }

        /// <summary>
        /// The service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Friend;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForFriendsInviteMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public AskForFriendsInviteMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // AskForFriendsInviteMessage.
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        internal override void Process()
        {
            this.Device.NetworkManager.SendMessage(new FriendsInviteDataMessage(this.Device, this.Device.GameMode.Player.ToString()));
        }
    }
}