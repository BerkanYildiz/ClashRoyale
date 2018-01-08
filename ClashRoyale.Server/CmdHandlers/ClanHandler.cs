namespace ClashRoyale.CmdHandlers
{
    using System;
    using System.Reflection;

    using ClashRoyale.Logic.Alliance;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Maths;

    internal static class ClanHandler
    {
        private static LogicLong SelectedClan;

        /// <summary>
        /// Handles the specified arguments.
        /// </summary>
        /// <param name="Args">The arguments.</param>
        internal static void Handle(params string[] Args)
        {
            if (Args.Length < 2)
            {
                Console.WriteLine("[*] Invalid arguments, please use a valid command.");
            }
            else
            {
                if (Args[1] == "select")
                {
                    ClanHandler.Select(Args);
                }
                else if (Args[1] == "deselect")
                {
                    ClanHandler.Deselect(Args);
                }
                else if (Args[1] == "profile")
                {
                    ClanHandler.Profile(Args);
                }
                else if (Args[1] == "disconnect")
                {
                    ClanHandler.Disconnect(Args);
                }
            }
        }

        /// <summary>
        /// Selects the specified entity.
        /// </summary>
        /// <param name="Args">The arguments.</param>
        internal static void Select(params string[] Args)
        {
            if (Args.Length < 4)
            {
                return;
            }

            if (Args[1] != "select")
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

            var Clan = ClanHandler.GetEntity(HighId, LowId);

            if (Clan != null)
            {
                ClanHandler.SelectedClan = Clan.AllianceId;

                if (ClanHandler.SelectedClan.IsZero == false)
                {
                    Console.WriteLine("[*] Selected clan " + ClanHandler.SelectedClan + " called " + Clan.HeaderEntry.Name + ", with " + Clan.HeaderEntry.Score + " and " + Clan.HeaderEntry.MembersCount + " members.");
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
                return;
            }

            if (Args[1] != "deselect")
            {
                return;
            }

            if (ClanHandler.SelectedClan.IsZero == false)
            {
                ClanHandler.SelectedClan = LogicLong.Empty;
            }

            Console.WriteLine("[*] " + "Deselected the specified clan.");
        }

        internal static Clan GetEntity(int HighId, int LowId)
        {
            LogicLong EntityId = new LogicLong(HighId, LowId);
            
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
                return;
            }

            if (Args[1] != "clan")
            {
                return;
            }

            var Clan = ClanHandler.GetEntity(ClanHandler.SelectedClan.HigherInt, ClanHandler.SelectedClan.LowerInt);

            if (Clan != null)
            {
                ClanHandler.ShowValues(Clan);
            }
            else
            {
                Console.WriteLine("[*] Invalid arguments, please select a valid clan first.");
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

        /// <summary>
        /// Disconnects the specified <see cref="Clan"/>.
        /// </summary>
        /// <param name="Args">The arguments.</param>
        internal static void Disconnect(params string[] Args)
        {
            Console.WriteLine("[*] " + "Invalid arguments, a clan cannot be disconnected.");
        }
    }
}
