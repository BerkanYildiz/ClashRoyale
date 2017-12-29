namespace ClashRoyale.Server.Handlers
{
    using System;
    using System.Reflection;

    using ClashRoyale.Maths;
    using ClashRoyale.Server.Logic.Alliance;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Player;

    internal static class SelectHandler
    {
        private static LogicLong SelectedPlayer;
        private static LogicLong SelectedClan;

        /// <summary>
        /// Selects the specified entity.
        /// </summary>
        /// <param name="Args">The arguments.</param>
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
                    var Player = (Player) SelectHandler.GetEntity<Player>(HighId, LowId);

                    if (Player != null)
                    {
                        SelectHandler.SelectedPlayer = Player.PlayerId;

                        Console.WriteLine("[*] Selected : ");
                        Console.WriteLine("[*]  - Type  : " + "Player");
                        Console.WriteLine("[*]  - ID    : " + Player.PlayerId);
                        Console.WriteLine("[*]  - Name  : " + Player.Name);
                    }

                    break;
                }

                case "clan":
                {
                    var Clan = (Clan) SelectHandler.GetEntity<Clan>(HighId, LowId);

                    if (Clan != null)
                    {
                        SelectHandler.SelectedClan = Clan.AllianceId;

                        Console.WriteLine("[*] Selected : ");
                        Console.WriteLine("[*]  - Type  : " + "Clan");
                        Console.WriteLine("[*]  - ID    : " + Clan.AllianceId);
                        Console.WriteLine("[*]  - Name  : " + Clan.HeaderEntry.Name);
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Deselects the specified entity.
        /// </summary>
        /// <param name="Args">The arguments.</param>
        internal static void Deselect(params string[] Args)
        {
            if (Args.Length < 2)
            {
                SelectHandler.SelectedPlayer = LogicLong.Empty;
                SelectHandler.SelectedClan   = LogicLong.Empty;

                Console.WriteLine("[*] " + "Deselected both player and clan.");
            }
            else
            {
                switch (Args[1])
                {
                    case "player":
                    {
                        SelectHandler.SelectedPlayer = LogicLong.Empty;
                        break;
                    }

                    case "clan":
                    {
                        SelectHandler.SelectedClan = LogicLong.Empty;
                        break;
                    }

                    default:
                    {
                        return;
                    }
                }

                Console.WriteLine("[*] " + "Deselected the specified " + Args[1] + ".");
            }
        }

        internal static object GetEntity<T>(int HighId, int LowId)
        {
            LogicLong EntityId = new LogicLong(HighId, LowId);

            if (typeof(T) == typeof(Player))
            {
                if (EntityId.IsZero == false)
                {
                    var Player = Players.Get(EntityId.HigherInt, EntityId.LowerInt, false).Result;

                    if (Player != null)
                    {
                        return Player;
                    }
                    else
                    {
                        Console.WriteLine("[*] Invalid arguments, the specified player doesn't exist.");
                    }
                }
                else
                {
                    Console.WriteLine("[*] Missing arguments, please select a player first.");
                }
            }
            else if (typeof(T) == typeof(Clan))
            {
                if (EntityId.IsZero == false)
                {
                    var Clan = Clans.Get(EntityId.HigherInt, EntityId.LowerInt, false).Result;

                    if (Clan != null)
                    {
                        return Clan;
                    }
                    else
                    {
                        Console.WriteLine("[*] Invalid arguments, the specified clan doesn't exist.");
                    }
                }
                else
                {
                    Console.WriteLine("[*] Missing arguments, please select a clan first.");
                }
            }

            return null;
        }

        /// <summary>
        /// Shows the profile of the entity.
        /// </summary>
        /// <param name="Args">The arguments.</param>
        internal static void Profile(params string[] Args)
        {
            if (Args.Length < 2)
            {
                if (SelectHandler.SelectedClan.IsZero && SelectHandler.SelectedPlayer.IsZero == false)
                {
                    var Player = SelectHandler.GetEntity<Player>(SelectHandler.SelectedPlayer.HigherInt, SelectHandler.SelectedPlayer.LowerInt);

                    if (Player != null)
                    {
                        SelectHandler.ShowValues(Player);
                    }
                }
                else if (SelectHandler.SelectedPlayer.IsZero && SelectHandler.SelectedClan.IsZero == false)
                {
                    var Clan = SelectHandler.GetEntity<Clan>(SelectHandler.SelectedClan.HigherInt, SelectHandler.SelectedClan.LowerInt);

                    if (Clan != null)
                    {
                        SelectHandler.ShowValues(Clan);
                    }
                }
                else
                {
                    Console.WriteLine("[*] Missing arguments, use \"profile <entityType>\" where entityType is either \"player\" or \"clan\".");
                }

                return;
            }

            switch (Args[1])
            {
                case "player":
                {
                    var Player = SelectHandler.GetEntity<Player>(SelectHandler.SelectedPlayer.HigherInt, SelectHandler.SelectedPlayer.LowerInt);

                    if (Player != null)
                    {
                        SelectHandler.ShowValues(Player);
                    }

                    break;
                }

                case "clan":
                {
                    var Clan = SelectHandler.GetEntity<Clan>(SelectHandler.SelectedClan.HigherInt, SelectHandler.SelectedClan.LowerInt);

                    if (Clan != null)
                    {
                        SelectHandler.ShowValues(Clan);
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Shows the values.
        /// </summary>
        internal static void ShowValues(object Entity)
        {
            foreach (FieldInfo Field in Entity.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (Field != null)
                {
                    Console.WriteLine("[*] " + Field.Name + " : " + (Field.GetValue(Entity) != null ? Field.GetValue(Entity).ToString() : "(null)") + ".");
                }
            }
        }
    }
}
