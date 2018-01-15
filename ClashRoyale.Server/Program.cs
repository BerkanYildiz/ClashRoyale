namespace ClashRoyale
{
    using System;

    using ClashRoyale.CmdHandlers;
    using ClashRoyale.Database;
    using ClashRoyale.Handlers;

    using ClashRoyale.Logic.Battle;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Logic.Event;
    using ClashRoyale.Logic.Inbox;
    using ClashRoyale.Logic.RoyalTv;

    using ClashRoyale.Network;

    internal static class Program
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Program"/> has been initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        internal static void Main()
        {
            Base.Initialize();
            GameDb.Initialize();

            Devices.Initialize();
            Players.Initialize();
            Clans.Initialize();
            Battles.Initialize();
            Leaderboards.Initialize();

            InboxManager.Initialize();
            EventManager.Initialize();
            BattleManager.Initialize();
            RoyalTvManager.Initialize();

            HandlerFactory.Initialize();
            NetworkTcp.Initialize();

            Program.Initialized     = true;
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;

            CommandLine.Initialize();
        }

        /// <summary>
        /// Called when the cancel key has been pressed.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="ConsoleCancelEventArgs">The <see cref="ConsoleCancelEventArgs"/> instance containing the event data.</param>
        private static void ConsoleOnCancelKeyPress(object Sender, ConsoleCancelEventArgs ConsoleCancelEventArgs)
        {
            ConsoleCancelEventArgs.Cancel = true;

            if (Program.Initialized == false)
            {
                Environment.Exit(0);
            }
            else
            {
                ExitHandler.Run("exit", "-f");
            }
        }
    }
}