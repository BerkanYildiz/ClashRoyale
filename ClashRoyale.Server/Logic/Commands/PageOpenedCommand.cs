namespace ClashRoyale.Server.Logic.Commands
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Server.Logic.Home;
    using ClashRoyale.Server.Logic.Mode;

    internal class PageOpenedCommand : Command
    {
        internal int Page;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 527;
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
        internal override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.Page = Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.WriteVInt(this.Page);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        internal override byte Execute(GameMode GameMode)
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