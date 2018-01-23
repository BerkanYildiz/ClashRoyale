namespace ClashRoyale.Files.Csv
{
    using System.Collections.Generic;
    using System.IO;
    using ClashRoyale.Enums;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Files.Csv.Tilemaps;

    public static class CsvFiles
    {
        public static Dictionary<int, CsvTable> Files;
        public static Dictionary<int, string> Paths;
        public static Dictionary<string, TilemapData> Tilemaps;

        public static ResourceData GoldData;
        public static ResourceData FreeGoldData;
        public static ResourceData CardCountData;
        public static ResourceData StarCountData;
        public static ResourceData ChestCountData;

        public static CharacterData SummonerData;
        public static RarityData RarityCommonData;
        public static GameModeData GameModeLadderData;

        public static List<SpellData> Spells;
        public static List<CharacterData> Characters;

        public static int MaxExpLevel;

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="CsvFiles" /> has been already initialized.
        /// </summary>
        public static bool Initialized { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CsvFiles" /> class.
        /// </summary>
        public static void Initialize()
        {
            if (CsvFiles.Initialized)
            {
                return;
            }

            CsvFiles.Files = new Dictionary<int, CsvTable>();
            CsvFiles.Paths = new Dictionary<int, string>();
            CsvFiles.Tilemaps = new Dictionary<string, TilemapData>(32);

            CsvFiles.Spells = new List<SpellData>(72);
            CsvFiles.Characters = new List<CharacterData>(72);

            CsvFiles.Paths.Add(1, @"Gamefiles/csv_client/locales.csv");
            CsvFiles.Paths.Add(2, @"Gamefiles/csv_client/billing_packages.csv");
            CsvFiles.Paths.Add(3, @"Gamefiles/csv_logic/globals.csv");
            CsvFiles.Paths.Add(4, @"Gamefiles/csv_client/sounds.csv");
            CsvFiles.Paths.Add(5, @"Gamefiles/csv_logic/resources.csv");

            CsvFiles.Paths.Add(9, @"Gamefiles/csv_logic/character_buffs.csv");

            CsvFiles.Paths.Add(10, @"Gamefiles/csv_logic/projectiles.csv");
            CsvFiles.Paths.Add(11, @"Gamefiles/csv_client/effects.csv");
            CsvFiles.Paths.Add(12, @"Gamefiles/csv_logic/predefined_decks.csv");
            CsvFiles.Paths.Add(14, @"Gamefiles/csv_logic/rarities.csv");
            CsvFiles.Paths.Add(15, @"Gamefiles/csv_logic/locations.csv");
            CsvFiles.Paths.Add(16, @"Gamefiles/csv_logic/alliance_badges.csv");

            CsvFiles.Paths.Add(18, @"Gamefiles/csv_logic/npcs.csv");
            CsvFiles.Paths.Add(19, @"Gamefiles/csv_logic/treasure_chests.csv");

            CsvFiles.Paths.Add(20, @"Gamefiles/csv_client/client_globals.csv");
            CsvFiles.Paths.Add(21, @"Gamefiles/csv_client/particle_emitters.csv");
            CsvFiles.Paths.Add(22, @"Gamefiles/csv_logic/area_effect_objects.csv");

            CsvFiles.Paths.Add(26, @"Gamefiles/csv_logic/spells_characters.csv");
            CsvFiles.Paths.Add(27, @"Gamefiles/csv_logic/spells_buildings.csv");
            CsvFiles.Paths.Add(28, @"Gamefiles/csv_logic/spells_other.csv");
            CsvFiles.Paths.Add(29, @"Gamefiles/csv_logic/spells_heroes.csv");

            CsvFiles.Paths.Add(34, @"Gamefiles/csv_logic/characters.csv");
            CsvFiles.Paths.Add(35, @"Gamefiles/csv_logic/buildings.csv");

            CsvFiles.Paths.Add(40, @"Gamefiles/csv_client/health_bars.csv");
            CsvFiles.Paths.Add(41, @"Gamefiles/csv_client/music.csv");
            CsvFiles.Paths.Add(42, @"Gamefiles/csv_logic/decos.csv");
            CsvFiles.Paths.Add(43, @"Gamefiles/csv_logic/gamble_chests.csv");

            CsvFiles.Paths.Add(45, @"Gamefiles/csv_logic/tutorials_home.csv");
            CsvFiles.Paths.Add(46, @"Gamefiles/csv_logic/exp_levels.csv");

            CsvFiles.Paths.Add(48, @"Gamefiles/csv_logic/tutorials_npc.csv");

            CsvFiles.Paths.Add(50, @"Gamefiles/csv_client/background_decos.csv");

            CsvFiles.Paths.Add(52, @"Gamefiles/csv_logic/chest_order.csv");
            CsvFiles.Paths.Add(53, @"Gamefiles/csv_logic/taunts.csv");
            CsvFiles.Paths.Add(54, @"Gamefiles/csv_logic/arenas.csv");
            CsvFiles.Paths.Add(55, @"Gamefiles/csv_logic/resource_packs.csv");
            CsvFiles.Paths.Add(56, @"Gamefiles/csv_client/credits.csv");
            CsvFiles.Paths.Add(57, @"Gamefiles/csv_logic/regions.csv");
            CsvFiles.Paths.Add(58, @"Gamefiles/csv_client/news.csv");
            CsvFiles.Paths.Add(59, @"Gamefiles/csv_logic/alliance_roles.csv");

            CsvFiles.Paths.Add(60, @"Gamefiles/csv_logic/achievements.csv");
            CsvFiles.Paths.Add(61, @"Gamefiles/csv_client/hints.csv");
            CsvFiles.Paths.Add(62, @"Gamefiles/csv_client/helpshift.csv");
            CsvFiles.Paths.Add(63, @"Gamefiles/csv_logic/tournament_tiers.csv");
            CsvFiles.Paths.Add(64, @"Gamefiles/csv_logic/content_tests.csv");
            CsvFiles.Paths.Add(65, @"Gamefiles/csv_logic/survival_modes.csv");
            CsvFiles.Paths.Add(66, @"Gamefiles/csv_logic/shop.csv");
            CsvFiles.Paths.Add(67, @"Gamefiles/csv_logic/event_categories.csv");
            CsvFiles.Paths.Add(68, @"Gamefiles/csv_logic/draft_deck.csv");

            // Paths.Add(70, @"Gamefiles/csv_logic/abilities.csv");

            CsvFiles.Paths.Add(72, @"Gamefiles/csv_logic/game_modes.csv");

            CsvFiles.Paths.Add(74, @"Gamefiles/csv_logic/event_category_definitions.csv");
            CsvFiles.Paths.Add(75, @"Gamefiles/csv_logic/event_category_object_definitions.csv");
            CsvFiles.Paths.Add(76, @"Gamefiles/csv_logic/event_category_enums.csv");
            CsvFiles.Paths.Add(77, @"Gamefiles/csv_logic/configuration_definitions.csv");

            CsvFiles.Paths.Add(79, @"Gamefiles/csv_logic/pve_gamemodes.csv");

            CsvFiles.Paths.Add(81, @"Gamefiles/csv_logic/tve_gamemodes.csv");
            CsvFiles.Paths.Add(82, @"Gamefiles/csv_logic/tutorial_chest_order.csv");
            CsvFiles.Paths.Add(83, @"Gamefiles/csv_logic/skins.csv");
            CsvFiles.Paths.Add(84, @"Gamefiles/csv_logic/quest_order.csv");
            CsvFiles.Paths.Add(85, @"Gamefiles/csv_logic/event_targeting_definitions.csv");
            CsvFiles.Paths.Add(86, @"Gamefiles/csv_logic/shop_cycle.csv");
            CsvFiles.Paths.Add(87, @"Gamefiles/csv_logic/skin_sets.csv");

            foreach (KeyValuePair<int, string> Pair in CsvFiles.Paths)
            {
                if (!File.Exists(Pair.Value))
                {
                    Logging.Error(typeof(CsvFiles), "File.Exists(Path) != true at Initialize().");
                }
                else
                {
                    CsvFiles.Files.Add(Pair.Key, new CsvTable(Pair.Key, Pair.Value));
                }
            }

            foreach (CsvTable CsvTable in CsvFiles.Files.Values)
            {
                foreach (CsvData CsvData in CsvTable.Datas)
                {
                    CsvData.LoadingFinished();
                }
            }

            CsvFiles.MaxExpLevel = CsvFiles.Get(Gamefile.ExpLevels).Datas.Count;

            CsvFiles.GoldData = CsvFiles.Get(Gamefile.Resources).GetData<ResourceData>("Gold");
            CsvFiles.FreeGoldData = CsvFiles.Get(Gamefile.Resources).GetData<ResourceData>("FreeGold");
            CsvFiles.StarCountData = CsvFiles.Get(Gamefile.Resources).GetData<ResourceData>("StarCount");
            CsvFiles.CardCountData = CsvFiles.Get(Gamefile.Resources).GetData<ResourceData>("CardCount");
            CsvFiles.ChestCountData = CsvFiles.Get(Gamefile.Resources).GetData<ResourceData>("ChestCount");
            CsvFiles.SummonerData = CsvFiles.Get(Gamefile.Buildings).GetData<CharacterData>("KingTower");
            CsvFiles.GameModeLadderData = CsvFiles.Get(Gamefile.GameModes).GetData<GameModeData>("Ladder");

            foreach (ArenaData ArenaData in CsvFiles.Get(Gamefile.Arenas).Datas)
            {
                ArenaData.ConfigureSpells();
            }

            foreach (string FilePath in Directory.GetFiles(@"Gamefiles/tilemaps/"))
            {
                CsvFiles.Tilemaps.Add(FilePath, new TilemapData(FilePath));
            }

            Logging.Info(typeof(CsvFiles), "Loaded " + CsvFiles.Files.Count + " CSV files.");
        }

        /// <summary>
        ///     Gets the spell data by name.
        /// </summary>
        public static SpellData GetSpellDataByName(string Name)
        {
            return CsvFiles.Spells.Find(T =>
                T.Name == Name
            );
        }

        /// <summary>
        ///     Gets the <see cref="DataTable" /> at the specified index.
        /// </summary>
        /// <param name="Index">The index.</param>
        public static CsvTable Get(Gamefile Index)
        {
            return CsvFiles.Get((int) Index);
        }

        /// <summary>
        ///     Gets the <see cref="DataTable" /> at the specified index.
        /// </summary>
        /// <param name="Index">The index.</param>
        public static CsvTable Get(int Index)
        {
            if (CsvFiles.Files.ContainsKey(Index))
            {
                return CsvFiles.Files[Index];
            }

            return null;
        }

        /// <summary>
        ///     Gets the with global identifier.
        /// </summary>
        /// <param name="GlobalId">The global identifier.</param>
        public static CsvData GetWithGlobalId(int GlobalId)
        {
            if (CsvFiles.Files.TryGetValue(GlobalId / 1000000, out CsvTable Table))
            {
                return Table.GetWithGlobalId(GlobalId);
            }

            return null;
        }
    }
}