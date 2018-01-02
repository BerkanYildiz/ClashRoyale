namespace ClashRoyale.Logic.Apis
{
    using Newtonsoft.Json;

    public class ApiManager
    {
        [JsonProperty("facebook")]      public Facebook Facebook;
        [JsonProperty("gamecenter")]    public Gamecenter Gamecenter;
        [JsonProperty("google")]        public Google Google;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiManager"/> class.
        /// </summary>
        public ApiManager()
        {
            this.Facebook   = new Facebook();
            this.Gamecenter = new Gamecenter();
            this.Google     = new Google();
        }
    }
}