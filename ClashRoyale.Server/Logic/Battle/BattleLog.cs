namespace ClashRoyale.Server.Logic
{
    using System;

    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Files.Csv.Logic;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [JsonConverter(typeof(BattleLogConverter))]
    internal class BattleLog
    {
        internal int HighId;
        internal int LowId;

        internal BattleLogConfig GameConfig;
        internal BattleLogPlayer[] Players;
        internal ArenaData ArenaData;

        internal int ReplayVersion;

        internal bool Challenge;
        internal bool Survival;
        internal bool Tournament;
        internal bool FriendlyChallenge;

        internal string ReplayJson;

        /// <summary>
        /// Gets the battle identifier.
        /// </summary>
        internal long BattleId
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
        internal void LoadJson(JToken Json)
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
        internal JObject SaveJson(bool ServerSerialization = false)
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

    internal class BattleLogConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter Writer, object Value, JsonSerializer Serializer)
        {
            BattleLog Log = (BattleLog) Value;

            if (Value != null)
            {
                Log.SaveJson(true).WriteTo(Writer);
            }
            else
            {
                Writer.WriteNull();
            }
        }

        public override object ReadJson(JsonReader Reader, Type ObjectType, object ExistingValue, JsonSerializer Serializer)
        {
            BattleLog Log = (BattleLog) ExistingValue;

            if (Log == null)
            {
                Log = new BattleLog();
            }

            Log.LoadJson(JToken.Load(Reader));

            return Log;
        }

        public override bool CanConvert(Type ObjectType)
        {
            return ObjectType == typeof(BattleLog);
        }
    }
}