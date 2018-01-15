namespace ClashRoyale.CmdHandlers
{
    using System;

    using ClashRoyale.Logic.Collections;

    internal static class StatsHandler
    {
        /// <summary>
        /// Handles the specified arguments.
        /// </summary>
        /// <param name="Args">The arguments.</param>
        internal static void Handle(params string[] Args)
        {
            Console.WriteLine("[*] Devices : " + Devices.Count + ".");
            Console.WriteLine("[*] Players : " + Players.Count + ".");
            Console.WriteLine("[*] Clans   : " + Clans.Count   + ".");
            Console.WriteLine("[*] Battles : " + Battles.Count + ".");
        }
    }
}
