namespace ClashRoyale.Server.Logic.Alliance.Slots
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Server.Logic.Alliance.Stream;
    using ClashRoyale.Server.Logic.Player;
    using ClashRoyale.Server.Network.Packets.Server;

    using Newtonsoft.Json;

    internal class AllianceStreamEntries
    {
        private Clan Clan;

        [JsonProperty("s")]     private int Seed;
        [JsonProperty("slots")] private Dictionary<long, StreamEntry> Slots;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamEntries"/> class.
        /// </summary>
        internal AllianceStreamEntries()
        {
            this.Slots 		= new Dictionary<long, StreamEntry>(100);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamEntries"/> class.
        /// </summary>
        /// <param name="Clan">The alliance.</param>
        internal AllianceStreamEntries(Clan Clan) : this()
        {
            this.Clan 	= Clan;
        }

        /// <summary>
        /// Adds a new entry in the collection.
        /// </summary>
        internal void AddEntry(StreamEntry Entry)
        {
            Entry.HighId 	= Config.ServerId;
            Entry.LowId 	= Interlocked.Increment(ref this.Seed);

            if (this.Slots.Count > 100)
            {
                this.RemoveEntry(this.Slots.Values.First());
            }

            this.Slots.Add(Entry.StreamId, Entry);

            Task.Run(() =>
            {
                foreach (Player Player in this.Clan.Members.Connected.Values.ToArray())
                {
                    Player.GameMode.Device.NetworkManager.SendMessage(new AllianceStreamEntryMessage(Player.GameMode.Device, Entry));
                }
            });
        }

        /// <summary>
        /// Removes the specified entry of the collection.
        /// </summary>
        internal void RemoveEntry(StreamEntry Entry)
        {
            if (!Entry.Removed)
            {
                if (this.Slots.Remove(Entry.StreamId))
                {
                    Entry.Removed = true;

                    Task.Run(() =>
                    {
                        foreach (Player Player in this.Clan.Members.Connected.Values.ToArray())
                        {
                            Player.GameMode.Device.NetworkManager.SendMessage(new AllianceStreamRemovedMessage(Player.GameMode.Device, Entry.StreamId));
                        }
                    });
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
        /// Updates the specified entry.
        /// </summary>
        internal void UpdateEntry(StreamEntry Entry)
        {
            if (this.Slots.ContainsKey(Entry.StreamId))
            {
                if (!Entry.Removed)
                {
                    Task.Run(() =>
                    {
                        foreach (Player Player in this.Clan.Members.Connected.Values.ToArray())
                        {
                            Player.GameMode.Device.NetworkManager.SendMessage(new AllianceStreamEntryMessage(Player.GameMode.Device, Entry));
                        }
                    });
                }
                else
                {
                    Logging.Warning(this.GetType(), "Tried to update an entry that was removed.");
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "Tried to remove an entry that wasn't in the list.");
            }
        }

        /// <summary>
        /// Gets the specified entry from the collection.
        /// </summary>
        internal StreamEntry GetEntry(long StreamId)
        {
            if (this.Slots.TryGetValue(StreamId, out StreamEntry Entry))
            {
                return Entry;
            }

            return null;
        }

        /// <summary>
        /// Converts stream entry dictionary to stream entry array.
        /// </summary>
        internal StreamEntry[] ToArray()
        {
            return this.Slots.Values.ToArray();
        }
    }
}