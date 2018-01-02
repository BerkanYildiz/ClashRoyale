namespace ClashRoyale.Logic.Commands.Server
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Extensions.Utils;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Home.Spells;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;

    public class AllianceUnitReceivedCommand : ServerCommand
    {
        public string Sender;
        public SpellData Data;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 208;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceUnitReceivedCommand"/> class.
        /// </summary>
        public AllianceUnitReceivedCommand()
        {
            // AllianceUnitReceivedCommand.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimRewardCommand"/> class.
        /// </summary>
        public AllianceUnitReceivedCommand(string Sender, SpellData Data)
        {
            this.Sender = Sender;
            this.Data = Data;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            this.Sender = Stream.ReadString();
            this.Data = Stream.DecodeData<SpellData>();

            base.Decode(Stream);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteString(this.Sender);
            Stream.EncodeData(this.Data);

            base.Encode(Stream);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override byte Execute(GameMode GameMode)
        {
            Player Player = GameMode.Player;

            if (Player != null)
            {
                Spell Spell = GameMode.Home.GetSpellByData(this.Data);

                if (Spell == null)
                {
                    Spell = new Spell(this.Data);
                    Spell.SetCreateTime(TimeUtil.MinutesSince1970);

                    GameMode.Home.AddSpell(Spell);
                }
                else
                {
                    Spell.AddMaterial(1);
                }

                return 0;
            }

            return 1;
        }
    }
}