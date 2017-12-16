namespace ClashRoyale.Server.Logic.Commands
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Logic.Commands.Manager;
    using ClashRoyale.Server.Logic.Mode;

    using Newtonsoft.Json.Linq;

    internal class Command
    {
        internal int ExecuteTick   = -1;
        internal int TickWhenGiven = -1;

        internal LogicLong ExecutorId;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        internal virtual int Type
        {
            get
            {
                Logging.Info(this.GetType(), "Type must be overridden.");
                return 0;
            }
        }

        /// <summary>
        /// Gets whether this command is a server command.
        /// </summary>
        internal virtual bool IsServerCommand
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        public Command()
        {
            // Command.
        }

        /// <summary>
        /// Decodes the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal virtual void Decode(ByteStream Stream)
        {
            this.TickWhenGiven  = Stream.ReadVInt();
            this.ExecuteTick    = Stream.ReadVInt();

            this.ExecutorId     = Stream.DecodeLogicLong();

            if (this.TickWhenGiven == -1)
            {
                Logging.Warning(this.GetType(), "Command's (type = " + this.Type + ") tickWhenGiven is not set.");
            }
        }

        /// <summary>
        /// Encodes the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal virtual void Encode(ChecksumEncoder Stream)
        {
            Stream.EnableCheckSum(false);

            Stream.WriteVInt(this.TickWhenGiven);
            Stream.WriteVInt(this.ExecuteTick);

            Stream.EncodeLogicLong(this.ExecutorId);

            Stream.EnableCheckSum(true);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        internal virtual JObject Save()
        {
            return new JObject
            {
                {
                    "t", this.TickWhenGiven
                },
                {
                    "t2", this.ExecuteTick
                },
                {
                    "idHi", this.ExecutorId.HigherInt
                },
                {
                    "idLo", this.ExecutorId.LowerInt
                }
            };
        }

        /// <summary>
        /// Gets the command age in ticks.
        /// </summary>
        internal int GetAgeInTicks(int ServerTick)
        {
            return ServerTick - this.TickWhenGiven;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        internal virtual byte Execute(GameMode GameMode)
        {
            return 0;
        }

        /// <summary>
        /// Clones this commande.
        /// </summary>
        internal Command Clone()
        {
            ChecksumEncoder Encoder = new ChecksumEncoder(new ByteStream());
            Command Clone = CommandManager.CreateCommand(this.Type);

            this.Encode(Encoder);
            Encoder.ByteStream.SetOffset(0);
            Clone.Decode(Encoder.ByteStream);

            return Clone;
        }
    }
}