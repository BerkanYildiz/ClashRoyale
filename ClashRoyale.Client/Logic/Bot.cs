namespace ClashRoyale.Client.Logic
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Client.Network;
    using ClashRoyale.Client.Network.Packets.Client;

    using ClashRoyale.Enums;

    using Timer = System.Timers.Timer;

    internal class Bot
    {
        /// <summary>
        /// Gets or sets the bot identifier.
        /// </summary>
        internal int BotId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Bot"/> is logged.
        /// </summary>
        internal bool IsLogged
        {
            get
            {
                if (this.State == State.Logged)
                {
                    return true;
                }

                return false;
            }
        }
        
        internal NetworkManager Network;
        internal Timer Timer;

        internal State State;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bot"/> class.
        /// </summary>
        internal Bot()
        {
            this.Network = new NetworkManager(this);

            if (this.Network.TryConnect())
            {
                this.Network.SendMessage(new PreLoginMessage(this));

                Task.Run(() =>
                {
                    // Thread.Sleep(3000);
                    // this.Network.SendMessage(new StartTrainingBattleMessage(this));
                    // Logging.Info(this.GetType(), "Message sent");
                });
            }
            else
            {
                Logging.Info(this.GetType(), "TryConnect() != true at Bot().");
            }
        }
    }
}