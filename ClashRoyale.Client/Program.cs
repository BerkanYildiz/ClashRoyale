namespace ClashRoyale.Client
{
    using System;

    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Network.Packets;
    using ClashRoyale.Crypto.Randomizers;
    using ClashRoyale.Extensions.Game;
    using ClashRoyale.Files;
    using ClashRoyale.Files.Csv;

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

            Factory.Initialize();

            new Bot();

            Tests.Initialize();

            Program.Initialized = true;

            Console.ReadKey(false);
        }
    }
}