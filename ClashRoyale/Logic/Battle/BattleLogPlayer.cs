namespace ClashRoyale.Logic.Battle
{
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Home.Spells;
    using ClashRoyale.Logic.Player;

    using Newtonsoft.Json.Linq;

    public class BattleLogPlayer
    {
        public int AccountHighId;
        public int AccountLowId;
        public int AllianceHighId;
        public int AllianceLowId;
        public int HomeHighId;
        public int HomeLowId;

        public string Name;
        public string AllianceName;
        public int Stars;
        public int Score;
        public int ScoreP;
        public int HighScore;

        public AllianceBadgeData BadgeData;
        public SpellDeck Deck;

        /// <summary>
        /// Gets the battle log json.
        /// </summary>
        public JObject Json
        {
            get
            {
                JObject Json = new JObject();

                Json.Add("acc_hi", this.AccountHighId);
                Json.Add("acc_lo", this.AccountLowId);
                Json.Add("all_hi", this.AllianceHighId);
                Json.Add("all_lo", this.AllianceLowId);
                Json.Add("home_hi", this.HomeHighId);
                Json.Add("home_lo", this.HomeLowId);

                if (this.Name != null)
                {
                    Json.Add("name", this.Name);
                }

                if (this.AllianceName != null)
                {
                    Json.Add("alliance", this.Name);
                }

                if (this.Stars != 0)
                {
                    Json.Add("stars", this.Stars);
                }

                if (this.Score != 0)
                {
                    Json.Add("score", this.Score);
                }

                if (this.ScoreP != 0)
                {
                    Json.Add("score_p", this.ScoreP);
                }

                if (this.HighScore != 0)
                {
                    Json.Add("highscore", this.HighScore);
                }

                JsonHelper.SetLogicData(Json, "badge", this.BadgeData);

                if (this.Deck != null)
                {
                    Json.Add("spells", this.Deck.Save());
                }

                return Json;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleLogPlayer"/> class.
        /// </summary>
        public BattleLogPlayer()
        {
            // BattleLogPlayer.
        }

        /// <summary>
        /// Sets the player.
        /// </summary>
        public void SetPlayer(Player Player, int Stars)
        {
            this.Stars = Stars;

            this.AccountHighId = Player.HighId;
            this.AccountLowId = Player.LowId;
            this.AllianceHighId = Player.ClanHighId;
            this.AllianceLowId = Player.ClanLowId;
            this.HomeHighId = Player.Home.HighId;
            this.HomeLowId = Player.Home.LowId;

            this.Name = Player.Name;
            this.AllianceName = Player.AllianceName;

            this.Score = Player.Score;
            this.ScoreP = Player.MaxScore;

            this.BadgeData = Player.Badge;

            this.Deck = Player.Home.SpellDeck.Clone();
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        public void LoadJson(JToken Json)
        {
            JsonHelper.GetJsonNumber(Json, "acc_hi", out this.AccountHighId);
            JsonHelper.GetJsonNumber(Json, "acc_lo", out this.AccountLowId);
            JsonHelper.GetJsonNumber(Json, "alli_hi", out this.AllianceHighId);
            JsonHelper.GetJsonNumber(Json, "alli_lo", out this.AllianceLowId);
            JsonHelper.GetJsonNumber(Json, "home_hi", out this.HomeHighId);
            JsonHelper.GetJsonNumber(Json, "home_lo", out this.HomeLowId);

            JsonHelper.GetJsonString(Json, "name", out this.Name);
            JsonHelper.GetJsonString(Json, "alliance", out this.AllianceName);

            JsonHelper.GetJsonNumber(Json, "score", out this.Score);
            JsonHelper.GetJsonNumber(Json, "score_p", out this.ScoreP);
            JsonHelper.GetJsonNumber(Json, "stars", out this.Stars);
            JsonHelper.GetJsonNumber(Json, "highscore", out this.HighScore);

            JsonHelper.GetJsonData(Json, "badge", out this.BadgeData);

            if (JsonHelper.GetJsonArray(Json, "spells", out JArray Spells))
            {
                this.Deck = new SpellDeck();
                this.Deck.Load(Spells);
            }
        }
    }
}