namespace ClashRoyale.Server
{
    using System;

    using ClashRoyale.Server.Handlers;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Commands.Server;

    internal static class CommandLine
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CommandLine"/> has been initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            while (true)
            {
                string Input = Console.ReadLine();

                if (!string.IsNullOrEmpty(Input))
                {
                    string[] Args = Input.Trim().Split(' ');
                    
                    if (Args[0] == "exit")
                    {
                        ExitHandler.Run(Args);
                    }
                    else if (Args[0] == "gems")
                    {
                        Players.ForEach(Player =>
                        {
                            Player.GameMode.CommandManager.AddAvailableServerCommand(new DiamondsAddedCommand(1));
                        });
                    }
                }
            }
        }
    }
}
