namespace ClashRoyale
{
    using ClashRoyale.Database;
    using ClashRoyale.Logic.Battle;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Logic.Event;
    using ClashRoyale.Logic.Inbox;
    using ClashRoyale.Logic.RoyalTv;

    using ClashRoyale.Network;

    internal static class Program
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Program"/> has been initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        internal static void Main()
        {
            Base.Initialize();
            GameDb.Initialize();

            Players.Initialize();
            Clans.Initialize();
            Battles.Initialize();
            Leaderboards.Initialize();

            InboxManager.Initialize();
            EventManager.Initialize();
            BattleManager.Initialize();
            RoyalTvManager.Initialize();

            Handlers.Handlers.Initialize();
            NetworkTcp.Initialize();

            Program.Initialized = true;

            CommandLine.Initialize();
        }
    }
}