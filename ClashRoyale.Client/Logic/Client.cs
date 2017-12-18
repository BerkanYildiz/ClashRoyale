namespace ClashRoyale.Client.Logic
{
    using System;
    using System.Diagnostics;
    using System.Net.Sockets;
    using System.Threading;

    using ClashRoyale.Client.Core.Network;
    using ClashRoyale.Client.Logic.Enums;
    using ClashRoyale.Client.Packets.Crypto;
    using ClashRoyale.Client.Packets.Messages.Client;

    using Timer = System.Timers.Timer;

    internal class Client
    {
        internal Socket Socket;
        internal Device Device;
        internal Timer Timer;
        internal Gateway Gateway;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        internal Client()
        {
            this.Socket     = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.Device     = new Device(this.Socket, this);
            this.Gateway    = new Gateway(this.Device);
            
            Debug.WriteLine("[*] Connecting to game.clashroyaleapp.com on port 9339...");

            try
            {
                this.Socket.BeginConnect("game.clashroyaleapp.com", 9339, ar =>
                {
                    if (ar.IsCompleted)
                    {                
                        if (this.Socket.Connected)
                        {
                            this.Socket.EndConnect(ar);

                            Debug.WriteLine("[*]  Connected to " + this.Socket.RemoteEndPoint + ".");

                            this.Device.PepperInit.KeyVersion = 15;
                            this.Device.PepperInit.ServerPublicKey = PepperFactory.ServerPublicKeys[this.Device.PepperInit.KeyVersion];

                            this.Gateway.Send(new Pre_Authentification(this.Device, this.Device.PepperInit.KeyVersion));
                            this.Gateway.Receive();

                            // Task.Run(() => this.LetsCheat());
                        }
                        else
                        {
                            Debug.WriteLine("[*] Warning : We are not connected to the game server.");
                        }
                    }
                }, string.Empty);
            }
            catch
            {
                Debug.WriteLine("[*] Warning : Unable to connect to game server.");
            }
        }

        /// <summary>
        /// Lets cheat and win gold + experience.
        /// </summary>
        internal void LetsCheat()
        {
            while (this.Device.Connected)
            {
                this.Device.Crypto = new Rjindael();
                Thread.Sleep(100);

                this.Gateway.Send(new Authentification(this.Device));
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Keeps the alive.
        /// </summary>
        internal void KeepAlive()
        {
            this.Timer              = new Timer();
            this.Timer.AutoReset    = true;
            this.Timer.Interval     = TimeSpan.FromSeconds(3).TotalMilliseconds;
            this.Timer.Elapsed     += (Gobelin, Land) =>
            {
                if (this.Device.Connected)
                {
                    if (this.Device.State >= State.LOGGED)
                    {
                        this.Gateway.Send(new Keep_Alive(this.Device));
                    }
                }
            };
            this.Timer.Start();
        }
    }
}