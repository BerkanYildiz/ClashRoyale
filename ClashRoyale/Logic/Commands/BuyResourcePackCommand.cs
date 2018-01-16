namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Mode;

    public class BuyResourcePackCommand : Command
    {
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

        private ResourcePackData ResourcePackData;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuyResourcePackCommand"/> class.
        /// </summary>
        public BuyResourcePackCommand()
        {
            // BuyResourcePackCommand.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuyResourcePackCommand"/> class.
        /// </summary>
        /// <param name="ResourcePackData">The resource pack data.</param>
        public BuyResourcePackCommand(ResourcePackData ResourcePackData)
        {
            this.ResourcePackData = ResourcePackData;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);
            this.ResourcePackData = Stream.DecodeData<ResourcePackData>();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);
            Stream.EncodeData(this.ResourcePackData);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override byte Execute(GameMode GameMode)
        {
            if (this.ResourcePackData != null)
            {
                var ResourceData = CsvFiles.Get(Gamefile.Resources).GetData<ResourceData>(this.ResourcePackData.Resource);

                if (ResourceData != null)
                {
                    int Cost = this.ResourcePackData.Cost;

                    if (GameMode.Player.HasEnoughDiamonds(Cost))
                    {
                        GameMode.Player.UseDiamonds(Cost);
                        GameMode.Player.AddResource(ResourceData, this.ResourcePackData.Amount);

                        return 0;
                    }

                     return 3;
                }

                return 2;
            }

            return 1;
        }
    }
}