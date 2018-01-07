namespace ClashRoyale.Logic.Shop.Items
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;

    using Newtonsoft.Json.Linq;

    public class SpecialChestShopItem : ShopItem
    {
        public int ChestType;
        public TreasureChestData SpecialChestData;

        /// <summary>
        /// Gets the spell shop item type of this instance.
        /// </summary>
        public override int Type
        {
            get
            {
                return 4;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialChestShopItem"/> class.
        /// </summary>
        public SpecialChestShopItem() : base()
        {
            // SpecialChestShopItem.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellShopItem"/> class.
        /// </summary>
        public SpecialChestShopItem(int ShopIndex, int Cost, ResourceData BuyResourceData, TreasureChestData SpecialChestData, int ChestType) : base(ShopIndex, Cost, BuyResourceData)
        {
            this.ChestType = ChestType;
            this.SpecialChestData = SpecialChestData;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.SpecialChestData = Stream.DecodeData<TreasureChestData>();
            this.ChestType = Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.EncodeData(this.SpecialChestData);
            Stream.WriteVInt(this.ChestType);
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        public override void Load(JToken Json)
        {
            base.Load(Json);

            JsonHelper.GetJsonData(Json, "chest", out this.SpecialChestData);
            JsonHelper.GetJsonNumber(Json, "type", out this.ChestType);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        public override JObject Save()
        {
            JObject Json = base.Save();

            if (this.SpecialChestData != null)
            {
                Json.Add("chest", this.SpecialChestData.GlobalId);
            }

            return Json;
        }
    }
}