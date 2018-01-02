namespace ClashRoyale.Logic.Commands.Server
{
    using ClashRoyale.Exceptions;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;

    public class DiamondsRemovedCommand : ServerCommand
    {
        
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 275;
            }
        }

        private int Diamonds;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiamondsRemovedCommand"/> class.
        /// </summary>
        public DiamondsRemovedCommand()
        {
            // DiamondsRemovedCommand.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiamondsRemovedCommand"/> class.
        /// </summary>
        public DiamondsRemovedCommand(int Diamonds)
        {
            if (Diamonds < 0)
            {
                throw new LogicException(this.GetType(), "Diamonds < 0 at DiamondsRemovedCommand(Diamonds: "+ Diamonds + ").");
            }

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
                if (this.Diamonds > 0)
                {
                    GameMode.Player.AddFreeDiamonds(-this.Diamonds);
                }
                else
                {
                    return 2;
                }

                return 0;
            }

            return 1;
        }
    }
}