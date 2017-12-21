namespace ClashRoyale.Crypto
{
    using System.Collections.Generic;

    using ClashRoyale.Crypto.Nacl;
    using ClashRoyale.Extensions;

    public static class PepperFactory
    {
        /// <summary>
        /// Gets the latest supercell server public key for the <see cref="PepperCrypto"/>.
        /// </summary>
        public static byte[] PublicKey
        {
            get
            {
                return PepperFactory.PublicKeys[15];
            }
        }

        /// <summary>
        /// Gets the latest private server secret key for the <see cref="PepperCrypto"/>,
        /// and generates a public key with it.
        /// </summary>
        public static byte[] ModdedPublicKey
        {
            get
            {
                byte[] k = new byte[32];
                Curve25519Xsalsa20Poly1305.CryptoBoxGetpublickey(k, PepperFactory.SecretKeys[14]);
                return k;
            }
        }

        public static readonly Dictionary<int, byte[]> SecretKeys = new Dictionary<int, byte[]>
        {
            {
                14, "98 0C F7 BB 72 62 B3 86 FE A6 10 34 AB A7 37 06 13 62 79 19 66 6B 34 E6 EC F6 63 07 A3 81 DD 61".HexaToBytes()
            }
        };

        public static readonly Dictionary<int, byte[]> PublicKeys = new Dictionary<int, byte[]>
        {
            {
                15, "99 b6 18 76 f3 ff 18 ca ec a0 ae c1 f3 26 d9 98 1b bc af 64 e7 da a3 17 a7 f1 09 66 86 7a f9 68".HexaToBytes()
            }
        };
    }
}