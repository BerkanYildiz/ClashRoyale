namespace ClashRoyale.Server.Logic.Apis
{
    using Newtonsoft.Json;

    internal class ApiManager
    {
        [JsonProperty("facebook")]      internal Facebook Facebook;
        [JsonProperty("gamecenter")]    internal Gamecenter Gamecenter;
        [JsonProperty("google")]        internal Google Google;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiManager"/> class.
        /// </summary>
        internal ApiManager()
        {
            this.Facebook   = new Facebook();
            this.Gamecenter = new Gamecenter();
            this.Google     = new Google();
        }
    }
}