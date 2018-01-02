namespace ClashRoyale.Server.Network.Packets.Client.Bind
{
    using ClashRoyale.Database;
    using ClashRoyale.Database.Models;
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Apis;
    using ClashRoyale.Messages;

    using MongoDB.Driver;

    internal class BindGoogleAccountMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 14997;
            }
        }

        /// <summary>
        /// The service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Friend;
            }
        }

        private string GoogleId;
        private string GoogleToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindGoogleAccountMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public BindGoogleAccountMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // BindGoogleAccountMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Stream.ReadVInt();

            this.GoogleId    = this.Stream.ReadString();
            this.GoogleToken = this.Stream.ReadString();
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        public override void Process()
        {
            ApiManager ApiManager = this.Device.GameMode.Player.ApiManager;

            if (string.IsNullOrEmpty(this.GoogleId) || string.IsNullOrEmpty(this.GoogleToken))
            {
                Logging.Warning(this.GetType(), "Either GoogleId or GoogleToken is null or empty. (GoogleId : " + this.GoogleId + ", GoogleToken : \"" + this.GoogleToken + "\".");

                // Temp Fix, Network can be null

                if (string.IsNullOrEmpty(this.GoogleId) == false)
                {
                    if (ApiManager.Google.Filled == false)
                    {
                        ApiManager.Google.Identifier = this.GoogleId;
                    }
                }
            }
            else
            {
                if (ApiManager.Google.Filled)
                {
                    if (this.GoogleId == ApiManager.Google.Identifier)
                    {
                        return;
                    }
                }

                var Matches         = GameDb.Players.Find(new JsonFilterDefinition<PlayerDb>("{'Data.api.google.ggId': '" + this.GoogleId + "'}"));
                var MatchesCount    = Matches.Count();

                if (MatchesCount == 1)
                {
                    /* Player Player   = JsonConvert.DeserializeObject<Player>(Matches.First().Profile.ToString(), Resources.Players.Settings);

                    if (Player.ApiManager.Google.Token == this.GoogleToken)
                    {
                        this.Device.NetworkManager.SendMessage(new DeviceAlreadyBoundMessage(this.Device, Player));
                    }
                    else
                    {
                        this.Device.NetworkManager.SendMessage(new DisconnectedMessage(this.Device)); // TODO : Implement anti-hack / ban system.
                    } */
                }
                else if (MatchesCount == 0)
                {
                    ApiManager.Google.Identifier = this.GoogleId;
                    ApiManager.Google.Token      = this.GoogleToken;
                }
                else
                {
                    // Error.
                }
            }
        }
    }
}