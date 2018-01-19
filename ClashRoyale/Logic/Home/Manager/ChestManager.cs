namespace ClashRoyale.Logic.Home.Manager
{
    using System.Collections.Generic;

    using ClashRoyale.Extensions;

    public class ChestManager
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ChestManager"/> is initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        public static List<ChestEvent> Chests;
        
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            if (ChestManager.Initialized)
            {
                return;
            }

            ChestManager.Chests      = new List<ChestEvent>();
            ChestManager.Initialized = true;
        }

        /// <summary>
        /// Decodes from the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public static void Decode(ByteStream Stream)
        {
            int Length = Stream.ReadVInt();

            for (int i = 0; i < Length; i++)
            {
                ChestEvent ChestEvent = new ChestEvent();
                ChestEvent.Decode(Stream);
                ChestManager.Chests.Add(ChestEvent);
            }
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public static void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteVInt(ChestManager.Chests.Count);

            foreach (ChestEvent ChestEvent in ChestManager.Chests)
            {
                ChestEvent.Encode(Stream);
            }
        }
    }
}
