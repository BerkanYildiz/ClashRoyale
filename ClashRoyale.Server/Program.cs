namespace ClashRoyale
{
    using ClashRoyale.Crypto.Randomizers;
    using ClashRoyale.Extensions.Game;
    using ClashRoyale.Files;
    using ClashRoyale.Files.Csv;
    using ClashRoyale.Handlers;
    using ClashRoyale.Logic.Battle;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Logic.Event.Manager;
    using ClashRoyale.Logic.Inbox;
    using ClashRoyale.Logic.RoyalTv;
    using ClashRoyale.Network;

    using GameDb = ClashRoyale.Database.GameDb;

    internal class Program
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
        private static void Main()
        {
            Sentry.Initialize();
            XorShift.Initialize();

            CsvFiles.Initialize();
            Fingerprint.Initialize();
            Home.Initialize();
            
            Globals.Initialize();
            ClientGlobals.Initialize();
            GameDb.Initialize();

            Players.Initialize();
            Clans.Initialize();
            Battles.Initialize();
            Leaderboards.Initialize();

            InboxManager.Initialize();
            EventManager.Initialize();
            BattleManager.Initialize();
            RoyalTvManager.Initialize();

            Factory.Initialize();
            NetworkTcp.Initialize();

            Tests.Initialize();

            Program.Initialized = true;

            CommandLine.Initialize();
        }
    }
}