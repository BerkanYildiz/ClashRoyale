namespace ClashRoyale.Server.Logic.Commands.Server
{
    using ClashRoyale.Extensions;

    internal class ServerCommand : Command
    {
        internal int Id;

        /// <summary>
        /// Gets a value indicating whether the this command is a server command.
        /// </summary>
        internal override bool IsServerCommand
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
        internal override void Decode(ByteStream Stream)
        {
            this.Id = Stream.ReadVInt();
            base.Decode(Stream);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal override void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteVInt(this.Id);
            base.Encode(Stream);
        }
    }
}