namespace ClashRoyale.Logic.Commands.Server
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Home;
    using ClashRoyale.Logic.Home.Spells;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;
    using ClashRoyale.Logic.Reward;

    using Math = ClashRoyale.Maths.Math;

    public class ClaimRewardCommand : ServerCommand
    {
        private Reward Reward;
        
        private int LocationId;
        private int ChestType;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 210;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimRewardCommand"/> class.
        /// </summary>
        public ClaimRewardCommand()
        {
            // ClaimRewardCommand.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimRewardCommand"/> class.
        /// </summary>
        public ClaimRewardCommand(Reward Reward, int ChestType, int LocationId)
        {
            this.Reward = Reward;
            this.ChestType = ChestType;
            this.LocationId = LocationId;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            if (Stream.ReadBoolean())
            {
                this.Reward = Reward.DecodeReward(Stream);
            }

            this.LocationId = Stream.ReadVInt();
            this.ChestType = Stream.ReadVInt();

            base.Decode(Stream);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            if (this.Reward != null)
            {
                Stream.WriteBoolean(true);              
                Reward.EncodeReward(Stream, this.Reward);
            }
            else 
                Stream.WriteBoolean(false);
            
            Stream.WriteVInt(this.LocationId);
            Stream.WriteVInt(this.ChestType);
            
            base.Encode(Stream);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override byte Execute(GameMode GameMode)
        {
            Home Home = GameMode.Home;
            Player Player = GameMode.Player;

            TreasureChestData TreasureChestData = null;

            if (Home == null)
            {
                return 1;
            }

            if (Player == null)
            {
                return 2;
            }

            switch (this.ChestType)
            {
                case 2:
                {
                    if (GameMode.Home.FreeChest != null)
                    {
                        TreasureChestData = GameMode.Home.FreeChest.Data;

                        if (TreasureChestData == null)
                        {
                            goto End;
                        }
                    }

                    break;
                }
                case 3:
                {
                    if (GameMode.Home.StarChest != null)
                    {
                        TreasureChestData = GameMode.Home.StarChest.Data;

                        if (TreasureChestData == null)
                        {
                            goto End;
                        }
                    }

                    break;
                }
                case 4:
                {
                    if (GameMode.Home.PurchasedChest != null)
                    {
                        TreasureChestData = GameMode.Home.PurchasedChest.Data;

                        if (TreasureChestData == null)
                        {
                            goto End;
                        }
                    }

                    break;
                }
            }

            if (this.Reward.Spells == null)
            {
                goto End;
            }

            this.Reward.Spells.ForEach(Spell =>
            {
                if (!Spell.Data.NotInUse)
                {
                    Spell Existing = Home.GetSpellByData(Spell.Data);
                    int RefundGold;

                    if (Existing == null)
                    {
                        Existing = new Spell(Spell.Data);
                        Existing.SetShowNewIcon(true);

                        RefundGold = Existing.AddMaterial(Spell.Count);

                        Home.AddSpell(Existing);
                    }
                    else
                    {
                        RefundGold = Existing.AddMaterial(Spell.Count);
                    }

                    RefundGold *= Existing.Data.RarityData.GoldConversionValue;

                    if (RefundGold > 0)
                    {
                        if (Player.Gold + RefundGold > Player.MaxGold)
                        {
                            RefundGold = Math.Max(Player.MaxGold - Player.Gold, 0);
                        }

                        Player.AddFreeGold(RefundGold);
                    }
                }
            });

            End:

            if (Player.Gold + this.Reward.Gold > Player.MaxGold)
            {
                this.Reward.Gold = Math.Max(Player.MaxGold - Player.Gold, 0);
            }

            if (this.Reward.Gold > 0)
            {
                Player.AddFreeGold(this.Reward.Gold);
            }

            if (this.Reward.Diamonds > 0)
            {
                Player.AddFreeDiamonds(this.Reward.Diamonds);
            }

            Home.SetClaimingReward(false);

            switch (this.ChestType)
            {
                case 2:
                {
                    Home.FreeChestCollected();
                    break;
                }
                case 3:
                {
                    Home.CrownChestCollected();
                    break;
                }
                case 4:
                {
                    Home.PurchasedChestCollected();
                    break;
                }
            }

            if (TreasureChestData != null)
            {
                Player.XpGainHelper(TreasureChestData.Exp);
            }

            return 0;
        }
    }
}