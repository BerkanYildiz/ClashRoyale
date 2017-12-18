namespace ClashRoyale.Client.Packets.Crypto
{
    using System;

    using ClashRoyale.Client.Packets.Crypto.Encrypter;
    using ClashRoyale.Client.Packets.Crypto.Init;

    public static class PepperCrypto
    {
        internal static byte[] SendPepperAuthentification(ref PepperInit Init, byte[] Packet)
        {
            ++Init.State;
            return Packet;
        }

        internal static byte[] HandlePepperAuthentificationResponse(ref PepperInit Init, byte[] Packet)
        {
            ++Init.State;
            return Packet;
        }

        /// <summary>
        /// Sends the pepper login.
        /// </summary>
        internal static byte[] SendPepperLogin(ref PepperInit Init, byte[] Packet)
        {
            ++Init.State;

            Init.Nonce = new byte[24];
            Init.ClientPublicKey = new byte[32];
            Init.ClientSecretKey = new byte[32];

            Array.Copy(Init.ServerPublicKey, Init.ClientPublicKey, 32);

            curve25519xsalsa20poly1305.crypto_box_keypair(Init.ClientPublicKey, Init.ClientSecretKey);

            byte[] Encrypted = new byte[Packet.Length + 32 + 48];

            Array.Copy(Init.SessionKey, 0, Encrypted, 32, 24);
            Array.Copy(Init.Nonce, 0, Encrypted, 32 + 24, 24);
            Array.Copy(Packet, 0, Encrypted, 32 + 48, Packet.Length);

            Blake2BHasher Blake2B = new Blake2BHasher();

            Blake2B.Update(Init.ClientPublicKey);
            Blake2B.Update(Init.ServerPublicKey);

            curve25519xsalsa20poly1305.crypto_box(Encrypted, Encrypted, Blake2B.Finish(), Init.ServerPublicKey, Init.ClientSecretKey);

            Packet = new byte[Encrypted.Length + 16];

            Array.Copy(Init.ClientPublicKey, Packet, 32);
            Array.Copy(Encrypted, 16, Packet, 32, Packet.Length - 32);

            return Packet;
        }

        /// <summary>
        /// Handles the pepper login response.
        /// </summary>
        internal static byte[] HandlePepperLoginResponse(ref PepperInit Init, byte[] Packet, out IEncrypter SendEncrypter, out IEncrypter ReceiveEncrypter)
        {
            ++Init.State;

            byte[] Decrypted = new byte[Packet.Length + 16];

            Array.Copy(Packet, 0, Decrypted, 16, Packet.Length);

            Blake2BHasher Blake2B = new Blake2BHasher();

            Blake2B.Update(Init.Nonce);
            Blake2B.Update(Init.ClientPublicKey);
            Blake2B.Update(Init.ServerPublicKey);

            curve25519xsalsa20poly1305.crypto_box_open(Decrypted, Decrypted, Blake2B.Finish(), Init.ServerPublicKey, Init.ClientSecretKey);

            byte[] SecretKey = new byte[32];
            byte[] ReceiveNonce = new byte[24];

            Packet = new byte[Decrypted.Length - 32 - 56];

            Array.Copy(Decrypted, 32, ReceiveNonce, 0, 24);
            Array.Copy(Decrypted, 32 + 24, SecretKey, 0, 32);
            Array.Copy(Decrypted, 32 + 24 + 32, Packet, 0, Packet.Length);

            SendEncrypter = new PepperEncrypter(Init.Nonce, SecretKey);
            ReceiveEncrypter = new PepperEncrypter(ReceiveNonce, SecretKey);

            return Packet;
        }
            
        /// <summary>
        /// Encryptes input with secret box.
        /// </summary>
        internal static byte[] SecretBox(byte[] Input, byte[] Nonce, byte[] SecretKey)
        {
            byte[] c = new byte[Input.Length + 32];
            Array.Copy(Input, 0, c, 32, Input.Length);

            xsalsa20poly1305.crypto_secretbox(c, c, c.Length, Nonce, SecretKey);

            byte[] output = new byte[c.Length - 16];
            Array.Copy(c, 16, output, 0, output.Length);
            return output;
        }

        /// <summary>
        /// Decyptes input with secret box.
        /// </summary>
        internal static byte[] SecretBoxOpen(byte[] Input, byte[] Nonce, byte[] SecretKey)
        {
            byte[] c = new byte[Input.Length + 16];
            Array.Copy(Input, 0, c, 16, Input.Length);

            xsalsa20poly1305.crypto_secretbox(c, c, c.Length, Nonce, SecretKey);

            byte[] output = new byte[c.Length - 32];
            Array.Copy(c, 32, output, 0, output.Length);
            return output;
        }
    }
}