namespace ClashRoyale.Crypto.Blake
{
    public static class Blake2Builder
    {
        private static readonly Blake2BTreeConfig SequentialTreeConfig = new Blake2BTreeConfig
        {
            IntermediateHashSize = 0, LeafSize = 0, FanOut = 1, MaxHeight = 1
        };

        public static ulong[] ConfigB(Blake2BConfig Config, Blake2BTreeConfig TreeConfig)
        {
            bool IsSequential = TreeConfig == null;
            if (IsSequential)
            {
                TreeConfig = Blake2Builder.SequentialTreeConfig;
            }

            ulong[] RawConfig = new ulong[8];

            RawConfig[0] |= (uint) Config.OutputSize;

            if (Config.Key != null)
            {
                RawConfig[0] |= (uint) Config.Key.Length << 8;
            }

            RawConfig[0] |= (uint) TreeConfig.FanOut << 16;
            RawConfig[0] |= (uint) TreeConfig.MaxHeight << 24;
            RawConfig[0] |= (ulong) (uint) TreeConfig.LeafSize << 32;
            RawConfig[2] |= (uint) TreeConfig.IntermediateHashSize << 8;

            if (Config.Salt != null)
            {
                RawConfig[4] = Blake2BCore.BytesToUInt64(Config.Salt, 0);
                RawConfig[5] = Blake2BCore.BytesToUInt64(Config.Salt, 8);
            }

            if (Config.Personalization != null)
            {
                RawConfig[6] = Blake2BCore.BytesToUInt64(Config.Personalization, 0);
                RawConfig[7] = Blake2BCore.BytesToUInt64(Config.Personalization, 8);
            }

            return RawConfig;
        }

        public static void ConfigBSetNode(ulong[] RawConfig, byte Depth, ulong NodeOffset)
        {
            RawConfig[1] = NodeOffset;
            RawConfig[2] = (RawConfig[2] & ~0xFFul) | Depth;
        }
    }
}