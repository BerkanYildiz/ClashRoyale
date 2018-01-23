namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Mode;

    public class CopyDeckCommand : Command
    {
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 580;
            }
        }

        public int CopyDeckId;
        public int[] SpellIds;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyDeckCommand"/> class.
        /// </summary>
        public CopyDeckCommand()
        {
            // CopyDeckCommand.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.CopyDeckId = Stream.ReadVInt();
            this.SpellIds   = new int[Stream.ReadVInt()];

            for (int i = 0; i < this.SpellIds.Length; i++)
            {
                this.SpellIds[i] = Stream.ReadVInt();
            }

            Stream.ReadInt();
            Stream.ReadInt();
            Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.WriteVInt(this.CopyDeckId);
            Stream.WriteVInt(this.SpellIds.Length);

            foreach (int SpellId in this.SpellIds)
            {
                Stream.WriteVInt(SpellId);
            }

            Stream.WriteInt(0);
            Stream.WriteInt(3);
            Stream.WriteVInt(0);
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