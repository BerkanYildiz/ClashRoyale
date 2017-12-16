namespace ClashRoyale.Server.Logic
{
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Files.Csv.Logic;

    using Newtonsoft.Json.Linq;

    internal class BattleLogConfig
    {
        internal GameModeData GameMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleLogConfig"/> class.
        /// </summary>
        public BattleLogConfig()
        {
            // BattleLogConfig.
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        internal void LoadJson(JToken Json)
        {
            JsonHelper.GetJsonData(Json, "arena", out this.GameMode);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        internal JObject SaveJson()
        {
            JObject Json = new JObject();

            Json.Add("gmt", 1);
            Json.Add("plt", 1);

            JsonHelper.SetLogicData(Json, "gamemode", this.GameMode);

            Json.Add("t1s", 0);
            Json.Add("t2s", 0);

            return Json;
        }
    }
}