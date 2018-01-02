namespace ClashRoyale.Logic.Player.Items
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv;
    using ClashRoyale.Logic.Converters;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [JsonConverter(typeof(DataSlotConverter))]
    public class DataSlot
    {
        public CsvData Data;
        public int Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSlot"/> class.
        /// </summary>
        public DataSlot()
        {
            // DataSlot.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSlot"/> class.
        /// </summary>
        public DataSlot(CsvData CsvData, int Count)
        {
            this.Data   = CsvData;
            this.Count  = Count;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public void Decode(ByteStream Stream)
        {
            this.Data   = Stream.DecodeData();
            this.Count  = Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public void Encode(ChecksumEncoder Stream)
        {
            Stream.EncodeData(this.Data);
            Stream.WriteVInt(this.Count);
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        public void Load(JToken Json)
        {
            JsonHelper.GetJsonData(Json, "id", out this.Data);
            JsonHelper.GetJsonNumber(Json, "cnt", out this.Count);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        public JObject Save()
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
}