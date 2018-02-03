namespace ClashRoyale.CmdHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Messages.Server.Account;
    using ClashRoyale.Network;

    internal static class ExitHandler
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ExitHandler"/> is already exiting.
        /// </summary>
        internal static bool Exiting
        {
            get;
            set;
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        internal static void Run(params string[] Args)
        {
            if (Program.Initialized == false)
            {
                Environment.Exit(0);
            }

            if (ExitHandler.Exiting)
            {
                return;
            }

            ExitHandler.Exiting = true;

            var WarningsTask    = ExitHandler.SetMaintenance();

            var PlayersTask     = Players.SaveAll();
            var ClansTask       = Clans.SaveAll();
            var BattlesTask     = Battles.SaveAll();

            Task.WaitAll(WarningsTask, PlayersTask, ClansTask, BattlesTask);

            if (Args.Length > 1)
            {
                if (Args[1] == "--force" || Args[1] == "-f")
                {
                    ExitHandler.DisconnectEveryone().Wait();
                    Environment.Exit(0);
                }
            }

            ExitHandler.WaitTillWarnEnd();
            ExitHandler.DisconnectEveryone().Wait();

            Environment.Exit(0);
        }

        /// <summary>
        /// Sets the maintenance mode and warn the players about it.
        /// </summary>
        private static Task SetMaintenance()
        {
            // TODO : Deploy global maintenance.
            // TODO : Dynamically set the maintenance end time.

            Config.Maintenance.Begin(TimeSpan.FromMinutes(15));

            return Task.Run(() =>
            {
                Players.ForEach(Player =>
                {
                    Player.GameMode.Listener.SendMessage(new ServerShutdownMessage());
                });

                Logging.Info(typeof(ExitHandler), "Warned every player about the maintenance.");
            });
        }

        /// <summary>
        /// Disconnects everyone from the server.
        /// </summary>
        private static Task DisconnectEveryone()
        {
            return Task.Run(() =>
            {
                Players.ForEach(Player =>
                {
                    Player.GameMode.Listener.Disconnect();
                });

                Logging.Info(typeof(ExitHandler), "Disconnected every player for the maintenance.");
            });
        }

        /// <summary>
        /// Blocks the thread until the maintenance warning event ends.
        /// </summary>
        private static void WaitTillWarnEnd()
        {
            while (Config.Maintenance.Warning.TimeLeft.Seconds > 0)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
