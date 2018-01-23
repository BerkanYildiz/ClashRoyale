namespace ClashRoyale.Files.Csv
{
    using System.Collections.Generic;
    using ClashRoyale.Files.Csv.Client;
    using ClashRoyale.Files.Csv.Logic;

    public class CsvTable
    {
        public readonly List<CsvData> Datas;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CsvTable" /> class.
        /// </summary>
        /// <param name="Offset">The offset.</param>
        /// <param name="Path">The path.</param>
        public CsvTable(int Offset, string Path)
        {
            this.Offset = Offset;

            this.Datas = new List<CsvData>();
            CsvReader Reader = new CsvReader(Path);

            for (int i = 0; i < Reader.GetRowCount(); i++)
            {
                CsvRow Row = Reader.GetRowAt(i);
                CsvData Data = this.Load(Row);

                if (Row == null)
                {
                    Logging.Error(this.GetType(), "CsvRow == null.");
                }

                if (Data != null)
                {
                    this.Datas.Add(Data);
                }
                else
                {
                    Logging.Info(this.GetType(), "Data == null at CsvTable(" + Offset + ", " + Path + ").");
                }
            }
        }

        /// <summary>
        ///     Gets the offset of this <see cref="CsvTable" />.
        /// </summary>
        public int Offset { get; }

        /// <summary>
        ///     Loads the specified CSV row.
        /// </summary>
        /// <param name="CsvRow">The CSV row.</param>
        public CsvData Load(CsvRow CsvRow)
        {
            switch (this.Offset)
            {
                case 1:
                {
                    return new LocaleData(CsvRow, this);
                }

                case 2:
                {
                    return new BillingPackageData(CsvRow, this);
                }

                case 3:
                case 20:
                {
                    return new GlobalData(CsvRow, this);
                }

                case 4:
                {
                    return new SoundData(CsvRow, this);
                }

                case 5:
                {
                    return new ResourceData(CsvRow, this);
                }

                case 9:
                {
                    return new CharacterBuffData(CsvRow, this);
                }

                case 10:
                {
                    return new ProjectileData(CsvRow, this);
                }

                case 11:
                {
                    return new EffectData(CsvRow, this);
                }

                case 12:
                {
                    return new PredefinedDeckData(CsvRow, this);
                }

                case 14:
                {
                    return new RarityData(CsvRow, this);
                }

                case 15:
                {
                    return new LocationData(CsvRow, this);
                }

                case 16:
                {
                    return new AllianceBadgeData(CsvRow, this);
                }

                case 18:
                {
                    return new NpcData(CsvRow, this);
                }

                case 19:
                {
                    return new TreasureChestData(CsvRow, this);
                }

                case 21:
                {
                    return new ParticleEmitterData(CsvRow, this);
                }

                case 22:
                {
                    return new AreaEffectObjectData(CsvRow, this);
                }

                case 26:
                case 27:
                case 28:
                case 29:
                {
                    return new SpellData(CsvRow, this);
                }

                case 34:
                case 35:
                {
                    return new CharacterData(CsvRow, this);
                }

                case 40:
                {
                    return new HealthBarData(CsvRow, this);
                }

                case 41:
                {
                    return new MusicData(CsvRow, this);
                }

                case 42:
                {
                    return new DecoData(CsvRow, this);
                }

                case 43:
                {
                    return new GambleChestData(CsvRow, this);
                }

                case 45:
                case 48:
                {
                    return new TutorialData(CsvRow, this);
                }

                case 46:
                {
                    return new ExpLevelData(CsvRow, this);
                }

                case 50:
                {
                    return new BackgroundDecoData(CsvRow, this);
                }

                case 51:
                {
                    return new SpellSetData(CsvRow, this);
                }

                case 52:
                {
                    return new ChestOrderData(CsvRow, this);
                }

                case 53:
                {
                    return new TauntData(CsvRow, this);
                }

                case 54:
                {
                    return new ArenaData(CsvRow, this);
                }

                case 55:
                {
                    return new ResourcePackData(CsvRow, this);
                }

                case 56:
                {
                    return new CsvData(CsvRow, this); // TODO
                }

                case 57:
                {
                    return new RegionData(CsvRow, this);
                }

                case 58:
                {
                    return new NewsData(CsvRow, this);
                }

                case 59:
                {
                    return new AllianceRoleData(CsvRow, this);
                }

                case 60:
                {
                    return new AchievementData(CsvRow, this);
                }

                case 61:
                {
                    return new HintData(CsvRow, this);
                }

                case 62:
                {
                    return new HelpshiftData(CsvRow, this);
                }

                case 63:
                {
                    return new TournamentTierData(CsvRow, this);
                }

                case 64:
                {
                    return new ContentTestData(CsvRow, this);
                }

                case 65:
                {
                    return new SurvivalModeData(CsvRow, this);
                }

                case 66:
                {
                    return new ShopData(CsvRow, this);
                }

                case 67:
                {
                    return new EventCategoryData(CsvRow, this);
                }

                case 68:
                {
                    return new DraftDeckData(CsvRow, this);
                }
                    
                case 72:
                {
                    return new GameModeData(CsvRow, this);
                }

                case 74:
                {
                    return new EventCategoryDefinitionData(CsvRow, this);
                }

                case 75:
                {
                    return new EventCategoryObjectDefinitionData(CsvRow, this);
                }

                case 76:
                {
                    return new EventCategoryEnumData(CsvRow, this);
                }

                case 77:
                {
                    return new ConfigurationDefinitionData(CsvRow, this);
                }

                case 79:
                {
                    return new PveGamemodeData(CsvRow, this);
                }

                case 81:
                {
                    return new TveGamemodeData(CsvRow, this);
                }

                case 82:
                {
                    return new TutorialChestOrderData(CsvRow, this);
                }

                case 83:
                {
                    return new SkinData(CsvRow, this);
                }

                case 84:
                {
                    return new QuestOrderData(CsvRow, this);
                }

                case 85:
                {
                    return new EventTargetingDefinitionData(CsvRow, this);
                }

                case 86:
                {
                    return new ShopCycleData(CsvRow, this);
                }

                case 87:
                {
                    return new SkinSetData(CsvRow, this);
                }
            }

            return new CsvData(CsvRow, this);
        }

        /// <summary>
        ///     Gets the data with identifier.
        /// </summary>
        /// <param name="Identifier">The identifier.</param>
        public CsvData GetWithInstanceId(int Identifier)
        {
            if (this.Datas.Count > Identifier)
            {
                return this.Datas[Identifier];
            }

            return null;
        }

        /// <summary>
        ///     Gets the data with identifier.
        /// </summary>
        /// <param name="Identifier">The identifier.</param>
        public T GetWithInstanceId<T>(int Identifier) where T : CsvData
        {
            if (this.Datas.Count > Identifier)
            {
                return this.Datas[Identifier] as T;
            }

            return null;
        }

        /// <summary>
        ///     Gets the data with identifier.
        /// </summary>
        /// <param name="GlobalId">The identifier.</param>
        public CsvData GetWithGlobalId(int GlobalId)
        {
            return this.GetWithInstanceId(GlobalId % 1000000);
        }

        /// <summary>
        ///     Gets the data with identifier.
        /// </summary>
        /// <param name="GlobalId">The identifier.</param>
        public T GetWithGlobalId<T>(int GlobalId) where T : CsvData
        {
            return this.GetWithInstanceId(GlobalId % 1000000) as T;
        }

        /// <summary>
        ///     Gets the data.
        /// </summary>
        /// <param name="Name">The name.</param>
        public CsvData GetData(string Name)
        {
            return this.Datas.Find(Data => Data.Name == Name);
        }

        /// <summary>
        ///     Gets the data.
        /// </summary>
        /// <param name="Name">The name.</param>
        public T GetData<T>(string Name) where T : CsvData
        {
            return this.Datas.Find(Data => Data.Name == Name) as T;
        }
    }
}