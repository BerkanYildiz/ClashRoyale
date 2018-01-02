namespace ClashRoyale.Logic.Commands.Server
{
    using ClashRoyale.Extensions;

    public class ServerCommand : Command
    {
        public int Id;

        /// <summary>
        /// Gets a value indicating whether the this command is a server command.
        /// </summary>
        public override bool IsServerCommand
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerCommand"/> class.
        /// </summary>
        public ServerCommand()
        {
            // ServerCommand.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public override void Decode(ByteStream Stream)
        {
            this.Id = Stream.ReadVInt();
            base.Decode(Stream);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public override void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteVInt(this.Id);
            base.Encode(Stream);
        }
    }
}