namespace ClashRoyale.CmdHandlers
{
    using System;

    public static class ExitHandler
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
            if (ExitHandler.Exiting)
            {
                return;
            }

            ExitHandler.Exiting = true;

            if (Args.Length > 1)
            {
                if (Args[1] == "--force" || Args[1] == "-f")
                {
                    Environment.Exit(0);
                }
            }

            Environment.Exit(0);
        }
    }
}
