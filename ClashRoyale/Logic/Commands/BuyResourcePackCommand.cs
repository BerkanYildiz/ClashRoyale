namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Home;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;

    public class BuyResourcePackCommand : Command
    {
        private TreasureChestData ChestData;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 511;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuyResourcePackCommand"/> class.
        /// </summary>
        public BuyResourcePackCommand()
        {
            // BuyResourcePackCommand.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);
            this.ChestData = Stream.DecodeData<TreasureChestData>();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);
            Stream.EncodeData(this.ChestData);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override byte Execute(GameMode GameMode)
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