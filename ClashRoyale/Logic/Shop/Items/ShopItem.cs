namespace ClashRoyale.Logic.Shop.Items
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;

    using Newtonsoft.Json.Linq;

    public class ShopItem
    {
        public int Cost;
        public int ShopIndex;

        public ResourceData BuyResourceData;

        /// <summary>
        /// Gets the spell shop item type of this instance.
        /// </summary>
        public virtual int Type
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
        /// <param name="ShopIndex">Index of the shop.</param>
        /// <param name="Cost">The cost.</param>
        /// <param name="BuyResourceData">The buy resource data.</param>
        public ShopItem(int ShopIndex, int Cost, ResourceData BuyResourceData) : this()
        {
            this.Cost               = Cost;
            this.ShopIndex          = ShopIndex;
            this.BuyResourceData    = BuyResourceData;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public virtual void Decode(ByteStream Stream)
        {
            Stream.ReadVInt();
            this.ShopIndex          = Stream.ReadVInt();
            Stream.ReadVInt();
            this.Cost               = Stream.ReadVInt();
            this.BuyResourceData    = Stream.DecodeData<ResourceData>();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public virtual void Encode(ChecksumEncoder Stream)
        {
            /*  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  * *
             *  01  00  00  9A-05  96-02  05-01  1A-13  0F  00  00  B8-2E  00-00 *
             *  01  00  01  9A-05  AC-04  05-01  1A-0D  1E  00  00  B8-2E  00-00 *
             *  01  00  02  9A-05  B4-07  05-01  1A-31  32  00  00  B8-2E  00-00 *
             *  01  00  03  9A-05  A0-0C  05-01  1B-0A  08  01  00  B8-2E  00-01 *
             *  01  00  04  9A-05  B0-12  05-01  1C-03  0C  01  00  B8-2E  00-00 *
             *  01  00  05  9A-05  80-19  05-01  1A-03  10  01  00  B8-2E  00-00 *
             *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  * */

            Stream.WriteVInt(0);
            Stream.WriteVInt(this.ShopIndex);
            Stream.WriteVInt(0);
            Stream.WriteVInt(this.Cost);
            Stream.EncodeData(this.BuyResourceData);
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        public virtual void Load(JToken Json)
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
        public virtual JObject Save()
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