namespace ClashRoyale.Server.Logic.Scoring
{
    using System.Collections.Generic;

    using ClashRoyale.Server.Files.Csv;
    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal static class Leaderboards
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance has been already initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            set;
        }

        internal static LeaderboardPlayers GlobalPlayers;
        internal static LeaderboardClans GlobalClans;

        internal static Dictionary<string, LeaderboardPlayers> RegionalPlayers;
        internal static Dictionary<string, LeaderboardClans> RegionalClans;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            if (Leaderboards.Initialized)
            {
                return;
            }

            GlobalPlayers       = new LeaderboardPlayers();
            GlobalClans         = new LeaderboardClans();

            RegionalPlayers     = new Dictionary<string, LeaderboardPlayers>();
            RegionalClans       = new Dictionary<string, LeaderboardClans>();

            foreach (RegionData Region in CsvFiles.Get(Gamefile.Regions).Datas)
            {
                RegionalPlayers.Add(Region.Name, new LeaderboardPlayers(Region));
                RegionalClans.Add(Region.Name, new LeaderboardClans(Region));
            }

            Initialized         = true;
        }

        /// <summary>
        /// Gets the regional players.
        /// </summary>
        /// <param name="Region">The region.</param>
        internal static LeaderboardPlayers GetRegionalPlayers(string Region)
        {
            if (RegionalPlayers.ContainsKey(Region))
            {
                return RegionalPlayers[Region];
            }

            return null;
        }

        /// <summary>
        /// Gets the regional clans.
        /// </summary>
        /// <param name="Region">The region.</param>
        internal static LeaderboardClans GetRegionalClans(string Region)
        {
            if (RegionalClans.ContainsKey(Region))
            {
                return RegionalClans[Region];
            }

            return null;
        }
    }
}