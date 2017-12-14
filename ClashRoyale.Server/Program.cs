namespace ClashRoyale.Server
{
    using System;

    using ClashRoyale.Server.Crypto.Randomizers;
    using ClashRoyale.Server.Database;
    using ClashRoyale.Server.Files;
    using ClashRoyale.Server.Files.Csv;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Network;
    using ClashRoyale.Server.Network.Packets;

    internal static class Program
    {
        internal static XorShift Random;

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        private static void Main()
        {
            Program.Random = new XorShift();

            CsvFiles.Initialize();
            Home.Initialize();
            Fingerprint.Initialize();
            GameDb.Initialize();
            Players.Initialize();
            Factory.Initialize();
            TcpGateway.Initialize();

            Console.ReadKey(false);
        }
    }
}