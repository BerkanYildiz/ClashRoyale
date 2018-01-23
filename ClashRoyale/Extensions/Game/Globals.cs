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

        public static int ResourceDiamondCost1000000;
        public static int ResourceDiamondCost100000;
        public static int ResourceDiamondCost10000;
        public static int ResourceDiamondCost1000;
        public static int ResourceDiamondCost100;
        public static int ResourceDiamondCost10;
        public static int ResourceDiamondCost1;

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
            Globals.StartingGold                    = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("STARTING_GOLD").NumberValue;
            Globals.StartingDiamonds                = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("STARTING_DIAMONDS").NumberValue;

            Globals.AllianceCreateCost              = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("ALLIANCE_CREATE_COST").NumberValue;
            Globals.MultipleDecks                   = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("MULTIPLE_DECKS_ENABLED").BooleanValue;
            Globals.QuestsEnabled                   = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("QUESTS_ENABLED").BooleanValue;
            Globals.MaxChest                        = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("MAX_CHEST_COUNT").NumberValue;
            Globals.StartMana                       = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("START_MANA").NumberValue;
            Globals.MaxMana                         = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("MAX_MANA").NumberValue;
            Globals.ManaRegenMs                     = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("MANA_REGEN_MS").NumberValue;
            Globals.ManaRegenMsEnd                  = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("MANA_REGEN_MS_END").NumberValue;
            Globals.ManaRegenMsOvertime             = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("MANA_REGEN_MS_OVERTIME").NumberValue;
            Globals.MaxMessageLength                = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("MAX_MESSAGE_LENGTH").NumberValue;
            Globals.MaxChestOpening                 = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("MAX_CHESTS_OPENING").NumberValue;
            Globals.ChestCatchupChance              = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("CHEST_CATCHUP_CHANCE").NumberValue;
            Globals.CrownChestCrownCount            = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("CROWN_CHEST_CROWN_COUNT").NumberValue;
            Globals.FreeChestIntervalHours          = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("FREE_CHEST_INTERVAL_HOURS").NumberValue;
            Globals.CrownChestCooldownHours         = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("CROWN_CHEST_COOLDOWN_HOURS").NumberValue;
            Globals.LeaveAllianceDonationCooldown   = 60 * CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("LEAVE_ALLIANCE_DONATION_COOLDOWN_MINUTES").NumberValue;
            Globals.RefreshArenaInLoadingFinished   = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("REFRESH_ARENA_IN_LOADING_FINISHED").BooleanValue;
            Globals.FreeChestDiamondLoop            = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("FREE_CHEST_DIAMONDS").NumberArray;
            Globals.CrownDiamondLoop                = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("CROWN_CHEST_DIAMONDS").NumberArray;
            Globals.StartingArena                   = CsvFiles.Get(Gamefile.Arenas).GetData<ArenaData>(CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("STARTING_ARENA").TextValue);
            Globals.TournamentMatchLengthSeconds    = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("TOURNAMENT_MATCH_LENGTH_SECONDS").NumberValue;
            Globals.TournamentOvertimeLengthSeconds = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("TOURNAMENT_OVERTIME_LENGTH_SECONDS").NumberValue;

            Globals.ResourceDiamondCost1000000      = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("RESOURCE_DIAMOND_COST_1000000").NumberValue;
            Globals.ResourceDiamondCost100000       = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("RESOURCE_DIAMOND_COST_100000").NumberValue;
            Globals.ResourceDiamondCost10000        = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("RESOURCE_DIAMOND_COST_10000").NumberValue;
            Globals.ResourceDiamondCost1000         = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("RESOURCE_DIAMOND_COST_1000").NumberValue;
            Globals.ResourceDiamondCost100          = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("RESOURCE_DIAMOND_COST_100").NumberValue;
            Globals.ResourceDiamondCost10           = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("RESOURCE_DIAMOND_COST_10").NumberValue;
            Globals.ResourceDiamondCost1            = CsvFiles.Get(Gamefile.Globals).GetData<GlobalData>("RESOURCE_DIAMOND_COST_1").NumberValue;
        }

        /// <summary>
        ///     Gets the resource diamonds cost.
        /// </summary>
        public static int GetResourceCost(int resourceCount)
        {
            if (resourceCount > 0)
            {
                if (resourceCount > 9)
                {
                    if (resourceCount > 99)
                    {
                        if (resourceCount > 999)
                        {
                            if (resourceCount > 9999)
                            {
                                if (resourceCount > 99999)
                                {
                                    return Globals.ResourceDiamondCost100000 + ((Globals.ResourceDiamondCost1000000 - Globals.ResourceDiamondCost100000) * (resourceCount / 100 - 1000) + 4500) / 9000;
                                }

                                return Globals.ResourceDiamondCost10000 + ((Globals.ResourceDiamondCost100000 - Globals.ResourceDiamondCost10000) * (resourceCount / 10 - 1000) + 4500) / 9000;
                            }

                            return Globals.ResourceDiamondCost1000 + ((Globals.ResourceDiamondCost10000 - Globals.ResourceDiamondCost1000) * (resourceCount - 1000) + 4500) / 9000;
                        }

                        return Globals.ResourceDiamondCost100 + ((Globals.ResourceDiamondCost1000 - Globals.ResourceDiamondCost100) * (resourceCount - 100) + 4500) / 9000;
                    }

                    return Globals.ResourceDiamondCost10 + ((Globals.ResourceDiamondCost100 - Globals.ResourceDiamondCost10) * (resourceCount - 10) + 4500) / 9000;
                }

                return Globals.ResourceDiamondCost1;
            }

            return 0;
        }
    }
}