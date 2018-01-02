namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Home;
    using ClashRoyale.Logic.Mode;

    public class PageOpenedCommand : Command
    {
        public int Page;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 599;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageOpenedCommand"/> class.
        /// </summary>
        public PageOpenedCommand()
        {
            // PageOpenedCommand.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageOpenedCommand"/> class.
        /// </summary>
        public PageOpenedCommand(int Page)
        {
            this.Page = Page;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.Page = Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.WriteVInt(this.Page);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override byte Execute(GameMode GameMode)
        {
            Home Home = GameMode.Home;

            if (Home != null)
            {
                if (this.Page > 0 && this.Page <= 4)
                {
                    Home.SetPageOpened(this.Page);

                    return 0                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              ;
                }

                return 2;
            }

            return 1;
        }
    }
}