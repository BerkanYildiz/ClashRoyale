namespace ClashRoyale
{
    using System.Threading;

    using ClashRoyale.Handlers;
    using ClashRoyale.Logic.Collections;
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

            Devices.Initialize();

            HandlerFactory.Initialize();
            NetworkTcp.Initialize();

            Program.Initialized = true;

            Thread.Sleep(-1);
        }
    }
}