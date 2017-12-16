namespace ClashRoyale.Server.Logic.Slots
{
    using System;
    using System.Collections.Generic;

    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Game;
    using ClashRoyale.Server.Files.Csv;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.Items;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [JsonConverter(typeof(CommoditySlotsConverter))]
    internal class CommoditySlots
    {
        [JsonProperty] private List<DataSlot>[] Slots;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommoditySlots"/> class.
        /// </summary>
        internal CommoditySlots()
        {
            this.Slots = new List<DataSlot>[8];

            this.Slots[0] = new List<DataSlot>(16);
            this.Slots[1] = new List<DataSlot>(16);
            this.Slots[2] = new List<DataSlot>(16);
            this.Slots[3] = new List<DataSlot>(16);
            this.Slots[4] = new List<DataSlot>(16);
            this.Slots[5] = new List<DataSlot>(16);
            this.Slots[6] = new List<DataSlot>(16);
            this.Slots[7] = new List<DataSlot>(16);
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal void Decode(ByteStream Stream)
        {
            int Count = Stream.ReadVInt();

            if (Count != 8)
            {
                Logging.Error(this.GetType(), "Invalid commodity count. Received commodity count:" + Count + ", server commodity count:" + 8);
            }

            for (int I = 0; I < 8; I++)
            {
                for (int J = Stream.ReadVInt(); J > 0; J--)
                {
                    DataSlot DataSlot = new DataSlot();
                    DataSlot.Decode(Stream);
                    this.Slots[I].Add(DataSlot);
                }
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteVInt(8);

            for (int I = 0; I < 8; I++)
            {
                Stream.WriteVInt(this.Slots[I].Count);

                this.Slots[I].ForEach(Slot =>
                {
                    Slot.Encode(Stream);
                });
            }
        }

        /// <summary>
        /// Adds the commodity count.
        /// </summary>
        internal void AddCommodityCount(CommodityType Type, CsvData CsvData, int Count)
        {
            this.AddCommodityCount((int) Type, CsvData, Count);
        }

        /// <summary>
        /// Adds the commodity count.
        /// </summary>
        internal void AddCommodityCount(int CommodityType, CsvData CsvData, int Count)
        {
            if (CommodityType >= 8)
            {
                Logging.Error(this.GetType(), "AddCommodityCount() - Commodity Type is not valid. (" + CommodityType + ")");
                return;
            }

            DataSlot Slot = this.Slots[CommodityType].Find(T => T.Data == CsvData);

            if (Slot == null)
            {
                this.Slots[CommodityType].Add(new DataSlot(CsvData, Count));
            }
            else
                Slot.Count += Count;
        }

        /// <summary>
        /// Gets if the collection has the specified data.
        /// </summary>
        internal bool Exists(int CommodityType, CsvData CsvData)
        {
            if (CommodityType >= 8)
            {
                Logging.Error(this.GetType(), "Exists() - Commodity Type is not valid. (" + CommodityType + ")");
                return false;
            }

            return this.Slots[CommodityType].Exists(T => T.Data.Equals(CsvData));
        }

        /// <summary>
        /// Gets the commodity count.
        /// </summary>
        internal int GetCommodityCount(CommodityType Type, CsvData CsvData)
        {
            return this.GetCommodityCount((int) Type, CsvData);
        }

        /// <summary>
        /// Gets the commodity count.
        /// </summary>
        internal int GetCommodityCount(int CommodityType, CsvData CsvData)
        {
            if (CommodityType >= 8)
            {
                Logging.Error(this.GetType(), "GetCommodityCount() - Commodity Type is not valid. (" + CommodityType + ")");
                return 0;
            }

            DataSlot Slot = this.Slots[CommodityType].Find(T => T.Data == CsvData);

            if (Slot != null)
            {
                return Slot.Count;
            }

            return 0;
        }

        /// <summary>
        /// Sets the commodity count.
        /// </summary>
        internal void SetCommodityCount(CommodityType Type, CsvData CsvData, int Count)
        {
            this.SetCommodityCount((int) Type, CsvData, Count);
        }

        /// <summary>
        /// Sets the commodity count.
        /// </summary>
        internal void SetCommodityCount(int CommodityType, CsvData CsvData, int Count)
        {
            if (CommodityType >= 8)
            {
                Logging.Error(this.GetType(), "SetCommodityCount() - Commodity Type is not valid. (" + CommodityType + ")");
                return;
            }

            DataSlot Slot = this.Slots[CommodityType].Find(T => T.Data == CsvData);

            if (Slot == null)
            {
                this.Slots[CommodityType].Add(new DataSlot(CsvData, Count));
            }
            else
                Slot.Count = Count;
        }

        /// <summary>
        /// Uses the specified commodity count.
        /// </summary>
        internal void UseCommodity(CommodityType CommodityType, CsvData CsvData, int Count)
        {
            this.UseCommodity((int) CommodityType, CsvData, Count);
        }

        /// <summary>
        /// Uses the specified commodity count.
        /// </summary>
        internal void UseCommodity(int CommodityType, CsvData CsvData, int Count)
        {
            if (CommodityType >= 8)
            {
                Logging.Error(this.GetType(), "UseCommodity() - Commodity Type is not valid. (" + CommodityType + ")");
                return;
            }

            DataSlot Slot = this.Slots[CommodityType].Find(T => T.Data == CsvData);

            if (Slot != null)
            {
                Slot.Count -= Count;
            }
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        internal void Load(JArray Array)
        {
            for (int I = 0; I < Array.Count; I++)
            {
                JArray Array2 = (JArray) Array[I];

                for (int J = 0; J < Array2.Count; J++)
                {
                    DataSlot DataSlot = new DataSlot();
                    DataSlot.Load(Array2[J]);
                    this.Slots[I].Add(DataSlot);
                }
            }
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        internal JArray Save()
        {
            JArray Array1 = new JArray();

            for (int I = 0; I < this.Slots.Length; I++)
            {
                JArray Array2 = new JArray();

                this.Slots[I].ForEach(Slot =>
                {
                    Array2.Add(Slot.Save());
                });

                Array1.Add(Array2);
            }

            return Array1;
        }

        /// <summary>
        /// Initializes commodities.
        /// </summary>
        internal void Initialize()
        {
            // Resources
            {
                this.AddCommodityCount(CommodityType.Resource, CsvFiles.GoldData, Globals.StartingGold);
                this.AddCommodityCount(CommodityType.Resource, CsvFiles.FreeGoldData, Globals.StartingGold);
            }
            // ProfileResource
            {
                this.AddCommodityCount(CommodityType.ProfileResource, CsvFiles.GetWithGlobalId(5000008), 0);
                this.AddCommodityCount(CommodityType.ProfileResource, CsvFiles.GetWithGlobalId(5000009), 0);
                this.AddCommodityCount(CommodityType.ProfileResource, CsvFiles.GetWithGlobalId(5000011), 0);
                this.AddCommodityCount(CommodityType.ProfileResource, CsvFiles.GetWithGlobalId(5000027), 0);
            }
        }
    }

    internal class CommoditySlotsConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter Writer, object Value, JsonSerializer Serializer)
        {
            CommoditySlots Slots = (CommoditySlots) Value;

            if (Slots != null)
            {
                Slots.Save().WriteTo(Writer);
            }
            else
                Writer.WriteNull();
        }

        public override object ReadJson(JsonReader Reader, Type ObjectType, object ExistingValue, JsonSerializer Serializer)
        {
            CommoditySlots Slots = (CommoditySlots) ExistingValue;

            if (Slots == null)
            {
                Slots = new CommoditySlots();
            }

            Slots.Load(JArray.Load(Reader));

            return Slots;
        }

        public override bool CanConvert(Type ObjectType)
        {
            return ObjectType == typeof(CommoditySlots);
        }
    }
}