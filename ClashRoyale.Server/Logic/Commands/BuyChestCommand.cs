namespace ClashRoyale.Server.Logic.Commands
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.Home;
    using ClashRoyale.Server.Logic.Mode;
    using ClashRoyale.Server.Logic.Player;

    internal class BuyChestCommand : Command
    {
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 539;
            }
        }

        private int ChestIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuyChestCommand"/> class.
        /// </summary>
        public BuyChestCommand()
        {
            // BuyChestCommand.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);
            this.ChestIndex = Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);
            Stream.WriteVInt(this.ChestIndex);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        internal override byte Execute(GameMode GameMode)
        {
            if (this.ChestIndex < 3)
            {
                Home Home     = GameMode.Home;
                Player Player = GameMode.Player;

                if (Home != null && Player != null)
                {
                    TreasureChestData ChestData = null; // TODO : Retrieve the chest at the specified index in the shop.

                    if (ChestData.ArenaData != null)
                    {
                        if (!ChestData.ArenaData.TrainingCamp)
                        {
                            if (ChestData.ArenaData == Player.Arena.ChestArenaData)
                            {
                                if (ChestData.InShop)
                                {
                                    int Cost = ChestData.ShopPrice;
                                    
                                    if (Player.HasEnoughDiamonds(Cost))
                                    {
                                        if (Home.PurchasedChest == null)
                                        {
                                            Player.UseDiamonds(Cost);
                                            Home.ChestPurchased(ChestData, 3);

                                            return 0;
                                        }

                                        return 8;
                                    }

                                    return 7;
                                }

                                return 6;
                            }

                            return 5;
                        }

                        return 4;
                    }

                    return 3;
                }

                return 2;
            }

            return 1;
        }
    }
}