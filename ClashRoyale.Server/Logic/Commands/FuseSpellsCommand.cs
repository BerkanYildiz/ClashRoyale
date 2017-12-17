namespace ClashRoyale.Server.Logic.Commands
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Files.Csv;
    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.Home.Spells;
    using ClashRoyale.Server.Logic.Mode;

    internal class FuseSpellsCommand : Command
    {
        internal SpellData SpellData;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 504;
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
        internal override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.SpellData = Stream.DecodeData<SpellData>();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.EncodeData(this.SpellData);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        internal override byte Execute(GameMode GameMode)
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