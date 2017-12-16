namespace ClashRoyale.Server.Extensions.Game
{
    using ClashRoyale.Server.Files.Csv;
    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal static class Globals
    {
        internal static int StartingGold;
        internal static int StartingDiamonds;
        internal static int MaxChest;
        internal static int StartMana;
        internal static int MaxMana;
        internal static int ManaRegenMs;
        internal static int ManaRegenMsEnd;
        internal static int ManaRegenMsOvertime;
        internal static int ManaSpeedUpWhenRemainingSecs;
        internal static int MaxMessageLength;
        internal static int AllianceCreateCost;
        internal static int MaxChestOpening;
        internal static int CrownChestCrownCount;
        internal static int FreeChestIntervalHours;
        internal static int CrownChestCooldownHours;
        internal static int LeaveAllianceDonationCooldown;
        internal static int ChestCatchupChance;
        internal static int TournamentMatchLengthSeconds;
        internal static int TournamentOvertimeLengthSeconds;
        internal static int[] FreeChestDimaondLoop;
        internal static int[] CrownDimaondLoop;

        internal static bool MultipleDecks;
        internal static bool QuestsEnabled;
        internal static bool RefreshArenaInLoadingFinished;

        internal static ArenaData StartingArena;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            Globals.StartingGold = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("STARTING_GOLD").NumberValue;
            Globals.StartingDiamonds = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("STARTING_DIAMONDS").NumberValue;
            Globals.AllianceCreateCost = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("ALLIANCE_CREATE_COST").NumberValue;
            Globals.MultipleDecks = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("MULTIPLE_DECKS_ENABLED").BooleanValue;
            Globals.QuestsEnabled = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("QUESTS_ENABLED").BooleanValue;
            Globals.MaxChest = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("MAX_CHEST_COUNT").NumberValue;
            Globals.StartMana = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("START_MANA").NumberValue;
            Globals.MaxMana = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("MAX_MANA").NumberValue;
            Globals.ManaRegenMs = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("MANA_REGEN_MS").NumberValue;
            Globals.ManaRegenMsEnd = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("MANA_REGEN_MS_END").NumberValue;
            Globals.ManaRegenMsOvertime = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("MANA_REGEN_MS_OVERTIME").NumberValue;
            Globals.MaxMessageLength = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("MAX_MESSAGE_LENGTH").NumberValue;
            Globals.MaxChestOpening = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("MAX_CHESTS_OPENING").NumberValue;
            Globals.ChestCatchupChance = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("CHEST_CATCHUP_CHANCE").NumberValue;
            Globals.CrownChestCrownCount = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("CROWN_CHEST_CROWN_COUNT").NumberValue;
            Globals.FreeChestIntervalHours = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("FREE_CHEST_INTERVAL_HOURS").NumberValue;
            Globals.CrownChestCooldownHours = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("CROWN_CHEST_COOLDOWN_HOURS").NumberValue;
            Globals.LeaveAllianceDonationCooldown = 60 * CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("LEAVE_ALLIANCE_DONATION_COOLDOWN_MINUTES").NumberValue;
            Globals.RefreshArenaInLoadingFinished = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("REFRESH_ARENA_IN_LOADING_FINISHED").BooleanValue;
            Globals.FreeChestDimaondLoop = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("FREE_CHEST_DIAMONDS").NumberArray;
            Globals.CrownDimaondLoop = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("CROWN_CHEST_DIAMONDS").NumberArray;
            Globals.StartingArena = CsvFiles.Get(Gamefile.Arena).GetData<ArenaData>(CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("STARTING_ARENA").TextValue);
            Globals.TournamentMatchLengthSeconds = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("TOURNAMENT_MATCH_LENGTH_SECONDS").NumberValue;
            Globals.TournamentOvertimeLengthSeconds = CsvFiles.Get(Gamefile.Global).GetData<GlobalData>("TOURNAMENT_OVERTIME_LENGTH_SECONDS").NumberValue;
        }
    }
}