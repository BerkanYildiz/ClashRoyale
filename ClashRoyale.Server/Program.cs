namespace ClashRoyale.Server
{
    using System;

    using ClashRoyale.Server.Crypto.Randomizers;
    using ClashRoyale.Server.Database;
    using ClashRoyale.Server.Extensions.Game;
    using ClashRoyale.Server.Files;
    using ClashRoyale.Server.Files.Csv;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Event.Manager;
    using ClashRoyale.Server.Logic.Manager;
    using ClashRoyale.Server.Logic.RoyalTV;
    using ClashRoyale.Server.Logic.Scoring;
    using ClashRoyale.Server.Network;
    using ClashRoyale.Server.Network.Packets;

    using Home = ClashRoyale.Server.Files.Home;

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

        internal static XorShift Random;

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        private static void Main()
        {
            Program.Random = new XorShift();

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

            Program.Initialized = true;
            Console.ReadKey(false);
        }
    }
}