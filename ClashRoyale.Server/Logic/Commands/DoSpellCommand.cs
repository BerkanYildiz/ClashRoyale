namespace ClashRoyale.Server.Logic.Commands
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.Home.Spells;
    using ClashRoyale.Server.Logic.Math;
    using ClashRoyale.Server.Logic.Mode;

    using Newtonsoft.Json.Linq;

    internal class DoSpellCommand : Command
    {
        internal int SpellIndex;
        internal int SpellDeckIndex;

        internal Spell Spell;
        internal Vector2 Coordinate;
        internal SpellData SpellData;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoSpellCommand"/> class.
        /// </summary>
        public DoSpellCommand()
        {
            this.Coordinate = new Vector2();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoSpellCommand"/> class.
        /// </summary>
        public DoSpellCommand(SpellData Data, int X, int Y) : this()
        {
            this.SpellData = Data;
            this.Coordinate.Set(X, Y);
        }
        
        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.SpellDeckIndex = Stream.ReadVInt();
            this.SpellData = Stream.DecodeData<SpellData>();
            this.SpellIndex = Stream.ReadVInt();

            if (Stream.ReadBoolean())
            {
                this.Spell = new Spell(this.SpellData);
                this.Spell.DecodeAttack(Stream);
            }

            this.Coordinate.Decode(Stream);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);
            
            Stream.WriteVInt(this.SpellDeckIndex);
            Stream.EncodeData(this.SpellData);
            Stream.WriteVInt(this.SpellIndex);

            if (this.Spell != null)
            {
                Stream.WriteBoolean(true);
                this.Spell.EncodeAttack(Stream);
            }
            else
            {
                Stream.WriteBoolean(false);
            }

            this.Coordinate.Encode(Stream);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        internal override byte Execute(GameMode GameMode)
        {
            return 0;
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        internal override JObject Save()
        {
            JObject Json = base.Save();

            Json.Add("idx", this.SpellDeckIndex);
            Json.Add("gid", this.SpellData.GlobalId);
            Json.Add("px", this.Coordinate.X);
            Json.Add("py", this.Coordinate.Y);
            Json.Add("sid", this.SpellIndex);

            return Json;
        }
    }
}