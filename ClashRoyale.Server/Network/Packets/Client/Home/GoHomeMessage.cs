namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Messages;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class GoHomeMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 14560;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Home;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoHomeMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public GoHomeMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // Go_Home_Message.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Stream.ReadInt();
            this.Stream.ReadVInt();
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        public override async void Process()
        {
            if (this.Device.GameMode.State <= HomeState.Home)
            {
                var Player = await Players.Get(this.Device.NetworkManager.AccountId.HigherInt, this.Device.NetworkManager.AccountId.LowerInt);

                if (Player != null)
                {
                    Player.GameMode.Device.NetworkManager.SendMessage(new OwnHomeDataMessage(this.Device, Player));
                }
                else
                {
                    Logging.Error(this.GetType(), "Player was null at Process().");
                }
            }
            else
            {
                Logging.Error(this.GetType(), "State was not correct at Process().");
            }
        }
    }
}