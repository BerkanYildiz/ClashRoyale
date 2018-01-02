namespace ClashRoyale.Logic.Apis
{
    using Newtonsoft.Json;

    public class Gamecenter
    {
        [JsonProperty("gcId")]      public string Identifier;
        [JsonProperty("gcCert")]    public string Certificate;
        [JsonProperty("gcBundle")]  public string AppBundle;

        /// <summary>
        /// Initializes a new instance of the <see cref="Gamecenter"/> class.
        /// </summary>
        public Gamecenter()
        {
            // Gamecenter.
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Gamecenter"/> is filled.
        /// </summary>
        public bool Filled
        {
            get
            {
                return !string.IsNullOrEmpty(this.Identifier) && !string.IsNullOrEmpty(this.Certificate) && !string.IsNullOrEmpty(this.AppBundle);
            }
        }
    }
}