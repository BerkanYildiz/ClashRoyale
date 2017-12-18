namespace ClashRoyale.Server.Network.Packets.Server
{
    using System;

    using ClashRoyale.Enums;
    using ClashRoyale.Extensions.Utils;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Home;
    using ClashRoyale.Server.Logic.Player;

    internal class OwnHomeDataMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 28502;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Home;
            }
        }

        private readonly Home Home;
        private readonly Player Player;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnHomeDataMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Player">The player.</param>
        public OwnHomeDataMessage(Device Device, Player Player) : base(Device)
        {
            this.Home   = Player.Home;
            this.Player = Player;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Home.Encode(this.Stream);
            this.Player.Encode(this.Stream);

            this.Stream.WriteVInt(TimeUtil.Timestamp);
            this.Stream.WriteVInt(TimeUtil.Timestamp);
            this.Stream.WriteVInt(TimeUtil.Timestamp);
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            this.Device.GameMode.LoadHomeState(this.Player, this.Home.SecondsSinceLastSave, 113);
            this.Device.GameMode.Home.LastTick = DateTime.UtcNow;
        }
    }
}