namespace ClashRoyale.Logic.Event.Manager
{
    using ClashRoyale.Extensions;

    public static class EventManager
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="EventManager"/> has been already initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
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
        public static void Encode(ByteStream Stream)
        {
            // TODO : Implement EventManager::Encode(ByteSteam).
        }
    }
}