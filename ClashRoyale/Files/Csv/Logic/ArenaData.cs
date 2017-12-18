namespace ClashRoyale.Files.Csv.Logic
{
    using System;
    using System.Collections.Generic;

    using ClashRoyale.Enums;

    public class ArenaData : CsvData
    {
        public LocationData PvPLocationData;
        public LocationData TeamVsTeamLocationData;
        public List<SpellData>[] UnlockedSpellsData;

        public TreasureChestData FreeChestData;
        public TreasureChestData CrownChestData;
        public TreasureChestData MagicChestData;
        public TreasureChestData SuperMagicalChestData;

        public ArenaData ChestArenaData;

        /// <summary>
        /// Gets the previous arena data.
        /// </summary>
        public ArenaData PreviousArena
        {
            get
            {
                ArenaData Previous = null;

                CsvFiles.Get(this.Instance).Datas.ForEach(Data =>
                {
                    ArenaData ArenaData = (ArenaData) Data;

                    if (ArenaData.TrainingCamp && !this.TrainingCamp || ArenaData.TrophyLimit < this.DemoteTrophyLimit)
                    {
                        if (Previous != null)
                        {
                            if (ArenaData.TrophyLimit > Previous.TrophyLimit)
                            {
                                Previous = ArenaData;
                            }
                        }
                        else
                            Previous = ArenaData;
                    }
                });

                return Previous;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArenaData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public ArenaData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // ArenaData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		public void ConfigureSpells()
        {
            this.PvPLocationData = CsvFiles.Get(Gamefile.Location).GetData<LocationData>(this.PvpLocation);
            this.TeamVsTeamLocationData = CsvFiles.Get(Gamefile.Location).GetData<LocationData>(this.TeamVsTeamLocation);

            this.UnlockedSpellsData = new List<SpellData>[CsvFiles.Get(Gamefile.Rarity).Datas.Count];

		    for (int I = 0; I < this.UnlockedSpellsData.Length; I++)
		    {
		        this.UnlockedSpellsData[I] = new List<SpellData>();

		        foreach (SpellData Data in CsvFiles.Get(Gamefile.SpellCharacter).Datas)
		        {
		            if (Data.IsUnlockedInArena(this))
		            {
		                if (Data.RarityData == CsvFiles.Get(Gamefile.Rarity).Datas[I])
		                {
		                    this.UnlockedSpellsData[I].Add(Data);
                        }
		            }
		        }

		        foreach (SpellData Data in CsvFiles.Get(Gamefile.SpellBuilding).Datas)
		        {
		            if (Data.IsUnlockedInArena(this))
		            {
		                if (Data.RarityData == CsvFiles.Get(Gamefile.Rarity).Datas[I])
		                {
		                    this.UnlockedSpellsData[I].Add(Data);
		                }
		            }
		        }

		        foreach (SpellData Data in CsvFiles.Get(Gamefile.SpellOther).Datas)
		        {
		            if (Data.IsUnlockedInArena(this))
		            {
		                if (Data.RarityData == CsvFiles.Get(Gamefile.Rarity).Datas[I])
		                {
		                    this.UnlockedSpellsData[I].Add(Data);
		                }
		            }
		        }

		        foreach (SpellData Data in CsvFiles.Get(Gamefile.SpellOther).Datas)
		        {
		            if (Data.IsUnlockedInArena(this))
		            {
		                if (Data.RarityData == CsvFiles.Get(Gamefile.Rarity).Datas[I])
		                {
		                    this.UnlockedSpellsData[I].Add(Data);
		                }
		            }
		        }
            }
            
            if (!this.TrainingCamp)
            {
                foreach (TreasureChestData Data in CsvFiles.Get(Gamefile.TreasureChest).Datas)
                {
                    if (Data.Arena == this.Name)
                    {
                        if (Data.BaseChest == "Free")
                        {
                            this.FreeChestData = Data;
                        }

                        if (Data.BaseChest == "Start")
                        {
                            this.CrownChestData = Data;
                        }

                        if (Data.BaseChest == "Magic")
                        {
                            this.MagicChestData = Data;
                        }

                        if (Data.BaseChest == "Super")
                        {
                            this.SuperMagicalChestData = Data;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.ChestArena))
            {
                this.ChestArenaData = CsvFiles.Get(Gamefile.Arena).GetData<ArenaData>(this.ChestArena);
            }
            else
            {
                this.ChestArenaData = this;
            }
        }

        /// <summary>
        /// Gets the unlocked spell count for specified rarity.
        /// </summary>
        public int GetUnlockedSpellCountForRarity(RarityData Rarity)
        {
            return this.UnlockedSpellsData[Rarity.Instance].Count;
        }

        /// <summary>
        /// Returns count scaled by chest reward multiplier.
        /// </summary>
        public int GetScaledChestReward(int Count)
        {
            if (this.PreviousArena == null || this.PreviousArena.ChestRewardMultiplier >= this.ChestRewardMultiplier)
            {
                return (this.ChestRewardMultiplier * Count + 50) / 100;
            }

            long V6 = (1374389535L * (this.ChestRewardMultiplier * Count + 50)) >> 32;
            return Math.Max(this.PreviousArena.GetScaledChestReward(Count) + 1, (int) (((int) V6 >> 5) + (V6 >> 31)));
        }

        public string Tid
        {
            get; set;
        }

        public string SubtitleTid
        {
            get; set;
        }

        public int Arena
        {
            get; set;
        }

        public string ChestArena
        {
            get; set;
        }

        public string TvArena
        {
            get; set;
        }

        public bool IsInUse
        {
            get; set;
        }

        public bool TrainingCamp
        {
            get; set;
        }

        public bool PveArena
        {
            get; set;
        }

        public int TrophyLimit
        {
            get; set;
        }

        public int DemoteTrophyLimit
        {
            get; set;
        }

        public int SeasonTrophyReset
        {
            get; set;
        }

        public int ChestRewardMultiplier
        {
            get; set;
        }

        public int BoostedCrownChestRewardMultiplier
        {
            get; set;
        }

        public int ChestShopPriceMultiplier
        {
            get; set;
        }

        public int RequestSize
        {
            get; set;
        }

        public int MaxDonationCountCommon
        {
            get; set;
        }

        public int MaxDonationCountRare
        {
            get; set;
        }

        public int MaxDonationCountEpic
        {
            get; set;
        }

        public string IconSwf
        {
            get; set;
        }

        public string IconExportName
        {
            get; set;
        }

        public string MainMenuIconExportName
        {
            get; set;
        }

        public string SmallIconExportName
        {
            get; set;
        }

        public int MatchmakingMinTrophyDelta
        {
            get; set;
        }

        public int MatchmakingMaxTrophyDelta
        {
            get; set;
        }

        public int MatchmakingMaxSeconds
        {
            get; set;
        }

        public string PvpLocation
        {
            get; set;
        }

        public string TeamVsTeamLocation
        {
            get; set;
        }

        public int DailyDonationCapacityLimit
        {
            get; set;
        }

        public int BattleRewardGold
        {
            get; set;
        }

        public string ReleaseDate
        {
            get; set;
        }

        public string SeasonRewardChest
        {
            get; set;
        }

        public string QuestCycle
        {
            get; set;
        }

        public string ForceQuestChestCycle
        {
            get; set;
        }

    }
}