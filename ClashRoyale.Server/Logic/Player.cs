namespace ClashRoyale.Server.Logic
{
    using System;

    using ClashRoyale.Server.Logic.Enums;

    using Newtonsoft.Json;

    internal class Player
    {
        [JsonProperty("highId")]            internal int HighId;
        [JsonProperty("lowId")]             internal int LowId;
        
        [JsonProperty("token")]             internal string Token;

        [JsonProperty("clanHighId")]        internal int ClanHighId;
        [JsonProperty("clanLowId")]         internal int ClanLowId;

        [JsonProperty("isDemoAccount")]     internal bool IsDemoAccount;
        [JsonProperty("isNameSet")]         internal bool IsNameSet;

        [JsonProperty("diamonds")]          internal int Diamonds;
        [JsonProperty("freeDiamonds")]      internal int FreeDiamonds;
        [JsonProperty("score")]             internal int Score;
        [JsonProperty("score2v2")]          internal int Score2V2;
        [JsonProperty("npcWins")]           internal int NpcWins;
        [JsonProperty("npcLosses")]         internal int NpcLosses;
        [JsonProperty("pvpWins")]           internal int PvPWins;
        [JsonProperty("pvpLosses")]         internal int PvPLosses;
        [JsonProperty("battles")]           internal int Battles;
        [JsonProperty("tbattles")]          internal int TotalBattles;

        [JsonProperty("lastTourScore")]     internal int LastTournamentScore;
        [JsonProperty("lastTourBestScore")] internal int LastTournamentBestScore;
        [JsonProperty("lastTourHighId")]    internal int LastTournamentHighId;
        [JsonProperty("lastTourLowId")]     internal int LastTournamentLowId;

        [JsonProperty("expLevel")]          internal int ExpLevel;
        [JsonProperty("expPoints")]         internal int ExpPoints;
        [JsonProperty("nameChangeState")]   internal int NameChangeState;
        [JsonProperty("allianceRole")]      internal int AllianceRole;

        [JsonProperty("name")]              internal string Name;
        [JsonProperty("allianceName")]      internal string AllianceName;

        [JsonProperty("arena")]             internal ArenaData Arena;
        [JsonProperty("lastTourArena")]     internal ArenaData LastTournamentArena;
        [JsonProperty("location")]          internal LocaleData Location;
        [JsonProperty("clanBadge")]         internal AllianceBadgeData Badge;

        [JsonProperty("commodities")]       internal CommoditySlots CommoditySlots;

        [JsonProperty("home")]              internal Home Home;
        [JsonProperty("debugRank")]         internal Rank Rank;

        [JsonProperty("save")]              internal DateTime Update    = DateTime.UtcNow;
        [JsonProperty("creation")]          internal DateTime Created   = DateTime.UtcNow;
        [JsonProperty("ban")]               internal DateTime Ban       = DateTime.UtcNow;

        /// <summary>
        /// Gets the player identifier.
        /// </summary>
        internal long PlayerId
        {
            get
            {
                return (long) this.HighId << 32 | (uint) this.LowId;
            }
        }

        /// <summary>
        /// Gets the alliance identifier.
        /// </summary>
        internal long ClanId
        {
            get
            {
                return (long) this.ClanHighId << 32 | (uint) this.ClanLowId;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has been already initialized.
        /// </summary>
        internal bool IsInitialized
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        internal Player()
        {
            this.Home = new Home(this);
        }

        /// <summary>
        /// Gets a value indicating whether the player is in an alliance.
        /// </summary>
        internal bool IsInAlliance
        {
            get
            {
                return this.ClanLowId > 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this player is banned.
        /// </summary>
        internal bool IsBanned
        {
            get
            {
                return this.Ban > DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Gets the time left in seconds before the ban ends.
        /// </summary>
        internal int BanSecondsLeft
        {
            get
            {
                return (int) this.Ban.Subtract(DateTime.UtcNow).TotalSeconds;
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal void Initialize()
        {
            
        }
    }
}
