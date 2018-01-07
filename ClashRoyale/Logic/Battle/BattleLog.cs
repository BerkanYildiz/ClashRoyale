namespace ClashRoyale.Logic.Battle
{
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Converters;
    using ClashRoyale.Logic.Replay;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [JsonConverter(typeof(BattleLogConverter))]
    public class BattleLog
    {
        public int HighId;
        public int LowId;

        public BattleLogConfig GameConfig;
        public BattleLogPlayer[] Players;
        public ArenaData ArenaData;

        public int ReplayVersion;

        public bool Challenge;
        public bool Survival;
        public bool Tournament;
        public bool FriendlyChallenge;

        public string ReplayJson;

        /// <summary>
        /// Gets the battle identifier.
        /// </summary>
        public long BattleId
        {
            get
            {
                return (long) this.HighId << 32 | (uint) this.LowId;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleLog"/> class.
        /// </summary>
        public BattleLog()
        {
            this.ReplayVersion  = 43;
            this.Players        = new BattleLogPlayer[4];
            this.GameConfig     = new BattleLogConfig();

            for (int I = 0; I < 4; I++)
            {
                this.Players[I] = new BattleLogPlayer();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleLog"/> class.
        /// </summary>
        public BattleLog(Battle Battle, Replay Replay) : this()
        {
            this.ArenaData = Battle.ArenaData;
            this.GameConfig.GameMode = Battle.GameModeData;

            for (int I = 0; I < 4; I++)
            {
                if (Battle.Players[I] != null)
                {
                    this.Players[I].SetPlayer(Battle.Players[I], 2);
                }
            }
            
            this.ReplayJson = Replay.Json.ToString(Formatting.None);
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        public void LoadJson(JToken Json)
        {
            JsonHelper.GetJsonNumber(Json, "highID", out this.HighId);
            JsonHelper.GetJsonNumber(Json, "lowID", out this.LowId);

            for (int I = 0; I < 4; I++)
            {
                if (JsonHelper.GetJsonObject(Json, "player" + I, out JToken Player))
                {
                    this.Players[I].LoadJson(Player);
                }
            }

            JsonHelper.GetJsonData(Json, "arena", out this.ArenaData);
            JsonHelper.GetJsonNumber(Json, "replayV", out this.ReplayVersion);
            JsonHelper.GetJsonBoolean(Json, "challenge", out this.Challenge);
            JsonHelper.GetJsonBoolean(Json, "tournament", out this.Tournament);
            JsonHelper.GetJsonBoolean(Json, "friendly_challenge", out this.FriendlyChallenge);

            if (JsonHelper.GetJsonObject(Json, "game_config", out JToken GameConfig))
            {
                this.GameConfig.LoadJson(GameConfig);
            }

            JsonHelper.GetJsonString(Json, "replayJSON", out this.ReplayJson);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        public JObject SaveJson(bool ServerSerialization = false)
        {
            JObject Json = new JObject();

            for (int I = 0; I < 4; I++)
            {
                Json.Add("player" + I, this.Players[I].Json);
            }

            JsonHelper.SetLogicData(Json, "arena", this.ArenaData);

            Json.Add("replayV", this.ReplayVersion);
            Json.Add("challenge", this.Challenge);
            Json.Add("tournament", this.Tournament);
            Json.Add("friendly_challenge", this.FriendlyChallenge);
            Json.Add("game_config", this.GameConfig.SaveJson());

            if (ServerSerialization)
            {
                Json.Add("highID", this.HighId);
                Json.Add("lowID", this.LowId);
                Json.Add("replayJSON", this.ReplayJson);
            }

            return Json;
        }
    }
}