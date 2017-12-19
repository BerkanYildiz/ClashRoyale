namespace ClashRoyale.Client.Network
{
    using System;
    using System.Net;

    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Network.Packets;
    using ClashRoyale.Crypto;
    using ClashRoyale.Crypto.Encrypters;
    using ClashRoyale.Crypto.Inits;
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Maths;

    internal class NetworkManager
    {
        internal Device Device;
        internal LogicLong AccountId;

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
        /// Initializes a new instance of the <see cref="NetworkManager"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        internal NetworkManager(Device Device)
        {
            this.Device         = Device;

            this.Session        = DateTime.UtcNow;
            this.LastMessage    = DateTime.UtcNow;
            this.LastKeepAlive  = DateTime.UtcNow;
        }

        /// <summary>
        /// Receives the message.
        /// </summary>
        internal void ReceiveMessage(short Type, short Version, byte[] Encrypted)
        {
            byte[] Packet = null;

            if (this.ReceiveEncrypter == null)
            {
                if (this.PepperInit.State == 1)
                {
                    if (Type == 20100)
                    {
                        Packet = PepperCrypto.HandlePepperAuthentificationResponse(ref this.PepperInit, Encrypted);
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
                    Message Message = Factory.CreateMessage(Type, this.Device, Stream);

                    if (Message != null)
                    {
                        Logging.Info(this.GetType(), "Receiving " + Message.GetType().Name + ".");

                        if (true) // this.RequestTime.CanHandleMessage(Message))
                        {
                            try
                            {
                                Message.Decode();
                                Message.Process();
                            }
                            catch (Exception Exception)
                            {
                                Logging.Error(this.GetType(), "ReceiveMessage() - An error has been throwed when the message type " + Message.Type + " has been processed. " + Exception);
                            }
                        }
                    }
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
        internal void SendMessage(Message Message)
        {
            if (Message.Device.Network.IsConnected)
            {
                if (Message.IsServerToClientMessage)
                {
                    Message.Encode();

                    byte[] Decrypted = Message.Stream.ToArray();
                    byte[] Encrypted = null;

                    if (Device.SendEncrypter != null)
                    {
                        Encrypted = Device.SendEncrypter.Encrypt(Decrypted);
                    }
                    else
                    {
                        if (Device.PepperInit.State > 0)
                        {
                            if (Device.PepperInit.State == 2)
                            {
                                Encrypted = PepperCrypto.SendPepperLogin(ref Device.PepperInit, Decrypted);
                            }
                        }
                        else
                        {
                            Encrypted = PepperCrypto.SendPepperAuthentification(ref Device.PepperInit, Decrypted);
                        }
                    }

                    Message.Stream.SetByteArray(Encrypted);

                    TcpGateway.Send(Message);

                    Message.Process();
                }
                else
                {
                    Logging.Error(this.GetType(), "Message.IsServerToClientMessage != true at SendMessage(Message " + Message.Type + ").");
                }
            }
        }

        /// <summary>
        /// Called when a keep alive message has been received.
        /// </summary>
        internal void KeepAliveMessageReceived()
        {
            this.LastKeepAlive = DateTime.UtcNow;
        }
    }
}