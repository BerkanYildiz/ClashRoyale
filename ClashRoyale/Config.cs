namespace ClashRoyale
{
    using System;

    public static class Config
    {
        public const string Environment     = "stage";

        public const bool IsMaxedServer     = false;
        public const bool IsDevelopment     = true;
        public const bool IsKunlunServer    = false;

        public const int ClientMajorVersion = 3;
        public const int ClientMinorVersion = 0;
        public const int ClientBuildVersion = 830;

        public const int ServerMajorVersion = 3;
        public const int ServerMinorVersion = 5;
        public const int ServerBuildVersion = 830;

        public const int BufferSize         = 2048 * 1;
        public const int ServerId           = 0;
        public const int MaxPlayers         = 10;
        public const int MaxChatEntries     = 100;

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

        public static class Maintenance
        {
            /// <summary>
            /// Gets a value indicating whether this <see cref="Maintenance"/> is enabled.
            /// </summary>
            public static bool Enabled
            {
                get
                {
                    return Maintenance.TimeLeft.Seconds > 0;
                }
            }

            /// <summary>
            /// Gets or sets the time when the maintenance event starts.
            /// </summary>
            public static DateTime StartTime
            {
                get
                {
                    return Warning.EndTime;
                }
            }

            /// <summary>
            /// Gets the time on which the maintenance ends.
            /// </summary>
            public static DateTime EndTime
            {
                get
                {
                    return Maintenance.StartTime.Add(Maintenance.Cooldown);
                }
            }

            /// <summary>
            /// Gets the time left before the programs ends the maintenance event.
            /// </summary>
            public static TimeSpan TimeLeft
            {
                get
                {
                    return Maintenance.EndTime - DateTime.UtcNow;
                }
            }
            
            /// <summary>
            /// Gets or sets the timespam defining when the maintenance is going to end 
            /// </summary>
            public static TimeSpan Cooldown
            {
                get;
                set;
            }

            /// <summary>
            /// Begins a maintenance event using specified cooldown.
            /// </summary>
            /// <param name="Cooldown">The cooldown.</param>
            public static void Begin(TimeSpan Cooldown)
            {
                Maintenance.Cooldown = Cooldown;
                Warning.StartTime    = DateTime.UtcNow;
            }

            /// <summary>
            /// Resets and ends the current maintenance.
            /// </summary>
            public static void Reset()
            {
                Maintenance.Cooldown = TimeSpan.Zero;
            }

            public static class Warning
            {
                /// <summary>
                /// Gets a value indicating whether a maintenance warning event is being executed.
                /// </summary>
                public static bool IsInProgress
                {
                    get
                    {
                        return Warning.TimeLeft.Seconds > 0;
                    }
                }

                /// <summary>
                /// Gets or sets the time when the maintenance warning message is being sent.
                /// </summary>
                public static DateTime StartTime
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets the time on which the program disconnects everyone.
                /// </summary>
                public static DateTime EndTime
                {
                    get
                    {
                        return Warning.StartTime.AddMinutes(5);
                    }
                }

                /// <summary>
                /// Gets the time left before the program disconnects everyone.
                /// </summary>
                public static TimeSpan TimeLeft
                {
                    get
                    {
                        return Warning.EndTime - DateTime.UtcNow;
                    }
                }
            }
        }
    }
}
