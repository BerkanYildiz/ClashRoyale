namespace ClashRoyale.Server.Logic.Apis
{
    using Newtonsoft.Json;

    internal class Google
    {
        [JsonProperty("ggId")]     internal string Identifier;
        [JsonProperty("ggToken")]  internal string Token;

        /// <summary>
        /// Initializes a new instance of the <see cref="Google"/> class.
        /// </summary>
        internal Google()
        {
            // Google.
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Google"/> is filled.
        /// </summary>
        internal bool Filled
        {
            get
            {
                return !string.IsNullOrEmpty(this.Identifier) || !string.IsNullOrEmpty(this.Token);
            }
        }
    }
}