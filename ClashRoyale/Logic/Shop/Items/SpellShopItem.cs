namespace ClashRoyale.Logic.Shop.Items
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;

    using Newtonsoft.Json.Linq;

    public class SpellShopItem : ShopItem
    {
        public int Amount;
        public int RarityIndex;
        public SpellData SpellData;

        /// <summary>
        /// Gets the spell shop item type of this instance.
        /// </summary>
        public override int Type
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellShopItem"/> class.
        /// </summary>
        public SpellShopItem() : base()
        {
            // SpellShopItem.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellShopItem"/> class.
        /// </summary>
        public SpellShopItem(int ShopIndex, int Cost, ResourceData BuyResourceData, SpellData SpellData, int Amount, int RarityIndex) : base(ShopIndex, Cost, BuyResourceData)
        {
            this.Amount = Amount;
            this.RarityIndex = RarityIndex;
            this.SpellData = SpellData;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.SpellData   = Stream.DecodeData<SpellData>();
            this.Amount      = Stream.ReadVInt();
            this.RarityIndex = Stream.ReadVInt();
            Stream.ReadBoolean();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.EncodeData(this.SpellData);
            Stream.WriteVInt(this.Amount);
            Stream.WriteVInt(this.RarityIndex);
            Stream.WriteBoolean(false);
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        public override void Load(JToken Json)
        {
            base.Load(Json);
            
            JsonHelper.GetJsonData(Json, "spell", out this.SpellData);
            JsonHelper.GetJsonNumber(Json, "amount", out this.Amount);
            JsonHelper.GetJsonNumber(Json, "rarity", out this.RarityIndex);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        public override JObject Save()
        {
            JObject Json = base.Save();

            if (this.SpellData != null)
            {
                Json.Add("spell", this.SpellData.GlobalId);
            }

            Json.Add("amount", this.Amount);
            Json.Add("rarity", this.RarityIndex);

            return Json;
        }
    }
}