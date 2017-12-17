namespace ClashRoyale.Server.Logic.Shop.Items
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Files.Csv.Logic;

    using Newtonsoft.Json.Linq;

    internal class ShopItem
    {
        internal int Cost;
        internal int ShopIndex;

        internal ResourceData BuyResourceData;

        /// <summary>
        /// Gets the spell shop item type of this instance.
        /// </summary>
        internal virtual int Type
        {
            get
            {
                return -1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopItem"/> class.
        /// </summary>
        public ShopItem()
        {
            // ShopItem.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopItem"/> class.
        /// </summary>
        public ShopItem(int ShopIndex, int Cost, ResourceData BuyResourceData) : this()
        {
            this.Cost = Cost;
            this.ShopIndex = ShopIndex;
            this.BuyResourceData = BuyResourceData;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal virtual void Decode(ByteStream Stream)
        {
            Stream.ReadVInt();
            this.ShopIndex = Stream.ReadVInt();
            Stream.ReadVInt();
            this.Cost = Stream.ReadVInt();
            this.BuyResourceData = Stream.DecodeData<ResourceData>();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal virtual void Encode(ByteStream Stream)
        {
            Stream.WriteVInt(0);
            Stream.WriteVInt(this.ShopIndex);
            Stream.WriteVInt(0);
            Stream.WriteVInt(this.Cost);
            Stream.EncodeData(this.BuyResourceData);
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        internal virtual void Load(JToken Json)
        {
            if (JsonHelper.GetJsonObject(Json, "base", out JToken Base))
            {
                JsonHelper.GetJsonNumber(Base, "si", out this.ShopIndex);
                JsonHelper.GetJsonNumber(Base, "cost", out this.Cost);
                JsonHelper.GetJsonData(Base, "bd", out this.BuyResourceData);
            }
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        /// <returns></returns>
        internal virtual JObject Save()
        {
            JObject Base = new JObject();

            Base.Add("si", this.ShopIndex);
            Base.Add("cost", this.Cost);
            Base.Add("bd", this.BuyResourceData.GlobalId);

            return new JObject
            {
                {
                    "base", Base
                }
            };
        }
    }
}