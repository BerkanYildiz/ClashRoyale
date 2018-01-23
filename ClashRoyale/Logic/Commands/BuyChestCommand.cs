namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Home;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;

    public class BuyChestCommand : Command
    {
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 539;
            }
        }

        public int ChestIndex;

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
        public override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);
            this.ChestIndex = Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);
            Stream.WriteVInt(this.ChestIndex);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override byte Execute(GameMode GameMode)
        {
            if (this.ChestIndex < 0)
            {
                return 1;
            }

            Home Home     = GameMode.Home;
            Player Player = GameMode.Player;

            if (Home != null && Player != null)
            {
                if (this.ChestIndex < Home.ShopChests.Count)
                {
                    var Chest = Home.ShopChests[this.ChestIndex];

                    if (Chest != null)
                    {
                        int Cost = Chest.Cost;

                        if (Player.HasEnoughDiamonds(Cost))
                        {
                            if (Home.PurchasedChest == null)
                            {
                                Player.UseDiamonds(Cost);
                                Home.ChestPurchased(Chest.ChestData, 3);

                                return 0;
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
    }
}