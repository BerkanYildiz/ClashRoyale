namespace ClashRoyale.Server.Network.Packets.Client.Bind
{
    using ClashRoyale.Server.Database;
    using ClashRoyale.Server.Database.Models;
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Apis;
    using ClashRoyale.Server.Logic.Enums;

    using MongoDB.Driver;

    internal class BindGamecenterAccount : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 14212;
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

        private string GamecenterId;
        private string Certificate;
        private string AppBundle;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindGamecenterAccount"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public BindGamecenterAccount(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // BindGamecenterAccount.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.Stream.ReadVInt();

            this.GamecenterId   = this.Stream.ReadString();
            this.Certificate    = this.Stream.ReadString();
            this.AppBundle      = this.Stream.ReadString();
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        internal override void Process()
        {
            if (string.IsNullOrEmpty(this.GamecenterId) || string.IsNullOrEmpty(this.Certificate))
            {
                Logging.Warning(this.GetType(), "Either GamecenterId or Certificate is null or empty.");
            }
            else
            {
                ApiManager ApiManager   = this.Device.GameMode.Player.ApiManager;

                if (ApiManager.Gamecenter.Filled)
                {
                    if (this.GamecenterId == ApiManager.Gamecenter.Identifier)
                    {
                        return;
                    }
                }

                var Matches         = GameDb.Players.Find(new JsonFilterDefinition<PlayerDb>("{'Data.api.gamecenter.gcId': '" + this.GamecenterId + "'}"));
                var MatchesCount    = Matches.Count();

                if (MatchesCount == 1)
                {
                    /* Player Player   = JsonConvert.DeserializeObject<Player>(Matches.First().Data.ToString(), Resources.Players.Settings);

                    if (Player.ApiManager.Gamecenter.Certificate == this.Certificate)
                    {
                        new DeviceAlreadyBoundMessage(this.Device, Player));
                    }
                    else
                    {
                        new DisconnectedMessage(this.Device)); // TODO : Implement anti-hack / ban system.
                    } */
                }
                else if (MatchesCount == 0)
                {
                    ApiManager.Gamecenter.Identifier    = this.GamecenterId;
                    ApiManager.Gamecenter.Certificate   = this.Certificate;
                    ApiManager.Gamecenter.AppBundle     = this.AppBundle;
                }
                else
                {
                    Logging.Error(this.GetType(), "More than 1 matches, aborting.");
                }
            }
        }
    }
}