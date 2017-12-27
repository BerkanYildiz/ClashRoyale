namespace ClashRoyale.Server.Logic.Home
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.Commands.Server;
    using ClashRoyale.Server.Logic.Converters;
    using ClashRoyale.Server.Logic.Mode;
    using ClashRoyale.Server.Logic.Reward;
    using ClashRoyale.Server.Logic.Time;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [JsonConverter(typeof(ChestConverter))]
    internal class Chest
    {
        private bool New;
        private bool Claimed;
        private bool Unlocked;

        private int Source;
        private int SlotIndex;

        private Reward Reward;
        private Timer UnlockTimer;
        private TreasureChestData ChestData;

        /// <summary>
        /// Gets if we can start unlocking.
        /// </summary>
        internal bool CanStartUnlocking
        {
            get
            {
                return this.UnlockTimer == null && !this.Unlocked;
            }
        }

        /// <summary>
        /// Gets if the chest is new.
        /// </summary>
        internal bool IsNew
        {
            get
            {
                return this.New;
            }
        }

        /// <summary>
        /// Gets if the chest is unlocked.
        /// </summary>
        internal bool IsUnlocked
        {
            get
            {
                return this.Unlocked;
            }
        }

        /// <summary>
        /// Gets if the chest is unlocking.
        /// </summary>
        internal bool IsUnlocking
        {
            get
            {
                return this.UnlockTimer != null;
            }
        }

        /// <summary>
        /// Gets the unlocking seconds left.
        /// </summary>
        internal int UnlockingSecondsLeft
        {
            get
            {
                if (this.UnlockTimer != null)
                {
                    return this.UnlockTimer.RemainingSeconds;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets the data of this chest.
        /// </summary>
        internal TreasureChestData Data
        {
            get
            {
                return this.ChestData;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Chest"/> class.
        /// </summary>
        public Chest()
        {
            // Chest.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Chest"/> class.
        /// </summary>
        public Chest(TreasureChestData Data)
        {
            this.ChestData = Data;

            if (Data.TotalTimeTakenSeconds <= 0)
            {
                this.Unlocked = true;
            }
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal void Decode(ByteStream Reader)
        {
            this.ChestData = Reader.DecodeData<TreasureChestData>();
            this.Unlocked = Reader.ReadBoolean();
            this.Claimed = Reader.ReadBoolean();
            this.New = Reader.ReadBoolean();

            if (Reader.ReadBoolean())
            {
                this.UnlockTimer = new Timer();
                this.UnlockTimer.Decode(Reader);
            }

            Reader.ReadVInt();
            this.Source = Reader.ReadVInt();
            this.SlotIndex = Reader.ReadVInt();

            Reader.ReadVInt();
            Reader.ReadVInt();

            Reader.ReadBoolean();
            Reader.ReadBoolean();
            Reader.ReadBoolean();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void Encode(ByteStream Stream)
        {
            Stream.EncodeData(this.ChestData);
            Stream.WriteBoolean(this.Unlocked);
            Stream.WriteBoolean(this.Claimed);
            Stream.WriteBoolean(this.New);

            if (this.UnlockTimer != null)
            {
                Stream.WriteBoolean(true);
                this.UnlockTimer.Encode(Stream);
            }
            else 
                Stream.WriteBoolean(false);

            Stream.WriteVInt(0);
            Stream.WriteVInt(this.Source);
            Stream.WriteVInt(this.SlotIndex);

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);
        }

        /// <summary>
        /// Creates a fast forward of time.
        /// </summary>
        internal void FastForward(int Seconds)
        {
            if (this.UnlockTimer != null)
            {
                this.UnlockTimer.FastForward(Seconds);

                if (this.UnlockTimer.IsFinished)
                {
                    this.UnlockDone();
                }
            }   
        }

        /// <summary>
        /// Sets if the chest is new.
        /// </summary>
        internal void SetNew(bool Value)
        {
            this.New = Value;
        }

        /// <summary>
        /// Sets the source.
        /// </summary>
        internal void SetSource(int Value)
        {
            this.Source = Value;
        }

        /// <summary>
        /// Speeds up the unlocking.
        /// </summary>
        internal void SpeedUpUnlocking()
        {
            this.UnlockDone();
        }

        /// <summary>
        /// Starts the unlocking.
        /// </summary>
        internal void StartUnlocking()
        {
            if (!this.CanStartUnlocking)
            {
                Logging.Error(this.GetType(), "StartUnlocking() - CanStartUnlocking == false");
                return;
            }

            this.UnlockTimer = new Timer();
            this.UnlockTimer.StartTimer(this.ChestData.TotalTimeTakenSeconds);
        }

        /// <summary>
        /// Sets claimed.
        /// </summary>
        internal void SetClaimed(GameMode GameMode, int ChestType)
        {
            this.Claimed = true;

            if (GameMode != null)
            {
                GameMode.Home.SetClaimingReward(true);
                GameMode.CommandManager.AddAvailableServerCommand(new ClaimRewardCommand(RewardRandomizer.RandomizeReward(this, GameMode.Home), ChestType, 14));
            }
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        internal void Tick()
        {
            if (this.UnlockTimer != null)
            {
                this.UnlockTimer.Tick();

                if (this.UnlockTimer.IsFinished)
                {
                    this.UnlockDone();
                }
            }
        }

        /// <summary>
        /// Called when the unlock is done.
        /// </summary>
        internal void UnlockDone()
        {
            this.Unlocked = true;
            this.UnlockTimer = null;
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        internal void Load(JObject Json)
        {
            JsonHelper.GetJsonBoolean(Json, "x", out this.Unlocked);
            JsonHelper.GetJsonNumber(Json, "s", out this.Source);
            JsonHelper.GetJsonBoolean(Json, "n", out this.Unlocked);
            JsonHelper.GetJsonNumber(Json, "slot", out this.SlotIndex);
            JsonHelper.GetJsonData(Json, "d", out this.ChestData);

            if (JsonHelper.GetJsonObject(Json, "t", out JToken JTimer))
            {
                this.UnlockTimer = new Timer();
                this.UnlockTimer.Load(JTimer);
            }
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        internal JObject Save()
        {
            JObject Json = new JObject();

            Json.Add("x", this.Unlocked);
            Json.Add("s", this.Source);
            Json.Add("n", this.New);
            Json.Add("slot", this.SlotIndex);
            JsonHelper.SetLogicData(Json, "d", this.ChestData);

            if (this.UnlockTimer != null)
            {
                Json.Add("t", this.UnlockTimer.Save());
            }

            if (this.Reward != null)
            {
                Json.Add("r", this.Reward.Save());
            }

            return Json;
        }
    }
}