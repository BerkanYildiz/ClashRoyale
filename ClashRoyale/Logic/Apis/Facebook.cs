namespace ClashRoyale.Logic.Apis
{
    using Newtonsoft.Json;

    public class Facebook
    {
        [JsonProperty("fbId")]    public string Identifier;
        [JsonProperty("fbToken")] public string Token;

        /// <summary>
        /// Initializes a new instance of the <see cref="Facebook"/> class.
        /// </summary>
        public Facebook()
        {
            // Facebook.
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Facebook"/> is filled.
        /// </summary>
        public bool Filled
        {
            get
            {
                return !string.IsNullOrEmpty(this.Identifier) && !string.IsNullOrEmpty(this.Token);
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            this.Identifier = null;
            this.Token      = null;
        }
    }
}