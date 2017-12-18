namespace ClashRoyale.Client.Packets
{
    using System;
    using System.Collections.Generic;

    internal class CommandFactory
    {
        internal static Dictionary<int, Type> Commands = new Dictionary<int, Type>();
    }
}