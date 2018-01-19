namespace ClashRoyale.Logic.Home
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Commands.Server;
    using ClashRoyale.Logic.Converters;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Reward;
    using ClashRoyale.Logic.Time;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [JsonConverter(typeof(ChestConverter))]
    public class Chest
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
        public bool CanStartUnlocking
        {
            get
            {
                return this.UnlockTimer == null && !this.Unlocked;
            }
        }

        /// <summary>
        /// Gets if the chest is new.
        /// </summary>
        public bool IsNew
        {
            get
            {
                return this.New;
            }
        }

        /// <summary>
        /// Gets if the chest is unlocked.
        /// </summary>
        public bool IsUnlocked
        {
            get
            {
                return this.Unlocked;
            }
        }

        /// <summary>
        /// Gets if the chest is unlocking.
        /// </summary>
        public bool IsUnlocking
        {
            get
            {
                return this.UnlockTimer != null;
            }
        }

        /// <summary>
        /// Gets the unlocking seconds left.
        /// </summary>
        public int UnlockingSecondsLeft
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
        public TreasureChestData Data
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
        public void Decode(ByteStream Reader)
        {
            this.ChestData  = Reader.DecodeData<TreasureChestData>();
            this.Unlocked   = Reader.ReadBoolean();
            this.Claimed    = Reader.ReadBoolean();
            this.New        = Reader.ReadBoolean();

            if (Reader.ReadBoolean())
            {
                this.UnlockTimer = new Timer();
                this.UnlockTimer.Decode(Reader);
            }

            Reader.ReadVInt();

            this.Source     = Reader.ReadVInt();
            this.SlotIndex  = Reader.ReadVInt();

            Reader.ReadVInt();
            Reader.ReadVInt();

            Reader.ReadBoolean();
            Reader.ReadBoolean();
            Reader.ReadBoolean();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public void Encode(ByteStream Stream)
        {
            Stream.EncodeData(this.ChestData);
            Stream.WriteBoolean(this.Unlocked);
            Stream.WriteBoolean(this.Claimed);
            Stream.WriteBoolean(this.New);

            Stream.WriteBoolean(this.UnlockTimer != null);

            if (this.UnlockTimer != null)
            {
                this.UnlockTimer.Encode(Stream);
            }

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
        public void FastForward(int Seconds)
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
        public void SetNew(bool Value)
        {
            this.New = Value;
        }

        /// <summary>
        /// Sets the source.
        /// </summary>
        public void SetSource(int Value)
        {
            this.Source = Value;
        }

        /// <summary>
        /// Speeds up the unlocking.
        /// </summary>
        public void SpeedUpUnlocking()
        {
            this.UnlockDone();
        }

        /// <summary>
        /// Starts the unlocking.
        /// </summary>
        public void StartUnlocking()
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
        public void SetClaimed(GameMode GameMode, int ChestType)
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
        public void Tick()
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
        public void UnlockDone()
        {
            this.Unlocked    = true;
            this.UnlockTimer = null;
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        public void Load(JObject Json)
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
        public JObject Save()
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