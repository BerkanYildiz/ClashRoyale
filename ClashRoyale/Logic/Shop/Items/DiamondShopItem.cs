namespace ClashRoyale.Logic.Shop.Items
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;

    using Newtonsoft.Json.Linq;

    public class DiamondShopItem : ShopItem
    {
        public bool Free;
        public int Amount;

        /// <summary>
        /// Gets the spell shop item type of this instance.
        /// </summary>
        public override int Type
        {
            get
            {
                return 5;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiamondShopItem"/> class.
        /// </summary>
        public DiamondShopItem() : base()
        {
            // DiamondShopItem.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiamondShopItem"/> class.
        /// </summary>
        public DiamondShopItem(int ShopIndex, int Cost, ResourceData BuyResourceData, int Amount, bool Free) : base(ShopIndex, Cost, BuyResourceData)
        {
            this.Free = Free;
            this.Amount = Amount;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.Amount = Stream.ReadVInt();
            this.Free = Stream.ReadBoolean();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.WriteVInt(this.Amount);
            Stream.WriteBoolean(this.Free);
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        public override void Load(JToken Json)
        {
            base.Load(Json);

            JsonHelper.GetJsonNumber(Json, "amount", out this.Amount);
            JsonHelper.GetJsonBoolean(Json, "free", out this.Free);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        public override JObject Save()
        {
            JObject Json = base.Save();

            Json.Add("amount", this.Amount);
            Json.Add("free", this.Free);

            return Json;
        }
    }
}