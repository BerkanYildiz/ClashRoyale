namespace ClashRoyale.Extensions.Game
{
    using ClashRoyale.Enums;
    using ClashRoyale.Files.Csv;
    using ClashRoyale.Files.Csv.Logic;

    public static class Globals
    {
        public static int StartingGold;
        public static int StartingDiamonds;
        public static int MaxChest;
        public static int StartMana;
        public static int MaxMana;
        public static int ManaRegenMs;
        public static int ManaRegenMsEnd;
        public static int ManaRegenMsOvertime;
        public static int ManaSpeedUpWhenRemainingSecs;
        public static int MaxMessageLength;
        public static int AllianceCreateCost;
        public static int MaxChestOpening;
        public static int CrownChestCrownCount;
        public static int FreeChestIntervalHours;
        public static int CrownChestCooldownHours;
        public static int LeaveAllianceDonationCooldown;
        public static int ChestCatchupChance;
        public static int TournamentMatchLengthSeconds;
        public static int TournamentOvertimeLengthSeconds;
        public static int[] FreeChestDiamondLoop;
        public static int[] CrownDiamondLoop;

        public static bool MultipleDecks;
        public static bool QuestsEnabled;
        public static bool RefreshArenaInLoadingFinished;

        public static ArenaData StartingArena;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            Globals.StartingGold                    = 100 * CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("STARTING_GOLD").NumberValue;
            Globals.StartingDiamonds                = 100 * CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("STARTING_DIAMONDS").NumberValue;

            Globals.AllianceCreateCost              = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("ALLIANCE_CREATE_COST").NumberValue;
            Globals.MultipleDecks                   = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("MULTIPLE_DECKS_ENABLED").BooleanValue;
            Globals.QuestsEnabled                   = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("QUESTS_ENABLED").BooleanValue;
            Globals.MaxChest                        = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("MAX_CHEST_COUNT").NumberValue;
            Globals.StartMana                       = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("START_MANA").NumberValue;
            Globals.MaxMana                         = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("MAX_MANA").NumberValue;
            Globals.ManaRegenMs                     = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("MANA_REGEN_MS").NumberValue;
            Globals.ManaRegenMsEnd                  = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("MANA_REGEN_MS_END").NumberValue;
            Globals.ManaRegenMsOvertime             = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("MANA_REGEN_MS_OVERTIME").NumberValue;
            Globals.MaxMessageLength                = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("MAX_MESSAGE_LENGTH").NumberValue;
            Globals.MaxChestOpening                 = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("MAX_CHESTS_OPENING").NumberValue;
            Globals.ChestCatchupChance              = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("CHEST_CATCHUP_CHANCE").NumberValue;
            Globals.CrownChestCrownCount            = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("CROWN_CHEST_CROWN_COUNT").NumberValue;
            Globals.FreeChestIntervalHours          = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("FREE_CHEST_INTERVAL_HOURS").NumberValue;
            Globals.CrownChestCooldownHours         = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("CROWN_CHEST_COOLDOWN_HOURS").NumberValue;
            Globals.LeaveAllianceDonationCooldown   = 60 * CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("LEAVE_ALLIANCE_DONATION_COOLDOWN_MINUTES").NumberValue;
            Globals.RefreshArenaInLoadingFinished   = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("REFRESH_ARENA_IN_LOADING_FINISHED").BooleanValue;
            Globals.FreeChestDiamondLoop            = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("FREE_CHEST_DIAMONDS").NumberArray;
            Globals.CrownDiamondLoop                = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("CROWN_CHEST_DIAMONDS").NumberArray;
            Globals.StartingArena                   = CsvFiles.Get(Gamefile.Arena).GetData<ArenaData>(CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("STARTING_ARENA").TextValue);
            Globals.TournamentMatchLengthSeconds    = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("TOURNAMENT_MATCH_LENGTH_SECONDS").NumberValue;
            Globals.TournamentOvertimeLengthSeconds = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("TOURNAMENT_OVERTIME_LENGTH_SECONDS").NumberValue;
        }
    }
}