namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Files;
    using ClashRoyale.Server.Logic;

    using Newtonsoft.Json;

    internal class AuthentificationFailedMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 20103;
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
        /// Gets the patching host according to the specified reason.
        /// </summary>
        private string ContentUrl
        {
            get
            {
                if (Fingerprint.IsCustom)
                {
                    return "https://raw.githubusercontent.com/BerkanYildiz/Patchs/master/ClashRoyale";
                }

                return "http://7166046b142482e67b30-2a63f4436c967aa7d355061bd0d924a1.r65.cf1.rackcdn.com";
            }
        }

        /// <summary>
        /// Gets the assets URL according to the specified reason.
        /// </summary>
        private string AssetsUrl
        {
            get
            {
                if (Fingerprint.IsCustom)
                {
                    return "https://game-assets.clashroyaleapp.com";
                }

                return "https://game-assets.clashroyaleapp.com";
            }
        }

        /// <summary>
        /// Gets the content update according to the specified reason.
        /// </summary>
        private string ContentUpdate
        {
            get
            {
                if (this.Reason == Reason.Patch)
                {
                    return Fingerprint.Json.ToString(Formatting.None);
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the redirect domain according to the specified reason.
        /// </summary>
        private string RedirectDomain
        {
            get
            {
                if (this.Reason == Reason.Redirection)
                {
                    return "game.clashroyaleapp.com";
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the update URL according to the specified reason.
        /// </summary>
        private string UpdateUrl
        {
            get
            {
                if (this.Reason == Reason.Update)
                {
                    return "https://mega.nz/#!E90UwDKa!e6Xv7bEXWSJDCyQy2o1PNqxONh0Q4qt3_rM5cks-pMo";
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the time left according to the specified reason.
        /// </summary>
        private int TimeLeft
        {
            get
            {
                if (this.Reason == Reason.Maintenance)
                {
                    return 3600;
                }

                if (this.Reason == Reason.Banned)
                {
                    return 604800;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets the message according to the specified reason.
        /// </summary>
        private string Message
        {
            get
            {
                if (this.Reason == Reason.Default)
                {
                    return "GobelinLand";
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the game urls.
        /// </summary>
        private string[] GameUrls
        {
            get
            {
                return new[]
                {
                    this.AssetsUrl, this.ContentUrl
                };
            }
        }

        private readonly Reason Reason;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthentificationFailedMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Reason">The reason.</param>
        public AuthentificationFailedMessage(Device Device, Reason Reason = Reason.Default) : base(Device)
        {
            this.Version = 4;
            this.Reason = Reason;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteVInt((int) this.Reason);

            this.Stream.WriteString(this.ContentUpdate);
            this.Stream.WriteString(this.UpdateUrl);
            this.Stream.WriteString(this.Message);
            this.Stream.WriteString(this.ContentUrl);

            this.Stream.WriteVInt(this.TimeLeft);
            this.Stream.WriteBoolean(false);

            this.Stream.WriteString(null);

            // Game Urls

            this.Stream.WriteVInt(this.GameUrls.Length);

            foreach (string GameUrl in this.GameUrls)
            {
                this.Stream.WriteString(GameUrl);
            }
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            this.Device.State = State.Disconnected;
        }
    }
}