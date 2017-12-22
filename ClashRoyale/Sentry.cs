namespace ClashRoyale
{
    using SharpRaven;

    public static class Sentry
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Sentry"/> has been already initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        public static RavenClient Raven;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            if (Sentry.Initialized)
            {
                return;
            }

            Sentry.Raven = new RavenClient("https://a3b287a6f45b43cda0cd3ea4c9e6b2bc:04228d445b414ac6bd920cc947d02e35@sentry.io/263200")
            {
                Environment = Config.Environment,
                Release     = Config.ServerVersion,
                Logger      = "ClashRoyale"
            };

            Sentry.Raven.Tags.Add("serverId", Config.ServerId.ToString());
            Sentry.Raven.Tags.Add("maxPlayers", Config.MaxPlayers.ToString());
            Sentry.Raven.Tags.Add("bufferSize", Config.BufferSize.ToString());
            Sentry.Raven.Tags.Add("isDev", Config.IsDevelopment.ToString());
            Sentry.Raven.Tags.Add("isKunlun", Config.IsKunlunServer.ToString());
            Sentry.Raven.Tags.Add("isMaxed", Config.IsMaxedServer.ToString());

            Sentry.Initialized = true;
        }
    }
}
