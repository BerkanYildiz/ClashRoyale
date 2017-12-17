namespace ClashRoyale.Server.Logic.Commands
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.Home;
    using ClashRoyale.Server.Logic.Mode;
    using ClashRoyale.Server.Logic.Player;

    internal class BuyChestCommand : Command
    {
        private TreasureChestData ChestData;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 516;
            }
        }

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
            this.ChestData = Stream.DecodeData<TreasureChestData>();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);
            Stream.EncodeData(this.ChestData);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        internal override byte Execute(GameMode GameMode)
        {
            if (this.ChestData != null)
            {
                Home Home = GameMode.Home;
                Player Player = GameMode.Player;

                if (Home != null && Player != null)
                {
                    if (this.ChestData.ArenaData != null)
                    {
                        if (!this.ChestData.ArenaData.TrainingCamp)
                        {
                            if (this.ChestData.ArenaData == Player.Arena.ChestArenaData)
                            {
                                if (this.ChestData.InShop)
                                {
                                    int Cost = this.ChestData.ShopPrice;
                                    
                                    if (Player.HasEnoughDiamonds(Cost))
                                    {
                                        if (Home.PurchasedChest == null)
                                        {
                                            Player.UseDiamonds(Cost);
                                            Home.ChestPurchased(this.ChestData, 3);

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