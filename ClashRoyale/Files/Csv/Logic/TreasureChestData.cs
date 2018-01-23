namespace ClashRoyale.Files.Csv.Logic
{
    using System;
    using ClashRoyale.Enums;
    using Math = ClashRoyale.Maths.Math;

    public class TreasureChestData : CsvData
    {
        public ArenaData ArenaData;
        public TreasureChestData BaseTreasureChestData;
        public SpellData[] GuaranteedSpellsData;

        public int TotalTimeTakenSeconds;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TreasureChestData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public TreasureChestData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // TreasureChestData.
        }

        /// <summary>
        ///     Gets the shop price.
        /// </summary>
        public int ShopPrice
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
        ///     Gets the random spell count.
        /// </summary>
        public int RandomSpellCount
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
        ///     Gets the different spell count.
        /// </summary>
        public int DifferentSpellCount
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
        ///     Gets the max gold.
        /// </summary>
        public int MaxGold
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
        ///     Gets the min gold.
        /// </summary>
        public int MinGold
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

        public string BaseChest { get; set; }

        public string Arena { get; set; }

        public bool InShop { get; set; }

        public bool InArenaInfo { get; set; }

        public bool TournamentChest { get; set; }

        public bool SurvivalChest { get; set; }

        public int ShopPriceWithoutSpeedUp { get; set; }

        public int TimeTakenDays { get; set; }

        public int TimeTakenHours { get; set; }

        public int TimeTakenMinutes { get; set; }

        public int TimeTakenSeconds { get; set; }

        public int RandomSpells { get; set; }

        public int DifferentSpells { get; set; }

        public int ChestCountInChestCycle { get; set; }

        public int RareChance { get; set; }

        public int EpicChance { get; set; }

        public int LegendaryChance { get; set; }

        public int SkinChance { get; set; }

        public string[] GuaranteedSpells { get; set; }

        public int MinGoldPerCard { get; set; }

        public int MaxGoldPerCard { get; set; }

        public string FileName { get; set; }

        public string ExportName { get; set; }

        public string ShopExportName { get; set; }

        public string GainedExportName { get; set; }

        public string AnimExportName { get; set; }

        public string OpenInstanceName { get; set; }

        public string SlotLandEffect { get; set; }

        public string OpenEffect { get; set; }

        public string TapSound { get; set; }

        public string TapSoundShop { get; set; }

        public string DescriptionTid { get; set; }

        public string Tid { get; set; }

        public string NotificationTid { get; set; }

        public string SpellSet { get; set; }

        public int Exp { get; set; }

        public int SortValue { get; set; }

        public bool SpecialOffer { get; set; }

        public bool DraftChest { get; set; }

        public string IconSwf { get; set; }

        public string IconExportName { get; set; }

        public bool BoostedChest { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            if (!string.IsNullOrEmpty(this.Arena))
            {
                this.ArenaData = CsvFiles.Get(Gamefile.Arenas).GetData<ArenaData>(this.Arena);

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
            {
                this.GuaranteedSpellsData = new SpellData[0];
            }

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
        ///     Gets the chance for specified rarity.
        /// </summary>
        public int GetChanceForRarity(RarityData Data)
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
        ///     Gets the speed up cost.
        /// </summary>
        public int GetSpeedUpCost(int RemainingTime)
        {
            if (RemainingTime <= 0)
            {
                return 0;
            }

            return Math.Clamp((this.TotalTimeTakenSeconds + 600 * RemainingTime - 1) / this.TotalTimeTakenSeconds, 1, this.TotalTimeTakenSeconds / 600);
        }
    }
}