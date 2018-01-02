namespace ClashRoyale.Logic.Commands
{
    using System;

    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Logic.Commands.Manager;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Maths;

    using Newtonsoft.Json.Linq;

    public class Command
    {
        public int ExecuteTick   = -1;
        public int TickWhenGiven = -1;

        public LogicLong ExecutorId;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public virtual int Type
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
        public virtual bool IsServerCommand
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
        public virtual void Decode(ByteStream Stream)
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
        public virtual void Encode(ChecksumEncoder Stream)
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
        public virtual JObject Save()
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
        public int GetAgeInTicks(int ServerTick)
        {
            return ServerTick - this.TickWhenGiven;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public virtual byte Execute(GameMode GameMode)
        {
            return 0;
        }

        /// <summary>
        /// Clones this commande.
        /// </summary>
        public Command Clone()
        {
            ChecksumEncoder Encoder = new ChecksumEncoder(new ByteStream());
            Command Clone = CommandManager.CreateCommand(this.Type);

            this.Encode(Encoder);
            Encoder.ByteStream.SetOffset(0);
            Clone.Decode(Encoder.ByteStream);

            return Clone;
        }

        /// <summary>
        /// Gets the packet data, in/from an hexa string.
        /// </summary>
        public string ToHexa(ByteStream Stream)
        {
            return BitConverter.ToString(Stream.ToArray(Stream.Offset, Stream.BytesLeft));
        }
    }
}