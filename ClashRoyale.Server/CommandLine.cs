namespace ClashRoyale.Server
{
    using System;

    using ClashRoyale.Server.Handlers;

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
                Console.Write("[*] > ");

                string Input = Console.ReadLine();

                if (!string.IsNullOrEmpty(Input))
                {
                    string[] Args = Input.Trim().Split(' ');

                    if (Args[0] == "clear")
                    {
                        Console.Clear();
                    }
                    else if (Args[0] == "exit")
                    {
                        ExitHandler.Run(Args);
                    }
                    else if (Args[0] == "player")
                    {
                        PlayerHandler.Handle(Args);
                    }
                    else if (Args[0] == "clan")
                    {
                        ClanHandler.Handle(Args);
                    }
                }
            }
        }
    }
}
