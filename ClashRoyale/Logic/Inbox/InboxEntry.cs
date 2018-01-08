namespace ClashRoyale.Logic.Inbox
{
    using System;

    using ClashRoyale.Extensions;

    using Newtonsoft.Json;

    public class InboxEntry
    {
        [JsonProperty("highId")]        public int HighId;
        [JsonProperty("lowId")]         public int LowId;

        [JsonProperty("image")]         public string Image       = "https://56f230c6d142ad8a925f-b174a1d8fb2cf6907e1c742c46071d76.ssl.cf2.rackcdn.com/inbox/ClashRoyale_logo_small.png";
        [JsonProperty("title")]         public string Title;
        [JsonProperty("message")]       public string Text;
        [JsonProperty("button")]        public string ButtonText  = "OK";
        [JsonProperty("link")]          public string Url         = "https://www.gobelinland.fr/";
        [JsonProperty("assetPath")]     public string AssetPath   = "http://<asset_path_update>";

        [JsonProperty("date")]          public DateTime Date;

        /// <summary>
        /// Gets the entry identifier.
        /// </summary>
        public long EntryId
        {
            get
            {
                return (long) this.HighId << 32 | (uint) this.LowId;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="InboxEntry"/> is outdated.
        /// </summary>
        public bool Outdated
        {
            get
            {
                return this.Date.AddDays(7) < DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="InboxEntry"/> is filled.
        /// </summary>
        public bool Filled
        {
            get
            {
                return !string.IsNullOrEmpty(this.Title) && !string.IsNullOrEmpty(this.Text);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboxEntry"/> class.
        /// </summary>
        public InboxEntry()
        {
            this.Date = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboxEntry"/> class.
        /// </summary>
        /// <param name="HighId">The high identifier.</param>
        /// <param name="LowId">The low identifier.</param>
        public InboxEntry(int HighId, int LowId)
        {
            this.HighId = HighId;
            this.LowId  = LowId;
        }

        /// <summary>
        /// Decodes the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public void Decode(ByteStream Stream)
        {
            this.Image      = Stream.ReadString();
            this.Title      = Stream.ReadString();
            this.Text       = Stream.ReadString();
            this.ButtonText = Stream.ReadString();
            this.Url        = Stream.ReadString();

            Stream.ReadString();
            Stream.ReadString();

            this.AssetPath  = Stream.ReadString();
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteString(this.Image);
            Stream.WriteString(this.Title);
            Stream.WriteString(this.Text);
            Stream.WriteString(this.ButtonText);
            Stream.WriteString(this.Url);
            Stream.WriteString(string.Empty);
            Stream.WriteString(string.Empty);
            Stream.WriteString(this.AssetPath);
        }
    }
}