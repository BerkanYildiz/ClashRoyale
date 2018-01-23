namespace ClashRoyale.Network
{
    using System;
    using System.Net;

    using ClashRoyale.Crypto;
    using ClashRoyale.Crypto.Encrypters;
    using ClashRoyale.Crypto.Inits;
    using ClashRoyale.Enums;
    using ClashRoyale.Exceptions;
    using ClashRoyale.Extensions;
    using ClashRoyale.Handlers;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Structures;
    using ClashRoyale.Maths;
    using ClashRoyale.Messages;

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

            this.SendEncrypter    = new Rc4Encrypter("fhsd6f86f67rt8fw78fw789we78r9789wer6re", "nonce");
            this.ReceiveEncrypter = new Rc4Encrypter("fhsd6f86f67rt8fw78fw789we78r9789wer6re", "nonce");
        }

        /// <summary>
        /// Receives the message.
        /// </summary>
        public void ReceiveMessage(short type, short version, byte[] packet)
        {
            packet = this.ReceiveEncrypter.Decrypt(packet);

            if (packet != null)
            {
                if (this.Device.State != State.Logged)
                {
                    if (type != 10100 && type != 10101)
                    {
                        if (++this.InvalidMessageStateCnt >= 5)
                        {
                            NetworkTcp.Disconnect(this.Device.Token.AsyncEvent);
                        }

                        return;
                    }
                }

                using (ByteStream stream = new ByteStream(packet))
                {
                    Message message = MessageFactory.CreateMessage(type, stream);

                    if (message != null)
                    {
                        Logging.Info(this.GetType(), "Receiving " + message.GetType().Name + ".");

                        if (this.RequestTime.CanHandleMessage(message))
                        {
                            try
                            {
                                message.Decode();
                            }
                            catch (Exception Exception)
                            {
                                Logging.Error(this.GetType(), "ReceiveMessage() - An error has been throwed when the message type " + message.Type + " has been processed. " + Exception);
                            }

                            HandlerFactory.MessageHandle(this.Device, message); // TODO : Probably call Task.Wait().
                        }
                    }
                    else
                    {
                        Logging.Info(this.GetType(), BitConverter.ToString(stream.ReadBytes(stream.BytesLeft)));
                    }
                }
            }
            else
            {
                if (this.Device.State == State.Logged)
                {
                    Logging.Error(this.GetType(), "packet == null at ReceiveMessage().");
                }
            }
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="Message">The message.</param>
        public void SendMessage(Message Message)
        {
            Logging.Info(typeof(NetworkTcp), "Sending " + Message.GetType().Name + ".");

            if (this.Device.Token.IsConnected)
            {
                if (Message.IsServerToClientMessage)
                {
                    Message.Encode();

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

                    NetworkTcp.Send(this.WriteHeader(Message), this.Device.Token);
                    HandlerFactory.MessageHandle(this.Device, Message); // TODO : Probably call Task.Wait().
                }
                else
                {
                    Logging.Error(this.GetType(), "Message.IsServerToClientMessage != true at SendMessage(Message " + Message.Type + ").");
                }
            }
        }

        /// <summary>
        ///     Writes the header of message.
        /// </summary>
        public byte[] WriteHeader(Message message)
        {
            byte[] stream = message.GetBytes();
            byte[] packet = new byte[7 + stream.Length];
            int messageLength = stream.Length;
            int messageType = message.Type;
            int messageVersion = message.Version;

            if (messageLength > 0xFFFFFF)
            {
                throw new LogicException(this.GetType(), "Message is too big. length: " + messageLength);
            }

            Array.Copy(stream, 0, packet, 7, messageLength);

            packet[1] = (byte) (messageType);
            packet[0] = (byte) (messageType >> 8);

            packet[4] = (byte) (messageLength);
            packet[3] = (byte) (messageLength >> 8);
            packet[2] = (byte) (messageLength >> 16);

            packet[6] = (byte) (messageVersion);
            packet[5] = (byte) (messageVersion >> 8);

            return packet;
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