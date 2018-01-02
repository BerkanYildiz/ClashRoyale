namespace ClashRoyale.Logic.Time
{
    using System;

    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Extensions.Utils;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Math = ClashRoyale.Maths.Math;

    [JsonConverter(typeof(TimerConverter))]
    public class Timer
    {
        private int Remainings;
        private int TotalTicks;
        private int EndTimestamp;

        /// <summary>
        /// Gets if this timer is finished.
        /// </summary>
        public bool IsFinished
        {
            get
            {
                return this.Remainings <= 0;
            }
        }

        /// <summary>
        /// Gets the remaining time in seconds.
        /// </summary>
        public int RemainingSeconds
        {
            get
            {
                if (this.Remainings > 0)
                {
                    return Math.Max((this.Remainings + 19) / 20, 1);
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets the remaining time in ticks.
        /// </summary>
        public int RemainingTicks
        {
            get
            {
                return this.Remainings;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        public Timer()
        {
            this.EndTimestamp = -1;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void StartTimer(int Seconds)
        {
            this.TotalTicks = Time.GetSecondsInTicks(Seconds);
            this.Remainings = this.TotalTicks;

            if (this.EndTimestamp == -1)
            {
                this.EndTimestamp = TimeUtil.Timestamp + Seconds;
            }
        }

        /// <summary>
        /// Adjust the end subtick.
        /// </summary>
        public void AdjustEndSubTick(int Ticks)
        {
            this.Remainings -= Ticks;

            if (this.Remainings < 0)
            {
                this.Remainings = 0;
            }
        }

        /// <summary>
        /// Adjust the end subtick.
        /// </summary>
        public void AdjustEndSubTick(Time Time)
        {
            this.Remainings -= Time;

            if (this.Remainings < 0)
            {
                this.Remainings = 0;
            }
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public void Decode(ByteStream Stream)
        {
            this.Remainings = Stream.ReadVInt();
            this.TotalTicks = Stream.ReadVInt();
            this.EndTimestamp = Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteVInt(this.Remainings);
            Stream.WriteVInt(this.TotalTicks);
            Stream.WriteVInt(this.EndTimestamp);
        }

        /// <summary>
        /// Creates a fast forward of time.
        /// </summary>
        public void FastForward(int Seconds)
        {
            if (this.Remainings > 0)
            {
                this.AdjustEndSubTick(Time.GetSecondsInTicks(Seconds));
            }
        }

        /// <summary>
        /// Resets the timer.
        /// </summary>
        public void Reset()
        {
            this.TotalTicks = 0;
            this.Remainings = 0;
            this.EndTimestamp = -1;
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        public void Tick()
        {
            if (this.Remainings > 0)
            {
                --this.Remainings;

                if (this.Remainings < 0)
                {
                    this.Remainings = 0;
                }
            }
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        public void Load(JToken Json)
        {
            JsonHelper.GetJsonNumber(Json, "ticks", out this.TotalTicks);
            JsonHelper.GetJsonNumber(Json, "remaining", out this.Remainings);
            JsonHelper.GetJsonNumber(Json, "timestamp", out this.EndTimestamp);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        public JObject Save()
        {
            JObject Json = new JObject();

            Json.Add("ticks", this.TotalTicks);
            Json.Add("remaining", this.Remainings);
            Json.Add("timestamp", this.EndTimestamp);

            return Json;
        }
    }

    public class TimerConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter Writer, object Value, JsonSerializer Serializer)
        {
            Timer Timer = (Timer) Value;

            if (Timer != null)
            {
                Timer.Save().WriteTo(Writer);
            }
        }

        public override object ReadJson(JsonReader Reader, Type ObjectType, object ExistingValue, JsonSerializer Serializer)
        {
            Timer Deck = (Timer) ExistingValue;

            if (Deck == null)
            {
                Deck = new Timer();
            }

            Deck.Load(JObject.Load(Reader));

            return Deck;
        }

        public override bool CanConvert(Type ObjectType)
        {
            return ObjectType == typeof(Timer);
        }
    }
}