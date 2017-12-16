namespace ClashRoyale.Server.Logic.Commands.Server
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.Mode;

    internal class AddChestCommand : ServerCommand
    {
        internal bool Add;
        internal ArenaData ArenaData;
        internal string Name;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 211;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddChestCommand"/> class.
        /// </summary>
        public AddChestCommand()
        {
            // AddChestCommand.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddChestCommand"/> class.
        /// </summary>
        public AddChestCommand(bool Add, ArenaData ArenaData, string Name)
        {
            this.Add = Add;
            this.ArenaData = ArenaData;
            this.Name = Name;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode(ByteStream Stream)
        {
            this.Add = Stream.ReadBoolean();
            Stream.ReadBoolean();
            Stream.ReadBoolean();
            this.ArenaData = Stream.DecodeData<ArenaData>();
            this.Name = Stream.ReadString();

            base.Decode(Stream);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteBoolean(this.Add);
            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);
            Stream.EncodeData(this.ArenaData);
            Stream.WriteString(this.Name);
            Stream.WriteVInt(3);

            base.Encode(Stream);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        internal override byte Execute(GameMode GameMode)
        {
            return 0;
        }
    }
}