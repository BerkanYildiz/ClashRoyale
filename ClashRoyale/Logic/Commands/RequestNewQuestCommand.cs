namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Mode;

    public class RequestNewQuestCommand : Command
    {
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 584;
            }
        }

        public int QuestIndex;
        public int QuestUnknown;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestNewQuestCommand"/> class.
        /// </summary>
        public RequestNewQuestCommand()
        {
            // RequestNewQuestCommand.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.QuestIndex     = Stream.ReadVInt();
            this.QuestUnknown   = Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.WriteVInt(this.QuestIndex);
            Stream.WriteVInt(this.QuestUnknown);
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