namespace ClashRoyale.Server.Logic.Apis
{
    using Newtonsoft.Json;

    internal class Gamecenter
    {
        [JsonProperty("gcId")]      internal string Identifier;
        [JsonProperty("gcCert")]    internal string Certificate;
        [JsonProperty("gcBundle")]  internal string AppBundle;

        /// <summary>
        /// Initializes a new instance of the <see cref="Gamecenter"/> class.
        /// </summary>
        internal Gamecenter()
        {
            // Gamecenter.
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Gamecenter"/> is filled.
        /// </summary>
        internal bool Filled
        {
            get
            {
                return !string.IsNullOrEmpty(this.Identifier) && !string.IsNullOrEmpty(this.Certificate) && !string.IsNullOrEmpty(this.AppBundle);
            }
        }
    }
}