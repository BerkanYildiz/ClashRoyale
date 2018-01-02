namespace ClashRoyale.Logic.Battle.Event
{
    using System.Collections.Generic;

    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;

    using Newtonsoft.Json.Linq;

    public class BattleEvent
    {
        public int HighId;
        public int LowId;

        public byte Type;

        public List<int> Ticks;
        public List<int> Coords;
        public List<int> Params;

        /// <summary>
        /// Gets the sender id.
        /// </summary>
        public long SenderId
        {
            get
            {
                return (long) this.HighId << 32 | (uint) this.LowId;
            }
        }

        /// <summary>
        /// Gets the num of coords.
        /// </summary>
        public int NumCoords
        {
            get
            {
                return this.Coords.Count;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleEvent"/> class.
        /// </summary>
        public BattleEvent()
        {
            this.Ticks 	= new List<int>(4);
            this.Coords = new List<int>(8);
            this.Params = new List<int>(4);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleEvent"/> class.
        /// </summary>
        public BattleEvent(byte Type)
        {
            this.Type 	= Type;
        }

        /// <summary>
        /// Gets the coord x of event.
        /// </summary>
        public int GetCoordX(int Value)
        {
            return this.Coords[Value];
        }

        /// <summary>
        /// Gets the coord y of event.
        /// </summary>
        public int GetCoordY(int Value)
        {
            return this.Coords[Value | 1];
        }

        /// <summary>
        /// Sets the tick.
        /// </summary>
        public void SetTick(int Tick)
        {
            if (this.Ticks.Count > 0)
            {
                Logging.Info(this.GetType(), "Replay event: setting tick will clear old ticks and coords.");
                return;
            }

            this.Ticks.Add(Tick);
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public void Decode(ByteStream Stream)
        {
            this.Type 	= Stream.ReadByte();
            this.HighId = Stream.ReadVInt();
            this.LowId 	= Stream.ReadVInt();

            Stream.DecodeIntList(ref this.Ticks);
            Stream.DecodeIntList(ref this.Coords);
            Stream.DecodeIntList(ref this.Params);

            if (this.Type == 7)
            {
                Logging.Info(this.GetType(), "Unknown Value: " + Stream.ReadByte());
            }

            if (this.Type == 8)
            {
                Logging.Info(this.GetType(), "Unknown Boolean: " + Stream.ReadByte());
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public void Encode(ByteStream Stream)
        {
            Stream.WriteByte(this.Type);
            Stream.WriteLogicLong(this.HighId, this.LowId);
            Stream.EncodeIntList(this.Ticks);
            Stream.EncodeIntList(this.Coords);
            Stream.EncodeIntList(this.Params);

            if (this.Type == 7)
            {
                // ?
            }

            if (this.Type == 8)
            {
                Stream.WriteBoolean(false);
            }
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        public JObject Save()
        {
            JObject Json = new JObject();

            Json.Add("type", this.Type);
            Json.Add("id_hi", this.HighId);
            Json.Add("id_lo", this.LowId);

            JsonHelper.SetIntArray(Json, "ticks", this.Ticks.ToArray());

            if (this.Coords.Count > 0)
            {
                JsonHelper.SetIntArray(Json, "coords", this.Params.ToArray());
            }

            JsonHelper.SetIntArray(Json, "params", this.Params.ToArray());

            return Json;
        }
    }
}