namespace ClashRoyale.Logic.Commands.Server
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;

    public class TransactionsRevokedCommand : ServerCommand
    {
        public int Diamonds;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 215;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsRevokedCommand"/> class.
        /// </summary>
        public TransactionsRevokedCommand()
        {
            // TransactionsRevokedCommand.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsRevokedCommand"/> class.
        /// </summary>
        public TransactionsRevokedCommand(int Diamonds)
        {
            this.Diamonds = Diamonds;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);
            this.Diamonds = Stream.ReadInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);
            Stream.WriteInt(this.Diamonds);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override byte Execute(GameMode GameMode)
        {
            Player Player = GameMode.Player;

            if (Player != null)
            {
                Player.UseDiamonds(this.Diamonds);

                return 0;
            }

            return 1;
        }
    }
}