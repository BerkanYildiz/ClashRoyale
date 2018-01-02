namespace ClashRoyale.Logic.Battle
{
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;

    using Newtonsoft.Json.Linq;

    public class BattleLogConfig
    {
        public GameModeData GameMode;

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
        public void LoadJson(JToken Json)
        {
            JsonHelper.GetJsonData(Json, "arena", out this.GameMode);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        public JObject SaveJson()
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