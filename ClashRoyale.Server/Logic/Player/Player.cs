namespace ClashRoyale.Server.Logic.Player
{
    using System;

    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Game;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv;
    using ClashRoyale.Files.Csv.Client;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.Apis;
    using ClashRoyale.Server.Logic.Home;
    using ClashRoyale.Server.Logic.Mode;
    using ClashRoyale.Server.Logic.Player.Enums;
    using ClashRoyale.Server.Logic.Player.Slots;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Math = ClashRoyale.Maths.Math;

    internal class Player : PlayerBase
    {
        internal GameMode GameMode;

        // Id

        [JsonProperty("accountHigh")]           internal int HighId;
        [JsonProperty("accountLow")]            internal int LowId;
        
        [JsonProperty("accountToken")]          internal string Token;
        [JsonProperty("accountLocation")]       internal LocaleData AccountLocation;

        [JsonProperty("clanHighId")]            internal int ClanHighId;
        [JsonProperty("clanLowId")]             internal int ClanLowId;

        [JsonProperty("isDemoAccount")]         internal bool IsDemoAccount;

        // Game
        
        [JsonProperty("isNameSet")]             internal bool IsNameSet;

        [JsonProperty("diamonds")]              internal int Diamonds;
        [JsonProperty("freeDiamonds")]          internal int FreeDiamonds;
        [JsonProperty("score")]                 internal int Score;
        [JsonProperty("score2v2")]              internal int Score2V2;
        [JsonProperty("npcWins")]               internal int NpcWins;
        [JsonProperty("npcLosses")]             internal int NpcLosses;
        [JsonProperty("pvpWins")]               internal int PvPWins;
        [JsonProperty("pvpLosses")]             internal int PvPLosses;
        [JsonProperty("battles")]               internal int Battles;
        [JsonProperty("tbattles")]              internal int TotalBattles;

        [JsonProperty("lastTourScore")]         internal int LastTournamentScore;
        [JsonProperty("lastTourBestScore")]     internal int LastTournamentBestScore;
        [JsonProperty("lastTourHighId")]        internal int LastTournamentHighId;
        [JsonProperty("lastTourLowId")]         internal int LastTournamentLowId;

        [JsonProperty("expLevel")]              internal int ExpLevel;
        [JsonProperty("expPoints")]             internal int ExpPoints;
        [JsonProperty("nameChangeState")]       internal int NameChangeState;
        [JsonProperty("clanRole")]              internal int AllianceRole;

        [JsonProperty("username")]              internal string Name;
        [JsonProperty("clanName")]              internal string AllianceName;

        [JsonProperty("arena")]                 internal ArenaData Arena;
        [JsonProperty("lastTourArena")]         internal ArenaData LastTournamentArena;
        [JsonProperty("clanBadge")]             internal AllianceBadgeData Badge;

        [JsonProperty("commodities")]           internal CommoditySlots CommoditySlots;

        // DateTime

        [JsonProperty("save")]                  internal DateTime Update    = DateTime.UtcNow;
        [JsonProperty("creation")]              internal DateTime Created   = DateTime.UtcNow;
        [JsonProperty("ban")]                   internal DateTime Ban       = DateTime.UtcNow;

        // Home
        
        [JsonProperty("home")]                  internal Home Home;

        // Debug
        
        [JsonProperty("debugRank")]            internal Rank Rank = Rank.Administrator;

        // Apis
        
        [JsonProperty("api")]                   internal ApiManager ApiManager;

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
        internal long AllianceId
        {
            get
            {
                return (long) this.ClanHighId << 32 | (uint) this.ClanLowId;
            }
        }
        
        /// <summary>
        /// Gets a value indicating whether the player is connected.
        /// </summary>
        internal bool IsConnected
        {
            get
            {
                if (this.GameMode != null)
                {
                    return this.GameMode.IsConnected;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the player is in an alliance.
        /// </summary>
        internal bool IsInAlliance
        {
            get
            {
                return this.AllianceId != 0;
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
        /// Gets this instance generated checksum.
        /// </summary>
        internal override int Checksum
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the number of gold.
        /// </summary>
        internal int Gold
        {
            get
            {
                return this.CommoditySlots.GetCommodityCount(CommodityType.Resource, CsvFiles.GoldData);
            }
        }

        /// <summary>
        /// Gets the number of max gold.
        /// </summary>
        internal int MaxGold
        {
            get
            {
                return CsvFiles.GoldData.Cap;
            }
        }

        /// <summary>
        /// Gets the number of max score.
        /// </summary>
        internal int MaxScore
        {
            get
            {
                return this.CommoditySlots.GetCommodityCount(CommodityType.Resource, CsvFiles.Get(Gamefile.Resource).GetData<ResourceData>("MaxScore"));
            }
        }

        /// <summary>
        /// Gets the count of star.
        /// </summary>
        internal int StarCount
        {
            get
            {
                return this.CommoditySlots.GetCommodityCount(CommodityType.Resource, CsvFiles.StarCountData);
            }
        }

        /// <summary>
        /// Gets the experience level data.
        /// </summary>
        internal CsvData ExpLevelData
        {
            get
            {
                return CsvFiles.Get(Gamefile.ExpLevel).GetWithInstanceId(this.ExpLevel - 1);
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        internal Player()
        {
            this.Home           = new Home();
            this.CommoditySlots = new CommoditySlots();
            this.ApiManager     = new ApiManager();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="HighId">The high identifier.</param>
        /// <param name="LowId">The low identifier.</param>
        internal Player(int HighId, int LowId) : this()
        {
            this.HighId         = HighId;
            this.LowId          = LowId;

            this.Initialize();
        }

        /// <summary>
        /// Initializes values of members.
        /// </summary>
        internal void Initialize()
        {
            this.CommoditySlots.Initialize();

            this.ExpLevel       = 1;
            this.Diamonds       = Globals.StartingDiamonds;
            this.FreeDiamonds   = Globals.StartingDiamonds;

            this.Arena          = CsvFiles.Get(Gamefile.Arena).GetWithInstanceId<ArenaData>(1);
        }

        /// <summary>
        /// Adds free diamond.
        /// </summary>
        internal void AddFreeDiamonds(int Count)
        {
            if (Count > 0)
            {
                this.Diamonds += Count;
                this.FreeDiamonds += Count;
            }
        }

        /// <summary>
        /// Adds free gold.
        /// </summary>
        internal void AddFreeGold(int Count)
        {
            if (Count > 0)
            {
                this.CommoditySlots.AddCommodityCount(CommodityType.Resource, CsvFiles.GoldData, Count);
                this.CommoditySlots.AddCommodityCount(CommodityType.Resource, CsvFiles.FreeGoldData, Count);
            }
        }

        /// <summary>
        /// Gets the achievement progress by data.
        /// </summary>
        internal int GetAchievementProgress(CsvData CsvData)
        {
            return this.CommoditySlots.GetCommodityCount(CommodityType.AchievementProgress, CsvData);
        }

        /// <summary>
        /// Gets if the specified achievement reward has been claimed.
        /// </summary>
        internal bool GetIsAchievementRewardClaimed(AchievementData Data)
        {
            return this.CommoditySlots.Exists(3, Data);
        }

        /// <summary>
        /// Gets if the player has enough diamonds.
        /// </summary>
        internal bool HasEnoughDiamonds(int Count)
        {
            return this.Diamonds >= Count;
        }

        /// <summary>
        /// Gets if the player has enough resources.
        /// </summary>
        internal bool HasEnoughResources(ResourceData Data, int Count)
        {
            return this.CommoditySlots.GetCommodityCount(CommodityType.Resource, Data) >= Count;
        }

        /// <summary>
        /// Increases the number of three crown wins.
        /// </summary>
        internal void IncreaseThreeCrownWins()
        {
            this.CommoditySlots.AddCommodityCount(CommodityType.Resource, CsvFiles.Get(Gamefile.Resource).GetData<ResourceData>("ThreeCrownWins"), 1);
        }

        /// <summary>
        /// Gets if the specified achievement is completed.
        /// </summary>
        internal bool IsAchievementCompleted(AchievementData Data)
        {
            return Data.ActionCount <= this.GetAchievementProgress(Data);
        }

        /// <summary>
        /// Levels up this player.
        /// </summary>
        private void LevelUp(int ExpLevel)
        {
            ExpLevelData Data = CsvFiles.Get(Gamefile.ExpLevel).GetWithInstanceId<ExpLevelData>(ExpLevel - 1);

            this.AddFreeDiamonds(Data.DiamondReward);
        }

        /// <summary>
        /// Refreshes the arena.
        /// </summary>
        internal void RefreshArena()
        {
            CsvTable ArenaTable = CsvFiles.Get(Gamefile.Arena);

            if (ArenaTable.Datas.Count > 0)
            {
                ArenaData NextArena = null;

                for (int I = 0; I < ArenaTable.Datas.Count; I++)
                {
                    ArenaData Data = (ArenaData) ArenaTable.Datas[I];

                    if (this.Score < Data.TrophyLimit)
                    {
                        if (NextArena == null || NextArena.TrophyLimit < Data.TrophyLimit)
                        {
                            if (Data == this.Arena)
                            {
                                if (this.Score >= Data.DemoteTrophyLimit)
                                {
                                    NextArena = Data;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!Data.TrainingCamp)
                        {
                            if (NextArena != null)
                            {
                                if (NextArena.TrophyLimit < Data.TrophyLimit)
                                {
                                    NextArena = Data;
                                }
                            }
                            else
                            {
                                NextArena = Data;
                            }
                        }
                    }
                }

                if (NextArena != null)
                {
                    this.Arena = NextArena;
                }
            }
        }

        /// <summary>
        /// Called when the score of player has been changed.
        /// </summary>
        internal void ScoreChanged(int Count)
        {
            this.Score = Math.Max(this.Score + Count, 0);

            if (this.Score > this.MaxScore)
            {
                this.SetMaxScore(this.Score);
            }
        }

        /// <summary>
        /// Sets the achievement progress.
        /// </summary>
        internal void SetAchievementProgress(AchievementData Data, int Count)
        {
            this.CommoditySlots.SetCommodityCount(CommodityType.AchievementProgress, Data, Count);
        }

        /// <summary>
        /// Sets if the specified achievement reward is claimed.
        /// </summary>
        internal void SetAchievementRewardClaimed(AchievementData Data, int Count)
        {
            this.CommoditySlots.SetCommodityCount(CommodityType.AchievementCompleted, Data, Count);
        }

        /// <summary>
        /// Sets the alliance id.
        /// </summary>
        internal void SetAllianceId(int HighId, int LowId)
        {
            this.ClanHighId = HighId;
            this.ClanLowId  = LowId;
        }

        /// <summary>
        /// Sets the alliance name.
        /// </summary>
        internal void SetAllianceName(string Name)
        {
            this.AllianceName = Name;
        }

        /// <summary>
        /// Sets the name of the player.
        /// </summary>
        internal void SetName(string Name)
        {
            this.Name = Name;
        }

        /// <summary>
        /// Sets the role of the player in alliance.
        /// </summary>=
        internal void SetAllianceRole(int Role)
        {
            this.AllianceRole = Role;
        }

        /// <summary>
        /// Sets the role of the player in alliance.
        /// </summary>=
        internal void SetAllianceBadge(AllianceBadgeData Data)
        {
            this.Badge = Data;
        }

        /// <summary>
        /// Sets the number of star count.
        /// </summary>=
        internal void SetStarCount(int Count)
        {
            if (Globals.StartingArena != this.Arena)
            {
                if (Count >= 4)
                {
                    Logging.Error(this.GetType(), "SetStarCount() - Player: set star count out of bounds: " + Count);
                    return;
                }

                this.CommoditySlots.SetCommodityCount(CommodityType.Resource, CsvFiles.StarCountData, Count);

                if (Count == 3)
                {
                    this.IncreaseThreeCrownWins();
                }
            }
        }

        /// <summary>
        /// Sets the number of gold.
        /// </summary>
        internal void SetFreeGold(int Count)
        {
            this.CommoditySlots.SetCommodityCount(CommodityType.Resource, CsvFiles.GoldData, Count);
            this.CommoditySlots.SetCommodityCount(CommodityType.Resource, CsvFiles.FreeGoldData, Count);
        }

        /// <summary>
        /// Sets the number of diamonds.
        /// </summary>
        internal void SetFreeDiamonds(int Count)
        {
            this.Diamonds = Count;
            this.FreeDiamonds = Count;
        }

        /// <summary>
        /// Sets the number of chest.
        /// </summary>
        internal void SetCardsFound(int Count)
        {
            this.CommoditySlots.SetCommodityCount(CommodityType.ProfileResource, CsvFiles.CardCountData, Count);
        }

        /// <summary>
        /// Sets the number of chest.
        /// </summary>
        internal void SetChestCount(int Count)
        {
            if (Count > Globals.MaxChest)
            {
                Logging.Error(this.GetType() , "SetChestCount() - Set chest count out of bounds: "+ Count);
                return;
            }

            this.CommoditySlots.SetCommodityCount(CommodityType.Resource, CsvFiles.ChestCountData, Count);
        }

        /// <summary>
        /// Sets the favourite spell.
        /// </summary>
        internal void SetFavouriteSpell(SpellData Spell)
        {
            if (Spell != null)
            {
                CsvData CsvData = CsvFiles.Get(Gamefile.Resource).GetData("FavouriteSpell");
                this.CommoditySlots.SetCommodityCount(CommodityType.ProfileResource, CsvData, CsvData.GlobalId);
            }
        }

        /// <summary>
        /// Sets the max score.
        /// </summary>
        internal void SetMaxScore(int Count)
        {
            CsvData CsvData = CsvFiles.Get(Gamefile.Resource).GetData("MaxScore");
            this.CommoditySlots.SetCommodityCount(CommodityType.ProfileResource, CsvData, CsvData.GlobalId);
        }

        /// <summary>
        /// Sets the number of diamonds.
        /// </summary>
        internal void SetNameChangeState(int State)
        {
            this.NameChangeState = State;
        }

        /// <summary>
        /// Sets if the name is set by user.
        /// </summary>
        internal void SetNameSetByUser(bool Value)
        {
            this.IsNameSet = Value;
        }

        /// <summary>
        /// Uses the specified diamonds.
        /// </summary>
        internal void UseDiamonds(int Count)
        {
            this.Diamonds -= Count;
            this.FreeDiamonds -= Count;
        }

        /// <summary>
        /// Uses the specified diamonds.
        /// </summary>
        internal void UseGold(int Count)
        {
            this.CommoditySlots.UseCommodity(CommodityType.Resource, CsvFiles.GoldData, Count);
            this.CommoditySlots.UseCommodity(CommodityType.Resource, CsvFiles.FreeGoldData, Count);
        }

        /// <summary>
        /// Adds the number of specified exp points.
        /// </summary>
        internal void XpGainHelper(int ExpPoints)
        {
            int MaxExpLevel = CsvFiles.MaxExpLevel;

            if (CsvFiles.MaxExpLevel > this.ExpLevel)
            {
                this.ExpPoints += ExpPoints;

                ExpLevelData Data;

                for (int I = this.ExpLevel; I < MaxExpLevel; I++)
                {
                    Data = CsvFiles.Get(Gamefile.ExpLevel).GetWithInstanceId<ExpLevelData>(this.ExpLevel - 1);

                    if (this.ExpPoints >= Data.ExpToNextLevel)
                    {
                        this.ExpPoints -= Data.ExpToNextLevel;
                        ++this.ExpLevel;

                        this.LevelUp(this.ExpLevel - 1);
                    }
                }

                Data = CsvFiles.Get(Gamefile.ExpLevel).GetWithInstanceId<ExpLevelData>(this.ExpLevel - 1);

                if (this.ExpPoints > Data.ExpToNextLevel)
                {
                    this.ExpPoints = Data.ExpToNextLevel;
                }
            }
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        /// <param name="BattleEncode">if set to <c>true</c> [battle encode].</param>
        internal void Encode(ChecksumEncoder Stream, bool BattleEncode = false)
        {
            Stream.WriteLogicLong(this.HighId, this.LowId); // Avatar
            Stream.WriteLogicLong(this.HighId, this.LowId); // Account
            Stream.WriteLogicLong(this.HighId, this.LowId); // Home

            Stream.WriteString(this.Name);

            if (!BattleEncode)
            {
                Stream.WriteVInt(this.NameChangeState);
            }

            Stream.EncodeLogicData(this.Arena, 54);
            Stream.WriteVInt(this.Score);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            if (!BattleEncode)
            {
                Stream.WriteVInt(0);
            }

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.EncodeLogicData(null, 54); // 0x00
            Stream.WriteVInt(0); // 0x26

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.EncodeLogicData(null, 54);

            this.CommoditySlots.Encode(Stream);

            if (!BattleEncode)
            {
                Stream.WriteVInt(this.Diamonds);
                Stream.WriteVInt(this.FreeDiamonds);
                Stream.WriteVInt(this.ExpPoints);
                Stream.WriteVInt(this.ExpLevel);
                Stream.WriteVInt(0);

                Stream.WriteBoolean(this.IsNameSet);
                Stream.WriteBoolean(false); // ?
            }
            else
            {
                Stream.WriteVInt(this.ExpLevel);
            }

            Stream.WriteBoolean(false); // ?
            Stream.WriteBoolean(this.IsInAlliance);

            if (this.IsInAlliance)
            {
                Stream.WriteLogicLong(this.ClanHighId, this.ClanLowId);
                Stream.WriteString(this.AllianceName);
                Stream.EncodeLogicData(this.Badge, 16);

                if (!BattleEncode)
                {
                    Stream.WriteVInt(this.AllianceRole);
                }
            }

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteBool(false);

            // Stream.AddRange("B6-01  03  00  00".HexaToBytes());

            Stream.WriteVInt(0);
            Stream.WriteVInt(1);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        internal JObject Save(bool BattleSave)
        {
            JObject Json = new JObject();

            if (BattleSave)
            {
                Json.Add("accountID.hi", this.HighId);
                Json.Add("accountID.lo", this.LowId);
                Json.Add("expLevel", this.ExpLevel);
                Json.Add("name", this.Name);
                Json.Add("clan_name", this.AllianceName);
                Json.Add("scr", this.Score);
                Json.Add("clan_id_hi", this.ClanHighId);
                Json.Add("clan_id_lo", this.ClanLowId);
                Json.Add("productRed", false);

                JsonHelper.SetLogicData(Json, "arena", this.Arena);
                JsonHelper.SetLogicData(Json, "badge", this.Badge);
                JsonHelper.SetLogicData(Json, "kingSkin", CsvFiles.Get(Gamefile.Skin).GetWithInstanceId<SkinData>(0));
            }

            return Json;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.HighId + "-" + this.LowId;
        }
    }
}
