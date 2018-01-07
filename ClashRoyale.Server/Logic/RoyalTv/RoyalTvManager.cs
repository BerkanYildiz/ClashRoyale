namespace ClashRoyale.Logic.RoyalTv
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using ClashRoyale.Extensions.Game;
    using ClashRoyale.Files.Csv.Logic;

    public static class RoyalTvManager
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RoyalTvManager"/> has been already initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        public static List<RoyalTvEntry>[] Channels;
        public static int Seed;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            if (RoyalTvManager.Initialized)
            {
                return;
            }

            RoyalTvManager.Channels = new List<RoyalTvEntry>[ClientGlobals.TvArenas.Length];

            for (int I = 0; I < RoyalTvManager.Channels.Length; I++)
            {
                RoyalTvManager.Channels[I] = new List<RoyalTvEntry>(25);
            }

            RoyalTvManager.Initialized = true;
        }

        /// <summary>
        /// Adds the specified entry.
        /// </summary>
        public static void AddEntry(int ChannelIdx, RoyalTvEntry Entry)
        {
            if (ChannelIdx > -1 && RoyalTvManager.Channels.Length > ChannelIdx)
            {
                Entry.RunningId = Interlocked.Increment(ref RoyalTvManager.Seed);
                RoyalTvManager.Channels[ChannelIdx].Add(Entry);
            }
        }

        /// <summary>
        /// Gets the channel index by arena data
        /// </summary>
        public static int GetChannelArenaData(ArenaData ArenaData)
        {
            return Array.IndexOf(ClientGlobals.TvArenas, ArenaData);
        }

        /// <summary>
        /// Gets the entry by index.
        /// </summary>
        public static RoyalTvEntry GetEntryByIdx(int ChannelIdx, int Idx)
        {
            if (RoyalTvManager.Channels.Length > ChannelIdx)
            {
                if (RoyalTvManager.Channels[ChannelIdx].Count > Idx)
                {
                    return RoyalTvManager.Channels[ChannelIdx][Idx];
                }
            }

            return null;
        }
    }
}