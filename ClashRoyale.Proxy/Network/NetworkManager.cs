namespace ClashRoyale.Network
{
    using System;
    using System.Net;

    using ClashRoyale.Crypto;
    using ClashRoyale.Crypto.Encrypters;
    using ClashRoyale.Crypto.Inits;
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Handlers;
    using ClashRoyale.Logic.Structures;
    using ClashRoyale.Maths;
    using ClashRoyale.Messages;

    using Device = ClashRoyale.Logic.Device;

    public class NetworkManager
    {
        public Device Device;
        public RequestTime RequestTime;
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
        /// <param name="Device">The device.</param>
        public NetworkManager(Device Device)
        {
            this.Device         = Device;

            this.Session        = DateTime.UtcNow;
            this.LastMessage    = DateTime.UtcNow;
            this.LastKeepAlive  = DateTime.UtcNow;
        }

        /// <summary>
        /// Receives the message.
        /// </summary>
        public void ReceiveMessage(short Type, short Version, byte[] Packet)
        {
            if (this.ReceiveEncrypter == null)
            {
                if (this.PepperInit.State == 0)
                {
                    if (Type == 10101)
                    {
                        this.SendEncrypter      = new Rc4Encrypter("fhsd6f86f67rt8fw78fw789we78r9789wer6re", "nonce");
                        this.ReceiveEncrypter   = new Rc4Encrypter("fhsd6f86f67rt8fw78fw789we78r9789wer6re", "nonce");

                        Packet = this.ReceiveEncrypter.Decrypt(Packet);
                    }
                    else if (Type == 10100)
                    {
                        Packet = PepperCrypto.HandlePepperAuthentification(ref this.PepperInit, Packet);
                    }
                    else
                    {
                        Packet = null;
                    }
                }
                else
                {
                    if (this.PepperInit.State == 2)
                    {
                        Packet = Type == 10101 ? PepperCrypto.HandlePepperLogin(ref this.PepperInit, Packet) : null;
                    }
                    else
                    {
                        Packet = null;
                    }
                }
            }
            else
            {
                Packet = this.ReceiveEncrypter.Decrypt(Packet);
            }

            if (Packet != null)
            {
                using (ByteStream Stream = new ByteStream(Packet))
                {
                    Message Message = MessageFactory.CreateMessage(Type, Stream);

                    if (Message != null)
                    {
                        Logging.Info(this.GetType(), "Receiving " + Message.GetType().Name + " from " + this.Device.Token.Socket.RemoteEndPoint + ".");

                        try
                        {
                            Message.Decode();
                        }
                        catch (Exception Exception)
                        {
                            Logging.Error(this.GetType(), "ReceiveMessage() - An error has been throwed when the message type " + Message.Type + " has been processed. " + Exception);
                        }

                        HandlerFactory.MessageHandle(this.Device, Message).Wait();
                    }
                    else
                    {
                        // Logging.Info(this.GetType(), BitConverter.ToString(Stream.ReadBytes(Stream.BytesLeft)));

                        Message = new Message(Stream)
                        {
                            _Identifier = Type,
                            Version     = Version
                        };
                    }

                    Logging.Info(this.GetType(), BitConverter.ToString(Message.ToBytes));

                    Device.GameListener.SendMessage(Message);
                }
            }
            else
            {
                if (this.Device.State == State.Logged)
                {
                    Logging.Error(this.GetType(), "Packet == null at ReceiveMessage().");
                }
            }
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="Message">The message.</param>
        public void SendMessage(Message Message)
        {
            Logging.Info(typeof(NetworkTcp), "Sending " + Message.GetType().Name + " to " + this.Device.Token.Socket.RemoteEndPoint + ".");

            if (this.Device.Token.IsConnected)
            {
                // Message.Encode();

                byte[] Bytes = Message.Stream.ToArray();

                if (this.SendEncrypter == null)
                {
                    if (this.PepperInit.State > 0)
                    {
                        if (this.PepperInit.State == 1)
                        {
                            Bytes = PepperCrypto.SendPepperAuthentificationResponse(ref this.PepperInit, Bytes);
                        }
                        else
                        {
                            if (this.PepperInit.State == 3)
                            {
                                Bytes = PepperCrypto.SendPepperLoginResponse(ref this.PepperInit, out this.SendEncrypter, out this.ReceiveEncrypter, Bytes);
                            }
                        }
                    }
                }
                else
                {
                    Bytes = this.SendEncrypter.Encrypt(Bytes);
                }

                Message.Stream.SetByteArray(Bytes);

                Logging.Info(this.GetType(), BitConverter.ToString(Message.ToBytes));

                NetworkTcp.Send(Message.ToBytes, this.Device.Token);
                HandlerFactory.MessageHandle(this.Device, Message).Wait();
            }
        }

        /// <summary>
        /// Called when a keep alive message has been received.
        /// </summary>
        public void KeepAliveMessageReceived()
        {
            this.LastKeepAlive = DateTime.UtcNow;
        }
    }
}