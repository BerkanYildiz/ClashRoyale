namespace ClashRoyale.Server.Logic.Items
{
    internal static class ShopItemFactory
    {
        /// <summary>
        /// Creates a new instance of the shop item.
        /// </summary>
        internal static ShopItem CreateShopItem(int Type)
        {
            switch (Type)
            {
                case 1:
                {
                    return new SpellShopItem();
                }

                case 2:
                {
                    return new ResourceShopItem();
                }

                case 3:
                {
                    return new ChestShopItem();
                }

                case 4:
                {
                    return new SpecialChestShopItem();
                }

                case 5:
                {
                    return new DiamondShopItem();
                }

                default:
                {
                    Logging.Info(typeof(ShopItemFactory), "CreateShopItem() - Unknown shop cycle item type.");
                    return null;
                }
            }
        }
    }
}