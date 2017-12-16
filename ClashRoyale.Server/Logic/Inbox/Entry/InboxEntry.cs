namespace ClashRoyale.Server.Logic.Entry
{
    using System;

    using ClashRoyale.Server.Extensions;

    using Newtonsoft.Json;

    internal class InboxEntry
    {
        [JsonProperty("highId")]        internal int HighId;
        [JsonProperty("lowId")]         internal int LowId;

        [JsonProperty("image")]         internal string Image       = "https://56f230c6d142ad8a925f-b174a1d8fb2cf6907e1c742c46071d76.ssl.cf2.rackcdn.com/inbox/ClashRoyale_logo_small.png";
        [JsonProperty("title")]         internal string Title;
        [JsonProperty("message")]       internal string Text;
        [JsonProperty("button")]        internal string ButtonText  = "OK";
        [JsonProperty("link")]          internal string Url         = "https://www.gobelinland.fr/";
        [JsonProperty("assetPath")]     internal string AssetPath   = "http://<asset_path_update>";

        [JsonProperty("date")]          internal DateTime Date;

        /// <summary>
        /// Gets the entry identifier.
        /// </summary>
        internal long EntryId
        {
            get
            {
                return (long) this.HighId << 32 | (uint) this.LowId;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="InboxEntry"/> is outdated.
        /// </summary>
        internal bool Outdated
        {
            get
            {
                return this.Date.AddDays(7) < DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="InboxEntry"/> is filled.
        /// </summary>
        internal bool Filled
        {
            get
            {
                return !string.IsNullOrEmpty(this.Title) && !string.IsNullOrEmpty(this.Text);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboxEntry"/> class.
        /// </summary>
        internal InboxEntry()
        {
            this.Date = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboxEntry"/> class.
        /// </summary>
        /// <param name="HighId">The high identifier.</param>
        /// <param name="LowId">The low identifier.</param>
        internal InboxEntry(int HighId, int LowId)
        {
            this.HighId = HighId;
            this.LowId  = LowId;
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal void Encode(ByteStream Stream)
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