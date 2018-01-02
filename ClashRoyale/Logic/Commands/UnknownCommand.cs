namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Mode;

    public class UnknownCommand : Command
    {
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 501;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownCommand"/> class.
        /// </summary>
        public UnknownCommand()
        {
            // UnknownCommand.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            Stream.ReadVInt();
            Stream.ReadVInt();
            Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.WriteVInt(-1);
            Stream.WriteVInt(8);
            Stream.WriteVInt(-1);
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