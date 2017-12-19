namespace ClashRoyale.Client.Logic
{
    using System;
    using System.Threading;

    using ClashRoyale.Client.Network;
    using ClashRoyale.Client.Network.Packets.Client;
    using ClashRoyale.Crypto.Encrypters;
    using ClashRoyale.Crypto.Inits;
    using ClashRoyale.Enums;

    using Timer = System.Timers.Timer;

    internal class Device
    {
        internal int BotId;

        internal NetworkManager NetworkManager;
        internal NetworkToken   Network;

        internal Timer Timer;
        internal PepperInit PepperInit;

        internal IEncrypter SendEncrypter;
        internal IEncrypter ReceiveEncrypter;
        internal State State;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        internal Device(NetworkToken Network)
        {
            this.PepperInit     = new PepperInit();
            this.NetworkManager = new NetworkManager(this);
            this.Network        = Network; 

            this.Network.SetDevice(this);
        }

        /// <summary>
        /// Connects this instance to the official server.
        /// </summary>
        internal void Connect()
        {
            this.NetworkManager.Connect("game.clashroyaleapp.com", 9339);

            if (this.Network.IsConnected)
            {
                this.NetworkManager.SendMessage(new LoginMessage(this));
                this.NetworkManager.Receive();

                while (this.State != State.Logged)
                {
                    Thread.Sleep(100);
                }

                Logging.Info(this.GetType(), "[" + this.BotId + "] Success, the bot is logged.");
            }
            else
            {
                Logging.Info(this.GetType(), "[" + this.BotId + "] Warning, we are not connected to the game server.");
            }
        }

        /// <summary>
        /// Keeps the alive.
        /// </summary>
        internal void KeepAlive()
        {
            this.Timer = new Timer();
            this.Timer.AutoReset = true;
            this.Timer.Interval = TimeSpan.FromSeconds(5).TotalMilliseconds;
            this.Timer.Elapsed += (Gobelin, Land) =>
            {
                if (this.Network.IsConnected)
                {
                    if (this.State >= State.Login)
                    {
                        this.NetworkManager.SendMessage(new KeepAliveMessage(this));
                    }
                }
            };
            this.Timer.Start();
        }
    }
}