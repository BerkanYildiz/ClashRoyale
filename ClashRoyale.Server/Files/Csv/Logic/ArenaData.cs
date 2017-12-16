namespace ClashRoyale.Server.Files.Csv.Logic
{
    using System;
    using System.Collections.Generic;

    using ClashRoyale.Server.Logic.Enums;

    internal class ArenaData : CsvData
    {
        internal LocationData PvPLocationData;
        internal LocationData TeamVsTeamLocationData;
        internal List<SpellData>[] UnlockedSpellsData;

        internal TreasureChestData FreeChestData;
        internal TreasureChestData CrownChestData;
        internal TreasureChestData MagicChestData;
        internal TreasureChestData SuperMagicalChestData;

        internal ArenaData ChestArenaData;

        /// <summary>
        /// Gets the previous arena data.
        /// </summary>
        internal ArenaData PreviousArena
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
		internal void ConfigureSpells()
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
        internal int GetUnlockedSpellCountForRarity(RarityData Rarity)
        {
            return this.UnlockedSpellsData[Rarity.Instance].Count;
        }

        /// <summary>
        /// Returns count scaled by chest reward multiplier.
        /// </summary>
        internal int GetScaledChestReward(int Count)
        {
            if (this.PreviousArena == null || this.PreviousArena.ChestRewardMultiplier >= this.ChestRewardMultiplier)
            {
                return (this.ChestRewardMultiplier * Count + 50) / 100;
            }

            long V6 = (1374389535L * (this.ChestRewardMultiplier * Count + 50)) >> 32;
            return Math.Max(this.PreviousArena.GetScaledChestReward(Count) + 1, (int) (((int) V6 >> 5) + (V6 >> 31)));
        }

        internal string Tid
        {
            get; set;
        }

        internal string SubtitleTid
        {
            get; set;
        }

        internal int Arena
        {
            get; set;
        }

        internal string ChestArena
        {
            get; set;
        }

        internal string TvArena
        {
            get; set;
        }

        internal bool IsInUse
        {
            get; set;
        }

        internal bool TrainingCamp
        {
            get; set;
        }

        internal bool PveArena
        {
            get; set;
        }

        internal int TrophyLimit
        {
            get; set;
        }

        internal int DemoteTrophyLimit
        {
            get; set;
        }

        internal int SeasonTrophyReset
        {
            get; set;
        }

        internal int ChestRewardMultiplier
        {
            get; set;
        }

        internal int BoostedCrownChestRewardMultiplier
        {
            get; set;
        }

        internal int ChestShopPriceMultiplier
        {
            get; set;
        }

        internal int RequestSize
        {
            get; set;
        }

        internal int MaxDonationCountCommon
        {
            get; set;
        }

        internal int MaxDonationCountRare
        {
            get; set;
        }

        internal int MaxDonationCountEpic
        {
            get; set;
        }

        internal string IconSwf
        {
            get; set;
        }

        internal string IconExportName
        {
            get; set;
        }

        internal string MainMenuIconExportName
        {
            get; set;
        }

        internal string SmallIconExportName
        {
            get; set;
        }

        internal int MatchmakingMinTrophyDelta
        {
            get; set;
        }

        internal int MatchmakingMaxTrophyDelta
        {
            get; set;
        }

        internal int MatchmakingMaxSeconds
        {
            get; set;
        }

        internal string PvpLocation
        {
            get; set;
        }

        internal string TeamVsTeamLocation
        {
            get; set;
        }

        internal int DailyDonationCapacityLimit
        {
            get; set;
        }

        internal int BattleRewardGold
        {
            get; set;
        }

        internal string ReleaseDate
        {
            get; set;
        }

        internal string SeasonRewardChest
        {
            get; set;
        }

        internal string QuestCycle
        {
            get; set;
        }

        internal string ForceQuestChestCycle
        {
            get; set;
        }

    }
}