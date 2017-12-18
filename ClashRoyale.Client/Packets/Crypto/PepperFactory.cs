namespace ClashRoyale.Client.Packets.Crypto
{
    using System.Collections.Generic;

    internal static class PepperFactory
    {
        internal static readonly Dictionary<int, byte[]> ServerPublicKeys = new Dictionary<int, byte[]>
        {
            {
                14,
                // "4A EA 1B 14 EF 80 0C 29 EF 58 F7 D5 36 B7 9D AD 7A 3D 4A C2 C4 CE C8 F1 AB E4 15 05 13 74 11 55".HexaToBytes() // GL Server PK
                "98 0C F7 BB 72 62 B3 86 FE A6 10 34 AB A7 37 06 13 62 79 19 66 6B 34 E6 EC F6 63 07 A3 81 DD 61".HexaToBytes() // SC Server Pk  2.0.5
            },
            {
                15,
                "99 b6 18 76 f3 ff 18 ca ec a0 ae c1 f3 26 d9 98 1b bc af 64 e7 da a3 17 a7 f1 09 66 86 7a f9 68".HexaToBytes() // SC Server Pk  2.1.5
            }
        };
    }
}