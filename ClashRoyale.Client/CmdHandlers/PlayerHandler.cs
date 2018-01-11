namespace ClashRoyale.CmdHandlers
{
    using System;
    using System.Reflection;

    using ClashRoyale.Logic.Player;
    using ClashRoyale.Maths;

    public static class PlayerHandler
    {
        private static LogicLong SelectedPlayer;

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
                    PlayerHandler.Select(Args);
                }
                else if (Args[1] == "deselect")
                {
                    PlayerHandler.Deselect(Args);
                }
                else if (Args[1] == "profile")
                {
                    PlayerHandler.Profile(Args);
                }
                else if (Args[1] == "disconnect")
                {
                    PlayerHandler.Disconnect(Args);
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

            var Player = PlayerHandler.GetEntity(HighId, LowId);

            if (Player != null)
            {
                PlayerHandler.SelectedPlayer = Player.PlayerId;

                if (PlayerHandler.SelectedPlayer.IsZero == false)
                {
                    Console.WriteLine("[*] Selected player " + PlayerHandler.SelectedPlayer + " called " + Player.Name + ".");
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

            if (PlayerHandler.SelectedPlayer.IsZero == false)
            {
                PlayerHandler.SelectedPlayer = LogicLong.Empty;
            }

            Console.WriteLine("[*] " + "Deselected the specified player.");
        }

        internal static Player GetEntity(int HighId, int LowId)
        {
            LogicLong EntityId = new LogicLong(HighId, LowId);
            
            if (EntityId.IsZero == false)
            {
                Player Player = null;

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

            if (Args[1] != "profile")
            {
                return;
            }

            var Player = PlayerHandler.GetEntity(PlayerHandler.SelectedPlayer.HigherInt, PlayerHandler.SelectedPlayer.LowerInt);

            if (Player != null)
            {
                PlayerHandler.ShowValues(Player);
            }
            else
            {
                Console.WriteLine("[*] Invalid arguments, please select a valid player first.");
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
        /// Disconnects the specified <see cref="Player"/>.
        /// </summary>
        /// <param name="Args">The arguments.</param>
        internal static void Disconnect(params string[] Args)
        {
            var Player = PlayerHandler.GetEntity(PlayerHandler.SelectedPlayer.HigherInt, PlayerHandler.SelectedPlayer.LowerInt);

            if (Player != null)
            {
                if (Player.IsConnected)
                {
                    // NetworkTcp.Disconnect(Player.GameMode.Device.Token.AsyncEvent);

                    if (Player.IsConnected == false)
                    {
                        Console.WriteLine("[*] Success, player has been disconnected.");
                    }
                    else
                    {
                        Console.WriteLine("[*] Operation failed, player is still connected.");
                    }
                }
                else
                {
                    Console.WriteLine("[*] Operation aborted, player is already disconnected.");
                }
            }
            else
            {
                Console.WriteLine("[*] Invalid arguments, please select a valid player first.");
            }
        }
    }
}
