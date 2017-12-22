namespace ClashRoyale
{
    public static class Config
    {
        public const string Environment     = "stage";

        public const bool IsDevelopment     = true;
        public const bool IsKunlunServer    = false;
        public const bool IsMaxedServer     = false;

        public const int ClientMajorVersion = 3;
        public const int ClientMinorVersion = 0;
        public const int ClientBuildVersion = 830;

        public const int ServerMajorVersion = 3;
        public const int ServerMinorVersion = 5;
        public const int ServerBuildVersion = 830;

        public const int BufferSize         = 2048 * 1;
        public const int ServerId           = 0;
        public const int MaxPlayers         = 10;

        public static string ClientVersion
        {
            get
            {
                return Config.ClientMajorVersion + "." + Config.ClientBuildVersion + "." + Config.ClientMinorVersion;
            }
        }

        public static string ServerVersion
        {
            get
            {
                return Config.ServerMajorVersion + "." + Config.ServerBuildVersion + "." + Config.ServerMinorVersion;
            }
        }
    }
}
