namespace ClashRoyale.Logic.Alliance.Entries
{
    using System.Linq;

    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Client;
    using ClashRoyale.Files.Csv.Logic;

    using Newtonsoft.Json;

    public class AllianceHeaderEntry
    {
        private Clan Clan;

        [JsonProperty("highId")]        private int HighId;
        [JsonProperty("lowId")]         private int LowId;

        [JsonProperty("name")]          public string Name;

        [JsonProperty("type")] 			public int Type;
        [JsonProperty("required_score")]public int RequiredScore;

        [JsonProperty("region")]        public RegionData Region;
        [JsonProperty("locale")]        public LocaleData Locale;
        [JsonProperty("badge")]         public AllianceBadgeData Badge;

        [JsonProperty("members")]       public int NumberOfMembers
        {
            get
            {
                return this.Clan.Members.Count;
            }
        }

        [JsonProperty("score")]         public int Score
        {
            get
            {
                int Score      = 0;
                var Entries    = this.Clan.Members.Values.ToArray();

                for (int I = 0; I < Entries.Length; I++)
                {
                    Score += Entries[I].Score;
                }

                return Score / 2;
            }
        }

        [JsonProperty("donations")]     public int Donations
        {
            get
            {
                int Donations  = 0;
                var Entries    = this.Clan.Members.Values.ToArray();

                for (int I = 0; I < Entries.Length; I++)
                {
                    Donations += Entries[I].Donations;
                }

                return Donations;
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
        public AllianceHeaderEntry(Clan Clan)
        {
            this.Clan   = Clan;
            this.HighId 	= Clan.HighId;
            this.LowId 	    = Clan.LowId;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public void Encode(ByteStream Stream)
        {
            Stream.WriteLong(this.Clan.AllianceId);
            Stream.WriteString(this.Name);
            Stream.EncodeData(this.Badge);

            Stream.WriteVInt(this.Type);
            Stream.WriteVInt(this.NumberOfMembers);
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
        }

        /// <summary>
        /// Sets the alliance.
        /// </summary>
        public void SetAlliance(Clan Clan)
        {
            this.Clan   = Clan;
            this.HighId     = Clan.HighId;
            this.LowId      = Clan.LowId;
        }
    }
}