namespace ClashRoyale.Client
{
    using System;

    using ClashRoyale.Client.Core;
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Packets;

    internal class Program
    {
        internal static Resources Resources;

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        private static void Main()
        {
            new MessageFactory();
            new CommandFactory();

            Client Client = new Client();

            Console.ReadKey(false);
        }
    }
}