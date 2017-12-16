namespace ClashRoyale.Server.Logic.Items
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Files.Csv.Logic;

    using Newtonsoft.Json.Linq;

    internal class ChestShopItem : ShopItem
    {
        internal TreasureChestData ChestData;

        /// <summary>
        /// Gets the spell shop item type of this instance.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 3;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChestShopItem"/> class.
        /// </summary>
        public ChestShopItem() : base()
        {
            // ChestShopItem.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellShopItem"/> class.
        /// </summary>
        public ChestShopItem(int ShopIndex, int Cost, ResourceData BuyResourceData, TreasureChestData ChestData) : base(ShopIndex, Cost, BuyResourceData)
        {
            this.ChestData = ChestData;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.ChestData   = Stream.DecodeData<TreasureChestData>();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ByteStream Stream)
        {
            base.Encode(Stream);

            Stream.EncodeData(this.ChestData);
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        internal override void Load(JToken Json)
        {
            base.Load(Json);

            JsonHelper.GetJsonData(Json, "chest", out this.ChestData);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        internal override JObject Save()
        {
            JObject Json = base.Save();

            if (this.ChestData != null)
            {
                Json.Add("chest", this.ChestData.GlobalId);
            }

            return Json;
        }
    }
}