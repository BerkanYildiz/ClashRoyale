namespace ClashRoyale.Logic.Player.Slots
{
    using System.Collections.Generic;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Game;
    using ClashRoyale.Files.Csv;
    using ClashRoyale.Logic.Converters;
    using ClashRoyale.Logic.Player.Enums;
    using ClashRoyale.Logic.Player.Items;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [JsonConverter(typeof(CommoditySlotsConverter))]
    public class CommoditySlots
    {
        [JsonProperty] private List<DataSlot>[] Slots;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommoditySlots" /> class.
        /// </summary>
        public CommoditySlots()
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
        ///     Decodes this instance.
        /// </summary>
        public void Decode(ByteStream Stream)
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
        ///     Encodes this instance.
        /// </summary>
        public void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteVInt(8);

            for (int I = 0; I < 8; I++)
            {
                Stream.WriteVInt(this.Slots[I].Count);

                this.Slots[I].ForEach(Slot => { Slot.Encode(Stream); });
            }
        }

        /// <summary>
        ///     Adds the commodity count.
        /// </summary>
        public void AddCommodityCount(CommodityType Type, CsvData CsvData, int Count)
        {
            this.AddCommodityCount((int) Type, CsvData, Count);
        }

        /// <summary>
        ///     Adds the commodity count.
        /// </summary>
        public void AddCommodityCount(int CommodityType, CsvData CsvData, int Count)
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
            {
                Slot.Count += Count;
            }
        }

        /// <summary>
        ///     Gets if the collection has the specified data.
        /// </summary>
        public bool Exists(int CommodityType, CsvData CsvData)
        {
            if (CommodityType >= 8)
            {
                Logging.Error(this.GetType(), "Exists() - Commodity Type is not valid. (" + CommodityType + ")");
                return false;
            }

            return this.Slots[CommodityType].Exists(T => T.Data.Equals(CsvData));
        }

        /// <summary>
        ///     Gets the commodity count.
        /// </summary>
        public int GetCommodityCount(CommodityType Type, CsvData CsvData)
        {
            return this.GetCommodityCount((int) Type, CsvData);
        }

        /// <summary>
        ///     Gets the commodity count.
        /// </summary>
        public int GetCommodityCount(int CommodityType, CsvData CsvData)
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
        ///     Sets the commodity count.
        /// </summary>
        public void SetCommodityCount(CommodityType Type, CsvData CsvData, int Count)
        {
            this.SetCommodityCount((int) Type, CsvData, Count);
        }

        /// <summary>
        ///     Sets the commodity count.
        /// </summary>
        public void SetCommodityCount(int CommodityType, CsvData CsvData, int Count)
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
            {
                Slot.Count = Count;
            }
        }

        /// <summary>
        ///     Uses the specified commodity count.
        /// </summary>
        public void UseCommodity(CommodityType CommodityType, CsvData CsvData, int Count)
        {
            this.UseCommodity((int) CommodityType, CsvData, Count);
        }

        /// <summary>
        ///     Uses the specified commodity count.
        /// </summary>
        public void UseCommodity(int CommodityType, CsvData CsvData, int Count)
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
        ///     Loads this instance from json.
        /// </summary>
        public void Load(JArray Array)
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
        ///     Saves this instance to json.
        /// </summary>
        public JArray Save()
        {
            JArray Array1 = new JArray();

            for (int I = 0; I < this.Slots.Length; I++)
            {
                JArray Array2 = new JArray();

                this.Slots[I].ForEach(Slot => { Array2.Add(Slot.Save()); });

                Array1.Add(Array2);
            }

            return Array1;
        }

        /// <summary>
        ///     Initializes commodities.
        /// </summary>
        public void Initialize()
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
}