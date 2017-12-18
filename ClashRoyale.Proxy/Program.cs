namespace ClashRoyale.Proxy
{
    using ClashRoyale.Proxy.Logic;
    using ClashRoyale.Proxy.Logic.Collections;

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
        }
    }
}