namespace ClashRoyale.Server
{
    using System;

    using ClashRoyale.Crypto.Randomizers;
    using ClashRoyale.Extensions.Game;
    using ClashRoyale.Files;
    using ClashRoyale.Files.Csv;

    using ClashRoyale.Server.Database;
    using ClashRoyale.Server.Logic.Battle.Manager;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Event.Manager;
    using ClashRoyale.Server.Logic.Inbox;
    using ClashRoyale.Server.Logic.RoyalTV;
    using ClashRoyale.Server.Logic.Scoring;
    using ClashRoyale.Server.Network;
    using ClashRoyale.Server.Network.Packets;

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

            Battles.Initialize();
            Players.Initialize();
            Clans.Initialize();

            Factory.Initialize();
            TcpGateway.Initialize();

            Tests.Initialize();

            Program.Initialized = true;
            Console.ReadKey(false);
        }
    }
}