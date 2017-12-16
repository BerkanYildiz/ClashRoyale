namespace ClashRoyale.Server.Logic.Event.Manager
{
    using ClashRoyale.Server.Extensions;

    internal static class EventManager
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="EventManager"/> has been already initialized.
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
            if (EventManager.Initialized)
            {
                return;
            }

            // EventManager.

            EventManager.Initialized = true;
        }

        /// <summary>
        /// Encodes the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal static void Encode(ByteStream Stream)
        {
            // TODO : Implement EventManager::Encode(ByteSteam).
        }
    }
}