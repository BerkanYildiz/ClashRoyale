namespace ClashRoyale.Client.Logic.Slots
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using ClashRoyale.Client.Logic.Enums;
    using ClashRoyale.Client.Packets.Messages.Client;

    internal class Bots : List<Client>
    {
        internal Thread KeepAliveThread;
        internal List<Thread> CustomActionThreads;

        /// <summary>
        /// Gets the total logged client.
        /// </summary>
        internal int Connected
        {
            get
            {
                int Count = 0;

                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Device.Connected)
                    {
                        if (this[i].Device.State == State.LOGGED)
                        {
                            ++Count;
                        }
                    }
                }

                return Count;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bots"/> class.
        /// </summary>
        public Bots() : base(50000)
        {
            this.KeepAliveThread = new Thread(() =>
            {
                int SleepTime = 5000;
                DateTime Start = DateTime.UtcNow;

                while (true)
                {
                    if (SleepTime > 0)
                    {
                        Thread.Sleep(SleepTime);
                    }

                    Start = DateTime.UtcNow;

                    this.ExecuteAction(Client =>
                    {
                        Client.Gateway.Send(new Keep_Alive(Client.Device));
                    });

                    SleepTime = 5000 - (int) DateTime.UtcNow.Subtract(Start).TotalMilliseconds;
                }
            });

            this.CustomActionThreads = new List<Thread>(4);

            for (int i = 0; i < 3; i++)
            {
                int ThreadIndex = i;

                this.CustomActionThreads.Add(new Thread(() =>
                {
                    int SleepTime = 500;

                    DateTime Start = DateTime.UtcNow;

                    while (true)
                    {
                        if (SleepTime > 0)
                        {
                            Thread.Sleep(SleepTime);
                        }
                        
                        Start = DateTime.UtcNow;
                        this.DownAction(ThreadIndex);
                        SleepTime = 500 - (int) DateTime.UtcNow.Subtract(Start).TotalMilliseconds;
                    }
                }));

                this.CustomActionThreads[i].Start();
            }

            this.KeepAliveThread.Start();
        }

        /// <summary>
        /// Creates the specified number of bot.
        /// </summary>
        internal void CreateBots(int Count)
        {
            for (int i = 0; i < Count; i++)
            {
                this.Add(new Client());
            }

            Console.WriteLine(Count + " Clients Created!");
        }

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        internal void ExecuteAction(Action<Client> Action)
        {
            int Count = this.Count;
            int TotalAction = 0;

            for (int i = 0; i < Count; i++)
            {
                if (this[i].Device.Connected)
                {
                    if (this[i].Device.State == State.LOGGED)
                    {
                        ++TotalAction;

                        Action(this[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        internal void ExecuteAction(Action<Client> Action, int StartOffset, int Count)
        {
            int TotalAction = 0;

            for (int i = StartOffset; i < Count; i++)
            {
                if (this[i].Device.Connected)
                {
                    if (this[i].Device.State == State.LOGGED)
                    {
                        ++TotalAction;

                        Action(this[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Executes the down action.
        /// </summary>
        internal void DownAction(int ThreadIndex)
        {
            int TotalClient = this.Count;
            int StartOffset = 1500 * ThreadIndex;
            int Count = 0;

            if (ThreadIndex <= this.CustomActionThreads.Count - 1)
            {
                Count = TotalClient - StartOffset;
            }
            else
            {
                Count = Math.Min(TotalClient - StartOffset, 1500);
            }

            if (Count > 0)
            {
                this.ExecuteAction(Client =>
                {
                    Client.Gateway.Send(new Keep_Alive(Client.Device));
                    // Client.Gateway.Send(new Ask_For_Joinable_Alliances(Client.Device));
                    Client.Gateway.Send(new Go_Home(Client.Device));
                    Client.Gateway.Send(new Client_Capabilities(Client.Device));
                    Client.Gateway.Send(new Go_Home(Client.Device));
                }, StartOffset, Count);
            }
        }
    }
}