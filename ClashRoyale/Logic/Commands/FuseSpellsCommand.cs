namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Home.Spells;
    using ClashRoyale.Logic.Mode;

    public class FuseSpellsCommand : Command
    {
        public SpellData SpellData;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 592;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FuseSpellsCommand"/> class.
        /// </summary>
        public FuseSpellsCommand()
        {
            // FuseSpellsCommand.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FuseSpellsCommand"/> class.
        /// </summary>
        public FuseSpellsCommand(SpellData Data)
        {
            this.SpellData = Data;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.SpellData = Stream.DecodeData<SpellData>();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.EncodeData(this.SpellData);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override byte Execute(GameMode GameMode)
        {
            var Home = GameMode.Home;

            if (Home != null)
            {
                Spell Spell = Home.GetSpellByData(this.SpellData);

                if (Spell != null)
                {
                    if (Spell.Level < this.SpellData.MaxLevelIndex)
                    {
                        if (Spell.CanUpgrade)
                        {
                            int Cost = this.SpellData.RarityData.UpgradeCost[Spell.Level];

                            if (GameMode.Player.HasEnoughResources(CsvFiles.GoldData, Cost))
                            {
                                GameMode.Player.UseGold(Cost);
                                GameMode.Player.XpGainHelper(Spell.UpgradeExp);

                                Spell.UpgradeToNextLevel();

                                return 0;
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