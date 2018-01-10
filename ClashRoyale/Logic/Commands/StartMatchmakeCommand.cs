namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Home;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;
    using ClashRoyale.Messages.Server.Matchmaking;

    public class StartMatchmakeCommand : Command
    {
        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 594;
            }
        }

        private bool Is2v2;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartMatchmakeCommand"/> class.
        /// </summary>
        public StartMatchmakeCommand()
        {                       
            // StartMatchmakeCommand.
        }

        /// <summary>
        /// Decodes the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public override void Decode(ByteStream Stream)
        {
            base.Decode(Stream);

            this.Is2v2 = Stream.ReadBoolean();
            Stream.ReadVInt();
            Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.WriteBoolean(this.Is2v2);
            Stream.WriteVInt(0);
            Stream.WriteVInt(-1);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override byte Execute(GameMode GameMode)
        {
            Home Home       = GameMode.Home;
            Player Player   = GameMode.Player;

            if (Home != null)
            {
                if (Player != null)
                {
                    if (Player.Arena.TrainingCamp)
                    {
                        return 3;
                    }

                    if (!GameMode.Listener.IsAndroid)
                    {
                        GameMode.Listener.SendMessage(new MatchmakeFailedMessage());
                    }
                    else
                    {
                        GameMode.Listener.Matchmaking();
                    }
                    
                    return 0;
                }

                return 2;
            }

            return 1;
        }
    }
}