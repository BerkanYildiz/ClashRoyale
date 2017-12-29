namespace ClashRoyale.Server.Handlers
{
    using System;

    using ClashRoyale.Maths;
    using ClashRoyale.Server.Logic.Collections;

    internal static class SelectHandler
    {
        private static LogicLong SelectedPlayer;
        private static LogicLong SelectedClan;

        /// <summary>
        /// Runs this instance.
        /// </summary>
        internal static void Use(params string[] Args)
        {
            if (Args.Length < 4)
            {
                return;
            }

            if (!int.TryParse(Args[2], out int HighId))
            {
                return;
            }

            if (!int.TryParse(Args[3], out int LowId))
            {
                return;
            }

            switch (Args[1])
            {
                case "player":
                {
                    SelectHandler.SelectedPlayer = new LogicLong(HighId, LowId);

                    if (SelectHandler.SelectedPlayer.IsZero == false)
                    {
                        var Player = Players.Get(HighId, LowId, false).Result;

                        if (Player != null)
                        {
                            Console.WriteLine("[*] Selected : ");
                            Console.WriteLine("[*]  - Type  : " + "Player");
                            Console.WriteLine("[*]  - ID    : " + SelectHandler.SelectedPlayer);
                            Console.WriteLine("[*]  - Name  : " + Player.Name);
                        }
                    }

                    break;
                }

                case "clan":
                {
                    SelectHandler.SelectedClan = new LogicLong(HighId, LowId);

                    if (SelectHandler.SelectedClan.IsZero == false)
                    {
                        var Clan = Clans.Get(HighId, LowId, false).Result;

                        if (Clan != null)
                        {
                            Console.WriteLine("[*] Selected : ");
                            Console.WriteLine("[*]  - Type  : " + "Clan");
                            Console.WriteLine("[*]  - ID    : " + SelectHandler.SelectedClan);
                            Console.WriteLine("[*]  - Name  : " + Clan.HeaderEntry.Name);
                        }
                    }

                    break;
                }
            }
        }
    }
}
