namespace ClashRoyale.Network
{
    using System;
    using System.Net;
    using System.Timers;

    using ClashRoyale.Crypto;
    using ClashRoyale.Crypto.Encrypters;
    using ClashRoyale.Crypto.Inits;
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Maths;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Account;

    public class NetworkManager
    {
        public Device Device;
        public LogicLong AccountId;

        public int Ping;
        public int InvalidMessageStateCnt;

        public string Interface;

        public DateTime LastChatMessage;
        public DateTime LastKeepAlive;
        public DateTime LastMessage;
        public DateTime Session;

        public PepperInit PepperInit;
        public IEncrypter SendEncrypter;
        public IEncrypter ReceiveEncrypter;

        public IPEndPoint UdpEndPoint;
        public Timer Timer;

        /// <summary>
        /// Gets the time since last keep alive in ms.
        /// </summary>
        public long TimeSinceLastKeepAliveMs
        {
            get
            {
                return (long) DateTime.UtcNow.Subtract(this.LastKeepAlive).TotalMilliseconds;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManager"/> class.
        /// </summary>
        public NetworkManager()
        {
            this.Session        = DateTime.UtcNow;
            this.LastMessage    = DateTime.UtcNow;
            this.LastKeepAlive  = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManager"/> class.
        /// </summary>
        /// <param name="Device">The bot.</param>
        public NetworkManager(Device Device) : this()
        {
            this.Device         = Device;
        }

        /// <summary>
        /// Receives the message.
        /// </summary>
        public void ReceiveMessage(short Type, short Version, byte[] Encrypted)
        {
            Logging.Info(this.GetType(), "ReceiveMessage(" + Type + ", " + Version + ", null);");

            byte[] Packet = null;

            if (this.ReceiveEncrypter == null)
            {
                if (this.PepperInit.State == 1)
                {
                    if (Type == 20100)
                    {
                        Packet = PepperCrypto.HandlePepperAuthentificationResponse(ref this.PepperInit, Encrypted);
                    }
                    else
                    {
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
                using (ByteStream Stream = new ByteStream(Packet))
                {
                    Message Message = MessageFactory.CreateMessage(Type, Stream);

                    if (Message != null)
                    {
                        Logging.Info(this.GetType(), "Receiving " + Message.GetType().Name + ".");

                        try
                        {
                            Message.Decode();
                        }
                        catch (Exception Exception)
                        {
                            Logging.Error(this.GetType(), "ReceiveMessage() - An error has been throwed when the message type " + Message.Type + " has been processed. " + Exception);
                        }

                        Handlers.Handlers.MessageHandle(this.Device, Message).ConfigureAwait(false);
                    }
                    else
                    {
                        Logging.Info(this.GetType(), BitConverter.ToString(Stream.ReadBytesWithoutLength(Stream.BytesLeft)));
                    }
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "Packet == null at ReceiveMessage().");
            }
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="Message">The message.</param>
        public void SendMessage(Message Message)
        {
            if (this.Device.Token.IsConnected)
            {
                if (Message.IsServerToClientMessage == false)
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
                                this.PepperInit.ServerPublicKey = PepperFactory.PublicKey;
                                Encrypted = PepperCrypto.SendPepperLogin(ref this.PepperInit, Decrypted);
                            }
                        }
                        else
                        {
                            Encrypted = PepperCrypto.SendPepperAuthentification(ref this.PepperInit, Decrypted);
                        }
                    }

                    Message.Stream.SetByteArray(Encrypted);

                    NetworkTcp.Send(Message.ToBytes, this.Device.Token);
                }
                else
                {
                    Logging.Warning(this.GetType(), "ClientToServer != false at SendMessage(Message " + Message.Type + ").");
                }
            }
            // else
            {
                // Logging.Warning(this.GetType(), "[" + this.Device.BotId + "] IsConnected != true at SendMessage(Message " + Message.Type + ").");
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
            this.Timer              = new Timer();
            this.Timer.AutoReset    = true;
            this.Timer.Interval     = TimeSpan.FromSeconds(5).TotalMilliseconds;
            this.Timer.Elapsed     += (Gobelin, Land) =>
            {
                if (this.Device.State >= State.Login)
                {
                    this.SendMessage(new KeepAliveMessage());
                }
            };
            this.Timer.Start();
        }
    }
}