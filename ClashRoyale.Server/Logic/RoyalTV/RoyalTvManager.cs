namespace ClashRoyale.Server.Logic.RoyalTV
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using ClashRoyale.Extensions.Game;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.RoyalTV.Entry;

    internal static class RoyalTvManager
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RoyalTvManager"/> has been already initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            set;
        }

        internal static List<RoyalTvEntry>[] Channels;
        internal static int Seed;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
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
        internal static void AddEntry(int ChannelIdx, RoyalTvEntry Entry)
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
        internal static int GetChannelArenaData(ArenaData ArenaData)
        {
            return Array.IndexOf(ClientGlobals.TvArenas, ArenaData);
        }

        /// <summary>
        /// Gets the entry by index.
        /// </summary>
        internal static RoyalTvEntry GetEntryByIdx(int ChannelIdx, int Idx)
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