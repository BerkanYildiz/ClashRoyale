namespace ClashRoyale.Logic.Apis
{
    using Newtonsoft.Json;

    public class Google
    {
        [JsonProperty("ggId")]     public string Identifier;
        [JsonProperty("ggToken")]  public string Token;

        /// <summary>
        /// Initializes a new instance of the <see cref="Google"/> class.
        /// </summary>
        public Google()
        {
            // Google.
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Google"/> is filled.
        /// </summary>
        public bool Filled
        {
            get
            {
                return !string.IsNullOrEmpty(this.Identifier) || !string.IsNullOrEmpty(this.Token);
            }
        }
    }
}