namespace ClashRoyale.Proxy
{
    using System;

    using ClashRoyale.Proxy.Logic.Collections;
    using ClashRoyale.Proxy.Network;

    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        private static void Main()
        {
            Launcher.Initialize();
            PacketType.Initialize();
            Devices.Initialize();
            new Gateway();
            Console.ReadKey(false);
        }
    }
}