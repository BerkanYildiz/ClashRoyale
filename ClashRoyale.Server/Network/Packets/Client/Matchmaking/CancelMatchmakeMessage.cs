namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Battle.Manager;
    using ClashRoyale.Messages;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class CancelMatchmakeMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 12269;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Matchmaking;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelMatchmakeMessage"/> class.
        /// </summary>
        public CancelMatchmakeMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // CancelMatchmakeMessage.
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        public override void Process()
        {
            Logging.Info(this.GetType(), "Player is canceling a matchmake.");

            if (BattleManager.Waitings.TryRemove(this.Device.GameMode.Player.PlayerId, out _))
            {
                this.Device.NetworkManager.SendMessage(new CancelMatchmakeDoneMessage(this.Device));
            }
            else
            {
                Logging.Info(this.GetType(), "Player is not allowed to cancel the matchmake.");
            }
        }
    }
}