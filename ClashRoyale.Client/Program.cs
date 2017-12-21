namespace ClashRoyale.Client
{
    using System;
    
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Network.Packets;

    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        private static void Main()
        {
            Factory.Initialize();
            Tests.Initialize();

            Bot Bot = new Bot();

            Console.ReadKey(false);
        }
    }
}