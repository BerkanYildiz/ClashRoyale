namespace ClashRoyale.Client.Files.Csv.Logic
{
    using System;

    internal class TreasureChestData : CsvData
    {
        internal ArenaData ArenaData;
        internal TreasureChestData BaseTreasureChestData;
        internal SpellData[] GuaranteedSpellsData;

        internal int TotalTimeTakenSeconds;

        /// <summary>
        /// Gets the shop price.
        /// </summary>
        internal int ShopPrice
        {
            get
            {
                int ShopPriceWithoutSpeedUp = 0;
                int SpeedUpCost = 0;
                int Cost = 0;

                if (this.BaseTreasureChestData != null)
                {
                    if (this.ArenaData != null)
                    {
                        ShopPriceWithoutSpeedUp = this.ArenaData.ChestShopPriceMultiplier * this.BaseTreasureChestData.ShopPriceWithoutSpeedUp / 100;
                    }
                    else
                    {
                        ShopPriceWithoutSpeedUp = this.BaseTreasureChestData.ShopPriceWithoutSpeedUp;
                    }

                    SpeedUpCost = this.BaseTreasureChestData.GetSpeedUpCost(this.BaseTreasureChestData.TotalTimeTakenSeconds);
                }
                else
                {
                    ShopPriceWithoutSpeedUp = this.ShopPriceWithoutSpeedUp;
                    SpeedUpCost = this.GetSpeedUpCost(this.TotalTimeTakenSeconds);
                }

                Cost = ShopPriceWithoutSpeedUp + SpeedUpCost;
                
                if (Cost > 20)
                {
                    if (Cost > 100)
                    {
                        if (Cost > 500)
                        {
                            return 100 * ((Cost + 50) / 100);
                        }

                        return 10 * ((Cost + 5) / 10);
                    }

                    return 5 * ((Cost + 3) / 5);
                }

                return Cost;
            }
        }

        /// <summary>
        /// Gets the random spell count.
        /// </summary>
        internal int RandomSpellCount
        {
            get
            {
                if (this.BaseChest != null)
                {
                    return this.ArenaData.GetScaledChestReward(this.BaseTreasureChestData.RandomSpells);
                }

                return this.ArenaData.GetScaledChestReward(this.RandomSpells);
            }
        }

        /// <summary>
        /// Gets the different spell count.
        /// </summary>
        internal int DifferentSpellCount
        {
            get
            {
                if (this.BaseChest != null)
                {
                    return this.BaseTreasureChestData.DifferentSpellCount;
                }

                return this.DifferentSpells;
            }
        }

        /// <summary>
        /// Gets the max gold.
        /// </summary>
        internal int MaxGold
        {
            get
            {
                if (this.BaseTreasureChestData == null)
                {
                    return this.RandomSpellCount * this.MaxGoldPerCard;
                }

                return this.ArenaData.GetScaledChestReward(this.BaseTreasureChestData.MaxGold);
            }
        }

        /// <summary>
        /// Gets the min gold.
        /// </summary>
        internal int MinGold
        {
            get
            {
                if (this.BaseTreasureChestData == null)
                {
                    return this.RandomSpellCount * this.MinGoldPerCard;
                }

                return this.ArenaData.GetScaledChestReward(this.BaseTreasureChestData.MinGold);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreasureChestData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public TreasureChestData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // TreasureChestData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
		{
		    if (!string.IsNullOrEmpty(this.Arena))
		    {
		        this.ArenaData = CsvFiles.Get(Gamefile.Arena).GetData<ArenaData>(this.Arena);

		        if (this.ArenaData == null)
		        {
		            throw new Exception("Arena " + this.Arena + " does not exist.");
		        }
		    }

		    if (!string.IsNullOrEmpty(this.BaseChest))
		    {
		        this.BaseTreasureChestData = this.CsvTable.GetData<TreasureChestData>(this.BaseChest);
		    }

		    if (!string.IsNullOrEmpty(this.GuaranteedSpells[0]))
		    {
		        this.GuaranteedSpellsData = new SpellData[this.GuaranteedSpells.Length];

		        for (int I = 0; I < this.GuaranteedSpells.Length; I++)
		        {
		            this.GuaranteedSpellsData[I] = CsvFiles.GetSpellDataByName(this.GuaranteedSpells[I]);
		        }
		    }
            else 
                this.GuaranteedSpellsData = new SpellData[0];

		    this.TotalTimeTakenSeconds = this.TimeTakenDays * 86400 + this.TimeTakenHours * 3600 + this.TimeTakenMinutes * 60 + this.TimeTakenSeconds;

		    if (this.BaseTreasureChestData != null)
		    {
		        if (!this.InShop)
		        {
		            this.InShop = this.BaseTreasureChestData.InShop;
		        }

		        if (this.TotalTimeTakenSeconds == 0)
		        {
		            this.TotalTimeTakenSeconds = this.BaseTreasureChestData.TotalTimeTakenSeconds;
		        }
		    }
		}

        /// <summary>
        /// Gets the chance for specified rarity.
        /// </summary>
        internal int GetChanceForRarity(RarityData Data)
        {
            int Chance;

            if (this.BaseTreasureChestData != null)
            {
                if (this.ArenaData.GetUnlockedSpellCountForRarity(Data) < 1)
                {
                    return 0;
                }

                Chance = this.BaseTreasureChestData.GetChanceForRarity(Data);
            }
            else
            {
                switch (Data.Name)
                {
                    case "Legendary":
                    {
                        Chance = this.LegendaryChance;
                        break;
                    }
                    case "Epic":
                    {
                        Chance = this.EpicChance;
                        break;
                    }
                    case "Rare":
                    {
                        Chance = this.RareChance;
                        break;
                    }
                    default:
                    {
                        Chance = 1;
                        break;
                    }
                }
            }

            return Chance;
        }

        /// <summary>
        /// Gets the speed up cost.
        /// </summary>
        internal int GetSpeedUpCost(int RemainingTime)
        {
            if (RemainingTime <= 0 )
            {
                return 0;
            }

            return Math.Clamp((this.TotalTimeTakenSeconds + 600 * RemainingTime - 1) / this.TotalTimeTakenSeconds, 1, this.TotalTimeTakenSeconds / 600);
        }

        internal string BaseChest
        {
            get; set;
        }

        internal string Arena
        {
            get; set;
        }

        internal bool InShop
        {
            get; set;
        }

        internal bool InArenaInfo
        {
            get; set;
        }

        internal bool TournamentChest
        {
            get; set;
        }

        internal bool SurvivalChest
        {
            get; set;
        }

        internal int ShopPriceWithoutSpeedUp
        {
            get; set;
        }

        internal int TimeTakenDays
        {
            get; set;
        }

        internal int TimeTakenHours
        {
            get; set;
        }

        internal int TimeTakenMinutes
        {
            get; set;
        }

        internal int TimeTakenSeconds
        {
            get; set;
        }

        internal int RandomSpells
        {
            get; set;
        }

        internal int DifferentSpells
        {
            get; set;
        }

        internal int ChestCountInChestCycle
        {
            get; set;
        }

        internal int RareChance
        {
            get; set;
        }

        internal int EpicChance
        {
            get; set;
        }

        internal int LegendaryChance
        {
            get; set;
        }

        internal int SkinChance
        {
            get; set;
        }

        internal string[] GuaranteedSpells
        {
            get; set;
        }

        internal int MinGoldPerCard
        {
            get; set;
        }

        internal int MaxGoldPerCard
        {
            get; set;
        }

        internal string FileName
        {
            get; set;
        }

        internal string ExportName
        {
            get; set;
        }

        internal string ShopExportName
        {
            get; set;
        }

        internal string GainedExportName
        {
            get; set;
        }

        internal string AnimExportName
        {
            get; set;
        }

        internal string OpenInstanceName
        {
            get; set;
        }

        internal string SlotLandEffect
        {
            get; set;
        }

        internal string OpenEffect
        {
            get; set;
        }

        internal string TapSound
        {
            get; set;
        }

        internal string TapSoundShop
        {
            get; set;
        }

        internal string DescriptionTid
        {
            get; set;
        }

        internal string Tid
        {
            get; set;
        }

        internal string NotificationTid
        {
            get; set;
        }

        internal string SpellSet
        {
            get; set;
        }

        internal int Exp
        {
            get; set;
        }

        internal int SortValue
        {
            get; set;
        }

        internal bool SpecialOffer
        {
            get; set;
        }

        internal bool DraftChest
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

        internal bool BoostedChest
        {
            get; set;
        }

    }
}