namespace ClashRoyale
{
    using ClashRoyale.Database;
    using ClashRoyale.Network;

    public class Program
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
        private static void Main()
        {
            Base.Initialize();
            GameDb.Initialize();

            Handlers.Handlers.Initialize();
            NetworkTcp.Initialize();

            Program.Initialized = true;

            CommandLine.Initialize();
        }
    }
}