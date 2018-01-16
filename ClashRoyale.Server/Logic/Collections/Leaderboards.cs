namespace ClashRoyale.Logic.Collections
{
    using System.Collections.Generic;

    using ClashRoyale.Enums;
    using ClashRoyale.Exceptions;
    using ClashRoyale.Files.Csv;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Scoring;

    public static class Leaderboards
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance has been already initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        public static LeaderboardPlayers GlobalPlayers;
        public static LeaderboardClans GlobalClans;

        public static Dictionary<string, LeaderboardPlayers> RegionalPlayers;
        public static Dictionary<string, LeaderboardClans> RegionalClans;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            if (Leaderboards.Initialized)
            {
                return;
            }

            Leaderboards.GlobalPlayers       = new LeaderboardPlayers();
            Leaderboards.GlobalClans         = new LeaderboardClans();

            Leaderboards.RegionalPlayers     = new Dictionary<string, LeaderboardPlayers>();
            Leaderboards.RegionalClans       = new Dictionary<string, LeaderboardClans>();

            foreach (RegionData Region in CsvFiles.Get(Gamefile.Regions).Datas)
            {
                Leaderboards.RegionalPlayers.Add(Region.Name, new LeaderboardPlayers(Region));
                Leaderboards.RegionalClans.Add(Region.Name, new LeaderboardClans(Region));
            }

            Leaderboards.Initialized         = true;
        }

        /// <summary>
        /// Gets the regional players.
        /// </summary>
        /// <param name="Region">The region.</param>
        public static LeaderboardPlayers GetRegionalPlayers(string Region)
        {
            #if DEBUG
            if (string.IsNullOrEmpty(Region))
            {
                throw new LogicException(typeof(Leaderboards), "Region == null at GetRegionalPlayers(Region).");
            }

            #endif

            if (Region.Contains("-"))
            {
                Region = Region.Split('-')[0];
            }

            Region = Region.ToUpper();

            if (Leaderboards.RegionalPlayers.ContainsKey(Region))
            {
                return Leaderboards.RegionalPlayers[Region];
            }

            return null;
        }

        /// <summary>
        /// Gets the regional clans.
        /// </summary>
        /// <param name="Region">The region.</param>
        public static LeaderboardClans GetRegionalClans(string Region)
        {
            #if DEBUG
            if (string.IsNullOrEmpty(Region))
            {
                throw new LogicException(typeof(Leaderboards), "Region == null at GetRegionalClans(Region).");
            }
            #endif

            if (Region.Contains("-"))
            {
                Region = Region.Split('-')[0];
            }

            Region = Region.ToUpper();

            if (Leaderboards.RegionalClans.ContainsKey(Region))
            {
                return Leaderboards.RegionalClans[Region];
            }

            return null;
        }
    }
}