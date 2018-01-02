namespace ClashRoyale.Logic.Commands.Server
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Mode;

    public class AddChestCommand : ServerCommand
    {
        public bool Add;
        public ArenaData ArenaData;
        public string Name;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
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
        public override void Decode(ByteStream Stream)
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
        public override void Encode(ChecksumEncoder Stream)
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
        public override byte Execute(GameMode GameMode)
        {
            return 0;
        }
    }
}