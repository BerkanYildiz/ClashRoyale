namespace ClashRoyale.Server.Logic.Apis
{
    using Newtonsoft.Json;

    internal class Facebook
    {
        [JsonProperty("fbId")]    internal string Identifier;
        [JsonProperty("fbToken")] internal string Token;

        /// <summary>
        /// Initializes a new instance of the <see cref="Facebook"/> class.
        /// </summary>
        internal Facebook()
        {
            // Facebook.
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Facebook"/> is filled.
        /// </summary>
        internal bool Filled
        {
            get
            {
                return !string.IsNullOrEmpty(this.Identifier) && !string.IsNullOrEmpty(this.Token);
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        internal void Reset()
        {
            this.Identifier = null;
            this.Token      = null;
        }
    }
}