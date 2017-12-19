namespace ClashRoyale.Crypto
{
    using System.Collections.Generic;

    using ClashRoyale.Crypto.Nacl;
    using ClashRoyale.Extensions;

    public static class PepperFactory
    {
        public static readonly byte[] SupercellServerPublicKey =
        {
            0x99, 0xb6, 0x18, 0x76, 0xf3, 0xff, 0x18, 0xca, 0xec, 0xa0, 0xae, 0xc1, 0xf3, 0x26, 0xd9, 0x98,
            0x1b, 0xbc, 0xaf, 0x64, 0xe7, 0xda, 0xa3, 0x17, 0xa7, 0xf1, 0x09, 0x66, 0x86, 0x7a, 0xf9, 0x68
        };

        public static byte[] ServerPublicKey
        {
            get
            {
                byte[] k = new byte[32];
                Curve25519Xsalsa20Poly1305.CryptoBoxGetpublickey(k, PepperFactory.SupercellServerPublicKey);
                return k;
            }
        }

        public static readonly Dictionary<int, byte[]> ServerSecretKeys = new Dictionary<int, byte[]>
        {
            {
                14, "98 0C F7 BB 72 62 B3 86 FE A6 10 34 AB A7 37 06 13 62 79 19 66 6B 34 E6 EC F6 63 07 A3 81 DD 61".HexaToBytes()
            },
            {
                15, "99 b6 18 76 f3 ff 18 ca ec a0 ae c1 f3 26 d9 98 1b bc af 64 e7 da a3 17 a7 f1 09 66 86 7a f9 68".HexaToBytes()
            }
        };
    }
}