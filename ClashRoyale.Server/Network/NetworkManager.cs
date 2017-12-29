﻿namespace ClashRoyale.Server.Network
{
    using System;
    using System.Net;

    using ClashRoyale.Crypto;
    using ClashRoyale.Crypto.Encrypters;
    using ClashRoyale.Crypto.Inits;
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Maths;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Structures;
    using ClashRoyale.Server.Network.Packets;

    internal class NetworkManager
    {
        internal Device Device;
        internal RequestTime RequestTime;
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
        internal void ReceiveMessage(short Type, short Version, byte[] Packet)
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
                if (this.Device.State != State.Logged)
                {
                    if (Type != 10100 && Type != 10101)
                    {
                        if (++this.InvalidMessageStateCnt >= 5)
                        {
                            TcpGateway.Disconnect(this.Device.Network.AsyncEvent);
                        }

                        return;
                    }
                }

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
                    else
                    {
                        Logging.Info(this.GetType(), BitConverter.ToString(Stream.ReadBytesWithoutLength(Stream.BytesLeft)));
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