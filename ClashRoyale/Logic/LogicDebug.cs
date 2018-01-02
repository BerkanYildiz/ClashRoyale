namespace GL.Servers.CR.Logic
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using GL.Servers.CR.Core;
    using GL.Servers.CR.Files;
    using GL.Servers.CR.Files.Logic;
    using GL.Servers.CR.Logic.Commands.Server;
    using GL.Servers.CR.Logic.Enums;
    using GL.Servers.CR.Logic.Spells;

    public class LogicDebug
    {
        [Conditional("DEBUG")]
        public static void Execute(string Args, Player[] Players)
        {
            switch (Args)
            {
                case "Add Chest":
                {
                    foreach (Player Player in Players)
                    {
                        if (Player.Connected)
                        {
                            Player.GameMode.CommandManager.AddAvailableServerCommand(new AddChestCommand(true, Player.Arena, "???"));
                        }
                    }

                    break;
                }

                case "Add Spell":
                {
                    foreach (Player Player in Players)
                    {
                        if (Player.Connected)
                        {
                            SpellSet SpellSet = new SpellSet(Player.Arena, null);
                            RarityData Rarity = (RarityData) CSV.Tables.Get(Gamefile.Rarity).Datas[Resources.Random.Next(CSV.Tables.Get(Gamefile.Rarity).Datas.Count - 1)];
                            SpellData Data = SpellSet.GetRandomSpell(Resources.Random, Rarity);

                            if (Data != null)
                            {
                                Player.GameMode.CommandManager.AddAvailableServerCommand(new AllianceUnitReceivedCommand("LogicDebug", Data));
                                break;
                            }
                        }
                    }

                    break;
                }

                case "Add 1000 Diamonds":
                {
                    foreach (Player Player in Players)
                    {
                        if (Player.Connected)
                        {
                            Player.GameMode.CommandManager.AddAvailableServerCommand(new DiamondsAddedCommand(1000));
                        }
                    }

                    break;
                }

                case "Add 10000 Diamonds":
                {
                    foreach (Player Player in Players)
                    {
                        if (Player.Connected)
                        {
                            Player.GameMode.CommandManager.AddAvailableServerCommand(new DiamondsAddedCommand(10000));
                        }
                    }

                    break;
                }

                case "Add 100000 Diamonds":
                {
                    foreach (Player Player in Players)
                    {
                        if (Player.Connected)
                        {
                            Player.GameMode.CommandManager.AddAvailableServerCommand(new DiamondsAddedCommand(100000));
                        }
                    }

                    break;
                }

                case "Remove 1000 Diamonds":
                {
                    foreach (Player Player in Players)
                    {
                        if (Player.Connected)
                        {
                            Player.GameMode.CommandManager.AddAvailableServerCommand(new TransactionsRevokedCommand(1000));
                        }
                    }

                    break;
                }

                case "Remove 10000 Diamonds":
                {
                    foreach (Player Player in Players)
                    {
                        if (Player.Connected)
                        {
                            Player.GameMode.CommandManager.AddAvailableServerCommand(new TransactionsRevokedCommand(10000));
                        }
                    }

                    break;
                }

                case "Remove 100000 Diamonds":
                {
                    foreach (Player Player in Players)
                    {
                        if (Player.Connected)
                        {
                            Player.GameMode.CommandManager.AddAvailableServerCommand(new TransactionsRevokedCommand(100000));
                        }
                    }

                    break;
                }

                case "Unlock Spells":
                {
                    foreach (Player Player in Players)
                    {
                        if (Player.Connected)
                        {
                            foreach (SpellData Data in CSV.Tables.Spells)
                            {
                                if (!Player.Home.HasSpell(Data))
                                {
                                    Player.GameMode.CommandManager.AddAvailableServerCommand(new AllianceUnitReceivedCommand("LogicDebug", Data));
                                }
                            }
                        }
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Gets the list of debug commands.
        /// </summary>
        /// <returns></returns>
        public static string[] GetListOfCommands()
        {
            List<string> Commands = new List<string>();

            Commands.Add("Add Chest");
            Commands.Add("Add Spell");
            Commands.Add("Add 1000 Diamonds");
            Commands.Add("Add 10000 Diamonds");
            Commands.Add("Add 100000 Diamonds");
            Commands.Add("Remove 1000 Diamonds");
            Commands.Add("Remove 10000 Diamonds");
            Commands.Add("Remove 100000 Diamonds");
            Commands.Add("Unlock Spells");

            return Commands.ToArray();
        }
    }
}
