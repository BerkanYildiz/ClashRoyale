namespace ClashRoyale.Client.Network
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Timers;

    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Network.Packets;
    using ClashRoyale.Client.Network.Packets.Client;
    using ClashRoyale.Crypto;
    using ClashRoyale.Crypto.Encrypters;
    using ClashRoyale.Crypto.Inits;
    using ClashRoyale.Extensions;
    using ClashRoyale.Maths;

    internal class NetworkManager
    {
        internal Bot Bot;
        internal LogicLong AccountId;
        internal NetworkGateway Gateway;

        internal int Ping;
        internal int InvalidMessageStateCnt;

        internal string ConnectionInterface;

        internal DateTime LastChatMessage;
        internal DateTime LastKeepAlive;
        internal DateTime LastMessage;
        internal DateTime Session;

        internal PepperInit PepperInit;
        internal IEncrypter SendEncrypter;
        internal IEncrypter ReceiveEncrypter;

        internal IPEndPoint UdpEndPoint;
        internal Timer Timer;

        /// <summary>
        /// Gets the time since last keep alive in ms.
        /// </summary>
        internal long TimeSinceLastKeepAliveMs
        {
            get
            {
                return (long) DateTime.UtcNow.Subtract(this.LastKeepAlive).TotalMilliseconds;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="NetworkManager"/> is connected.
        /// </summary>
        internal bool IsConnected
        {
            get
            {
                return this.Gateway.IsConnected;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManager"/> class.
        /// </summary>
        internal NetworkManager()
        {
            this.Gateway        = new NetworkGateway(this);

            this.Session        = DateTime.UtcNow;
            this.LastMessage    = DateTime.UtcNow;
            this.LastKeepAlive  = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManager"/> class.
        /// </summary>
        /// <param name="Bot">The bot.</param>
        internal NetworkManager(Bot Bot) : this()
        {
            this.Bot            = Bot;
        }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        internal bool TryConnect()
        {
            Logging.Info(this.GetType(), "TryConnect().");

            if (this.Gateway.Interface == AddressFamily.InterNetwork)
            {
                this.ConnectionInterface = "eth0";
            }
            else
            {
                this.ConnectionInterface = "wlan";
            }

            this.Gateway.Connect("game.clashroyaleapp.com");

            if (this.Gateway.IsConnected)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Receives the message.
        /// </summary>
        internal void ReceiveMessage(short Type, short Version, byte[] Encrypted)
        {
            byte[] Packet = null;

            if (this.ReceiveEncrypter == null)
            {
                Logging.Warning(this.GetType(), "[" + this.Bot.BotId + "] this.ReceiveEncrypter == null at ReceiveMessage(" + Type + ").");

                if (this.PepperInit.State == 1)
                {
                    if (Type == 20100)
                    {
                        Logging.Warning(this.GetType(), "[" + this.Bot.BotId + "] Type == 20100 at ReceiveMessage(" + Type + ").");

                        Packet = PepperCrypto.HandlePepperAuthentificationResponse(ref this.PepperInit, Encrypted);
                    }
                    else
                    {
                        Logging.Warning(this.GetType(), "[" + this.Bot.BotId + "] Packet = Encrypted at ReceiveMessage(" + Type + ").");

                        Packet = Encrypted;
                    }
                }
                else
                {
                    if (this.PepperInit.State == 3)
                    {
                        if (Type == 20103 || Type == 22280)
                        {
                            Packet = PepperCrypto.HandlePepperLoginResponse(ref this.PepperInit, Encrypted, out this.SendEncrypter, out this.ReceiveEncrypter);
                        }
                    }
                }
            }
            else
            {
                Packet = this.ReceiveEncrypter.Decrypt(Encrypted);
            }

            if (Packet != null)
            {
                Logging.Info(this.GetType(), "[" + this.Bot.BotId + "] " + Type + " : " + BitConverter.ToString(Packet) + ".");

                using (ByteStream Stream = new ByteStream(Packet))
                {
                    Message Message = Factory.CreateMessage(Type, this.Bot, Stream);

                    if (Message != null)
                    {
                        Logging.Info(this.GetType(), "[" + this.Bot.BotId + "] " + "Received " +  Message.GetType().Name + ".");

                        try
                        {
                            Message.Decode();
                            Message.Process();
                        }
                        catch (Exception Exception)
                        {
                            Logging.Error(this.GetType(), "[" + this.Bot.BotId + "] " + Exception.GetType().Name + " at ReceiveMessage().");
                        }
                    }
                    else
                    {
                        // Logging.Warning(this.GetType(), "[" + this.Bot.BotId + "] Message == null at ReceiveMessage().");
                    }
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "[" + this.Bot.BotId + "] Packet == null at ReceiveMessage().");
            }
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="Message">The message.</param>
        internal void SendMessage(Message Message)
        {
            if (Message.Bot.Network.IsConnected)
            {
                if (Message.IsClientToServerMessage)
                {
                    Message.Encode();

                    byte[] Decrypted = Message.Stream.ToArray();
                    byte[] Encrypted = null;

                    if (this.SendEncrypter != null)
                    {
                        Encrypted = this.SendEncrypter.Encrypt(Decrypted);
                    }
                    else
                    {
                        if (this.PepperInit.State > 0)
                        {
                            if (this.PepperInit.State == 2)
                            {
                                PepperInit.ServerPublicKey = PepperFactory.PublicKey;
                                Encrypted = PepperCrypto.SendPepperLogin(ref this.PepperInit, Decrypted);
                            }
                        }
                        else
                        {
                            Encrypted = PepperCrypto.SendPepperAuthentification(ref this.PepperInit, Decrypted);
                        }
                    }

                    Message.Stream.SetByteArray(Encrypted);

                    this.Gateway.Send(Message);

                    Message.Process();
                }
                else
                {
                    Logging.Warning(this.GetType(), "[" + this.Bot.BotId + "] ClientToServer != true at SendMessage(Message " + Message.Type + ").");
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "[" + this.Bot.BotId + "] IsConnected != true at SendMessage(Message " + Message.Type + ").");
            }
        }

        /// <summary>
        /// Called when a keep alive message has been received.
        /// </summary>
        internal void KeepAliveMessageReceived()
        {
            this.LastKeepAlive = DateTime.UtcNow;
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
                if (this.Bot.IsLogged)
                {
                    this.SendMessage(new KeepAliveMessage(this.Bot));
                }
            };
            this.Timer.Start();
        }
    }
}