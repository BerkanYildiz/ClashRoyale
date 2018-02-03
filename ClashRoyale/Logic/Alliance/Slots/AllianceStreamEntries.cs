namespace ClashRoyale.Logic.Alliance.Slots
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using ClashRoyale.Logic.Alliance.Stream;

    using Newtonsoft.Json;

    public class AllianceStreamEntries
    {
        [JsonProperty("seed")]      private int Seed;
        [JsonProperty("entries")]   private Dictionary<long, StreamEntry> Entries;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamEntries"/> class.
        /// </summary>
        public AllianceStreamEntries()
        {
            this.Entries    = new Dictionary<long, StreamEntry>(Config.MaxChatEntries);
        }

        /// <summary>
        /// Adds a new entry in the collection.
        /// </summary>
        public void AddEntry(StreamEntry Entry)
        {
            Entry.HighId 	= Config.ServerId;
            Entry.LowId 	= Interlocked.Increment(ref this.Seed);

            if (this.Entries.Count > Config.MaxChatEntries)
            {
                this.RemoveEntry(this.Entries.Values.First()); // TODO : Not wise, performance side
            }

            this.Entries.Add(Entry.StreamId, Entry);
        }

        /// <summary>
        /// Removes the specified entry of the collection.
        /// </summary>
        public void RemoveEntry(StreamEntry Entry)
        {
            if (!Entry.Removed)
            {
                if (this.Entries.Remove(Entry.StreamId))
                {
                    Entry.Removed = true;
                }
                else
                {
                    Logging.Warning(this.GetType(), "Tried to remove an entry from the list but Remove(StreamID) returned false.");
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "Tried to remove an entry that was already removed.");
            }
        }

        /// <summary>
        /// Gets the specified entry from the collection.
        /// </summary>
        public StreamEntry GetEntry(long StreamId)
        {
            if (this.Entries.TryGetValue(StreamId, out StreamEntry Entry))
            {
                return Entry;
            }

            return null;
        }

        /// <summary>
        /// Converts stream entry dictionary to stream entry array.
        /// </summary>
        public StreamEntry[] ToArray()
        {
            return this.Entries.Values.ToArray();
        }
    }
}