namespace ClashRoyale.Server.Network.Packets.Client.Bind
{
    using ClashRoyale.Server.Database;
    using ClashRoyale.Server.Database.Models;
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Apis;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Network.Packets.Server;

    using MongoDB.Driver;

    internal class BindFacebookAccount : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 14201;
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

        private bool Force;

        private string FbIdentifier;
        private string FbToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindFacebookAccount"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public BindFacebookAccount(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // BindFacebookAccount.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.Force        = this.Stream.ReadBoolean();
            this.FbIdentifier = this.Stream.ReadString();
            this.FbToken      = this.Stream.ReadString();
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        internal override async void Process()
        {
            if (string.IsNullOrEmpty(this.FbIdentifier) || string.IsNullOrEmpty(this.FbToken))
            {
                Logging.Warning(this.GetType(), "Either FbIdentifier or FbToken is null or empty.");
            }
            else
            {
                ApiManager ApiManager = this.Device.GameMode.Player.ApiManager;

                if (ApiManager.Facebook.Filled)
                {
                    if (this.FbIdentifier == ApiManager.Facebook.Identifier)
                    {
                        if (this.FbToken != ApiManager.Facebook.Token)
                        {
                            Logging.Warning(this.GetType(), "The FbTokens does not matches, aborting.");
                        }

                        return;
                    }
                }

                var Matches = GameDb.Players.Find(new JsonFilterDefinition<PlayerDb>("{'Data.api.facebook.fbId': '" + this.FbIdentifier + "'}"));
                var MatchesCount = Matches.Count();

                if (MatchesCount == 1)
                {
                    /* Player Player = JsonConvert.DeserializeObject<Player>(Matches.First().Profile.ToString(), Resources.Players.Settings);

                    if (Player.PlayerId == this.Device.GameMode.Player.PlayerId)
                    {
                        return;
                    }

                    if (Player.ApiManager.Facebook.Token == this.FbToken)
                    {
                        this.Device.NetworkManager.SendMessage(new FacebookAccountBoundMessage(this.Device));
                    }
                    else
                    {
                        if (this.Force)
                        {
                            var LiveInstance = await Players.Get(Player.HighId, Player.LowId, false);

                            if (LiveInstance != null)
                            {
                                Player = LiveInstance;
                            }

                            Player.ApiManager.Facebook.Reset();

                            await Players.Save(Player);

                            ApiManager.Facebook.Identifier  = this.FbIdentifier;
                            ApiManager.Facebook.Token       = this.FbToken;

                            this.Device.NetworkManager.SendMessage(new FacebookAccountBoundMessage(this.Device));
                        }
                        else
                        {
                            this.Device.NetworkManager.SendMessage(new FacebookAccountAlreadyBoundMessage(this.Device));
                        }
                    } */
                }
                else if (MatchesCount == 0)
                {
                    if (this.Force)
                    {
                        Logging.Warning(this.GetType(), "Force should've not been set to true.");
                    }

                    ApiManager.Facebook.Identifier = this.FbIdentifier;
                    ApiManager.Facebook.Token      = this.FbToken;

                    this.Device.NetworkManager.SendMessage(new FacebookAccountBoundMessage(this.Device));
                }
                else
                {
                    Logging.Error(this.GetType(), "More than 1 matches, aborting.");
                }
            }
        }
    }
}