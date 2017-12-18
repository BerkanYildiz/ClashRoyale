namespace ClashRoyale.Server.Logic.Alliance
{
    using System.Linq;

    using ClashRoyale.Extensions;
    using ClashRoyale.Server.Logic.Alliance.Entries;
    using ClashRoyale.Server.Logic.Alliance.Slots;

    using Newtonsoft.Json;

    internal class Clan
    {
        [JsonProperty("highId")] 	    internal int HighId;
        [JsonProperty("lowId")] 	    internal int LowId;

        [JsonProperty("description")] 	internal string Description;

        [JsonProperty("header")] 		internal AllianceHeaderEntry HeaderEntry;     
        [JsonProperty("members")] 		internal AllianceMemberEntries Members;
        [JsonProperty("messages")] 		internal AllianceStreamEntries Messages;

        /// <summary>
        /// Gets the alliance id.
        /// </summary>
        internal long AllianceId
        {
            get
            {
                return (long) this.HighId << 32 | (uint) this.LowId;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Clan"/> class.
        /// </summary>
        internal Clan()
        {
            this.HeaderEntry	= new AllianceHeaderEntry(this);
            this.Members    	= new AllianceMemberEntries(this);
            this.Messages   	= new AllianceStreamEntries(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Clan"/> class.
        /// </summary>
        /// <param name="HighId">The high identifier.</param>
        /// <param name="LowId">The low identifier.</param>
        internal Clan(int HighId, int LowId) : this()
        {
            this.HighId = HighId;
            this.LowId 	= LowId;

            this.HeaderEntry.SetAlliance(this);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void Encode(ByteStream Stream)
        {
            this.HeaderEntry.Encode(Stream);
			
            Stream.WriteString(this.Description);

            AllianceMemberEntry[] Entries = this.Members.Values.ToArray();

            Stream.WriteVInt(Entries.Length);

            for (int I = 0; I < Entries.Length; I++)
            {
                Entries[I].Encode(Stream);
            }

            Stream.WriteBoolean(false);
        }

        /// <summary>
        /// Called when this alliance has finished being loaded.
        /// </summary>
        internal void LoadingFinished()
        {
            // LoadingFinished.
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.HighId + "-" + this.LowId;
        }
    }
}