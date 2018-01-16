namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Mode;

    public class FilterClanChatCommand : Command
    {
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 513;
            }
        }

        public bool IsEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterClanChatCommand"/> class.
        /// </summary>
        public FilterClanChatCommand()
        {
            // FilterClanChatCommand.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);
            this.IsEnabled = Stream.ReadBoolean();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);
            Stream.WriteBoolean(this.IsEnabled);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override byte Execute(GameMode GameMode)
        {
            return 0;
        }
    }
}