namespace ClashRoyale.Logic.Alliance.Entries
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Client;
    using ClashRoyale.Files.Csv.Logic;

    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class AllianceHeaderEntry
    {
        [JsonProperty("highId")]        public int HighId;
        [JsonProperty("lowId")]         public int LowId;

        [JsonProperty("name")]          public string Name;

        [JsonProperty("type")] 			public int Type;
        [JsonProperty("score")]         public int Score;
        [JsonProperty("requiredScore")] public int RequiredScore;
        [JsonProperty("members")]       public int MembersCount;
        [JsonProperty("donations")]     public int Donations;

        [JsonProperty("region")]        public RegionData Region;
        [JsonProperty("locale")]        public LocaleData Locale;
        [JsonProperty("badge")]         public AllianceBadgeData Badge;

        public long ClanId
        {
            get
            {
                return (long) (uint) this.HighId << 32 | (uint) this.LowId;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceHeaderEntry"/> class.
        /// </summary>
        public AllianceHeaderEntry()
        {
            // AllianceHeaderEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceHeaderEntry"/> class.
        /// </summary>
        public AllianceHeaderEntry(int HighId, int LowId)
        {
            this.HighId 	    = HighId;
            this.LowId 	        = LowId;
        }

        /// <summary>
        /// Sets the alliance.
        /// </summary>
        /// <param name="HighId">The high identifier.</param>
        /// <param name="LowId">The low identifier.</param>
        /// <param name="MembersCount">The members count.</param>
        public void SetAlliance(int HighId, int LowId, int MembersCount)
        {
            this.HighId         = HighId;
            this.LowId          = LowId;
            this.MembersCount   = MembersCount;
        }

        /// <summary>
        /// Decodes from the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public void Decode(ByteStream Stream)
        {
            this.HighId         = Stream.ReadInt();
            this.LowId          = Stream.ReadInt();

            this.Name           = Stream.ReadString();
            this.Badge          = Stream.DecodeData<AllianceBadgeData>();

            this.Type           = Stream.ReadVInt();
            this.MembersCount   = Stream.ReadVInt();
            this.Score          = Stream.ReadVInt();
            this.RequiredScore  = Stream.ReadVInt();

            Stream.ReadVInt();
            Stream.ReadVInt();
            Stream.ReadVInt();
            Stream.ReadVInt();

            this.Donations      = Stream.ReadVInt();

            Stream.ReadVInt();

            this.Locale         = Stream.DecodeData<LocaleData>();
            this.Region         = Stream.DecodeData<RegionData>();

            bool Event          = Stream.ReadBoolean();

            if (Event)
            {
                // TODO : Decode the clan event.
            }
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteInt(this.HighId);
            Stream.WriteInt(this.LowId);
            Stream.WriteString(this.Name);
            Stream.EncodeData(this.Badge);

            Stream.WriteVInt(this.Type);
            Stream.WriteVInt(this.MembersCount);
            Stream.WriteVInt(this.Score);
            Stream.WriteVInt(this.RequiredScore);

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(50);
            Stream.WriteVInt(this.Donations);
            Stream.WriteVInt(2);

            Stream.EncodeData(this.Locale);
            Stream.EncodeData(this.Region);

            Stream.WriteBoolean(false);

            if (false)
            {
                // TODO : Encode the clan event.
            }
        }
    }
}