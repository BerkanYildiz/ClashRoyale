namespace ClashRoyale.Server.Logic.Items
{
    using System;

    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Files.Csv;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [JsonConverter(typeof(DataSlotConverter))]
    internal class DataSlot
    {
        internal CsvData Data;
        internal int Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSlot"/> class.
        /// </summary>
        internal DataSlot()
        {
            // DataSlot.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSlot"/> class.
        /// </summary>
        internal DataSlot(CsvData CsvData, int Count)
        {
            this.Data   = CsvData;
            this.Count  = Count;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal void Decode(ByteStream Stream)
        {
            this.Data   = Stream.DecodeData();
            this.Count  = Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void Encode(ChecksumEncoder Stream)
        {
            Stream.EncodeData(this.Data);
            Stream.WriteVInt(this.Count);
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        internal void Load(JToken Json)
        {
            JsonHelper.GetJsonData(Json, "id", out this.Data);
            JsonHelper.GetJsonNumber(Json, "cnt", out this.Count);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        internal JObject Save()
        {
            JObject Json = new JObject();

            if (this.Data != null)
            {
                Json.Add("id", this.Data.GlobalId);
            }

            Json.Add("cnt", this.Count);

            return Json;
        }
    }

    internal class DataSlotConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter Writer, object Value, JsonSerializer Serializer)
        {
            DataSlot DataSlot = (DataSlot) Value;

            if (DataSlot != null)
            {
                DataSlot.Save().WriteTo(Writer);
            }
            else
            {
                Writer.WriteValue(0);
            }
        }

        public override object ReadJson(JsonReader Reader, Type ObjectType, object ExistingValue, JsonSerializer Serializer)
        {
            DataSlot DataSlot = (DataSlot) ExistingValue;

            if (DataSlot == null)
            {
                DataSlot = new DataSlot();
            }

            DataSlot.Load(JObject.Load(Reader));

            return DataSlot;
        }

        public override bool CanConvert(Type ObjectType)
        {
            return ObjectType.BaseType == typeof(DataSlot) || ObjectType == typeof(DataSlot);
        }
    }
}