namespace ClashRoyale.Server.Crypto
{
    using System;

    using ClashRoyale.Server.Crypto.Blake;
    using ClashRoyale.Server.Crypto.Encrypters;
    using ClashRoyale.Server.Crypto.Inits;
    using ClashRoyale.Server.Crypto.Nacl;

    public static class PepperCrypto
    {
        internal static byte[] HandlePepperAuthentification(ref PepperInit Init, byte[] Packet)
        {
            ++Init.State;
            return Packet;
        }

        internal static byte[] HandlePepperLogin(ref PepperInit Init, byte[] Packet)
        {
            if (Packet.Length >= 32)
            {
                ++Init.State;

                Array.Copy(Packet, Init.ClientPublicKey = new byte[32], 32);

                Blake2BHasher Blake2B = new Blake2BHasher();

                Blake2B.Update(Init.ClientPublicKey);
                Blake2B.Update(Init.ServerPublicKey);

                byte[] C = new byte[Packet.Length - 16];
                Array.Copy(Packet, 32, C, 16, Packet.Length - 32);

                Curve25519Xsalsa20Poly1305.CryptoBoxBeforenm(Init.SharedKey = new byte[32], Init.ClientPublicKey, PepperFactory.ServerSecretKeys[Init.KeyVersion]);

                if (Curve25519Xsalsa20Poly1305.CryptoBoxAfternm(C, C, Blake2B.Finish(), Init.SharedKey) == 0)
                {
                    byte[] Sk = new byte[24];
                    Array.Copy(C, 32, Sk, 0, 24);

                    for (int I = 0; I < 24; I++)
                    {
                        if (Sk[I] != Init.SessionKey[I])
                        {
                            Logging.Error(typeof(PepperCrypto), "HandlePepperLogin() - Session Key is not valid.");
                            return null;
                        }
                    }

                    Array.Copy(C, 56, Init.Nonce = new byte[24], 0, 24);

                    byte[] Decrypted = new byte[C.Length - 80];
                    Array.Copy(C, 80, Decrypted, 0, Decrypted.Length);
                    return Decrypted;
                }

                Logging.Error(typeof(PepperCrypto), "HandlePepperLogin() - Unable to handle pepper login. curve25519xsalsa20poly1305.crypto_box_afternm != 0.");
            }

            return null;
        }

        /// <summary>
        /// Encryptes the login response message.
        /// </summary>
        internal static byte[] SendPepperLoginResponse(ref PepperInit Init, out IEncrypter SendEncrypter, out IEncrypter ReceiveEncrypter, byte[] Data)
        {
            ++Init.State;

            Blake2BHasher Blake2 = new Blake2BHasher();

            Blake2.Update(Init.Nonce);
            Blake2.Update(Init.ClientPublicKey);
            Blake2.Update(Init.ServerPublicKey);

            byte[] M = new byte[Data.Length + 88];

            byte[] SendNonce = new byte[24];
            byte[] SecretKey = new byte[32];

            Program.Random.NextBytes(SendNonce);
            Program.Random.NextBytes(SecretKey);

            SendEncrypter = new PepperEncrypter(SendNonce, SecretKey);
            ReceiveEncrypter = new PepperEncrypter(Init.Nonce, SecretKey);

            Buffer.BlockCopy(SendNonce, 0, M, 32, 24);
            Buffer.BlockCopy(SecretKey, 0, M, 56, 32);
            Buffer.BlockCopy(Data, 0, M, 88, Data.Length);

            if (Curve25519Xsalsa20Poly1305.CryptoBoxAfternm(M, M, Blake2.Finish(), Init.SharedKey) == 0)
            {
                byte[] Encrypted = new byte[M.Length - 16];
                Buffer.BlockCopy(M, 16, Encrypted, 0, M.Length - 16);
                return Encrypted;
            }

            Logging.Error(typeof(PepperCrypto), "Unable de send pepper login response.");

            return null;
        }

        /// <summary>
        /// Encryptes the authentification message.
        /// </summary>
        internal static byte[] SendPepperAuthentificationResponse(ref PepperInit Init, byte[] Data)
        {
            ++Init.State;
            return Data;
        }

        /// <summary>
        /// Encryptes input with secret box.
        /// </summary>
        internal static byte[] SecretBox(byte[] Input, byte[] Nonce, byte[] SecretKey)
        {
            byte[] C = new byte[Input.Length + 32];
            Array.Copy(Input, 0, C, 32, Input.Length);

            Xsalsa20Poly1305.CryptoSecretbox(C, C, C.Length, Nonce, SecretKey);

            byte[] Output = new byte[C.Length - 16];
            Array.Copy(C, 16, Output, 0, Output.Length);
            return Output;
        }

        /// <summary>
        /// Decyptes input with secret box.
        /// </summary>
        internal static byte[] SecretBoxOpen(byte[] Input, byte[] Nonce, byte[] SecretKey)
        {
            if (Input.Length >= 16)
            {
                byte[] C = new byte[Input.Length + 16];
                Array.Copy(Input, 0, C, 16, Input.Length);

                if (Xsalsa20Poly1305.CryptoSecretboxOpen(C, C, C.Length, Nonce, SecretKey) == 0)
                {
                    byte[] Output = new byte[C.Length - 32];
                    Array.Copy(C, 32, Output, 0, Output.Length);
                    return Output;
                }
            }

            Logging.Warning(typeof(PepperCrypto), "Unable to open secret box.");

            return null;
        }
    }
}