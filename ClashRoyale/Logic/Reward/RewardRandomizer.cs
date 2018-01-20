namespace ClashRoyale.Logic.Reward
{
    using System.Collections.Generic;
    using ClashRoyale.Crypto.Randomizers;
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions.Game;
    using ClashRoyale.Extensions.Utils;
    using ClashRoyale.Files.Csv;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Home;
    using ClashRoyale.Logic.Home.Spells;

    public class RewardRandomizer
    {
        /// <summary>
        ///     Creates a spell with spell data.
        /// </summary>
        public static Spell CreateSpell(SpellData Data)
        {
            if (Data == null)
            {
                return null;
            }

            Spell Spell = new Spell(Data);

            Spell.SetCreateTime(TimeUtil.MinutesSince1970);
            Spell.SetShowNewIcon(true);

            return Spell;
        }

        /// <summary>
        ///     Combines a list of spells.
        /// </summary>
        public static void CombineSpells(List<Spell> Spells, int Count, int[] CountByRarity, Home Home)
        {
            if (Count > 0)
            {
                if (Spells.Count >= 2)
                {
                    int RndCatchupChance = XorShift.Next(100);
                    int CatchupChance = Globals.ChestCatchupChance;

                    int Multiplier1 = 500 * Spells.Count;
                    int Multiplier2 = 250 * Spells.Count;
                    int Multiplier3 = 1000 * Spells.Count / 3;

                    int I = 0;

                    while (Spells.Count >= Count)
                    {
                        int Rnd = XorShift.Next(Spells.Count);
                        Spell Spell = Spells[Rnd];
                        SpellData SpellData = Spell.Data;
                        Spell Existing = Home.GetSpellByData(SpellData);

                        ++I;

                        if (Existing != null)
                        {
                            if (Spells.Count >= 2)
                            {
                                int J = 0;
                                int K = 0;

                                Spell Existing2 = null;

                                while (true)
                                {
                                    K = (Rnd + ++J) % Spells.Count;
                                    Existing2 = Home.GetSpellByData(Spells[K].Data);

                                    if (Existing2 != null)
                                    {
                                        if (Existing != Existing2)
                                        {
                                            int LvlIdx1 = Existing.LevelIndexIfAllMaterialUsed;
                                            int LvlIdx2 = Existing2.LevelIndexIfAllMaterialUsed;

                                            if ((I >= Multiplier2) | (RndCatchupChance >= CatchupChance) | (LvlIdx1 <= LvlIdx2))
                                            {
                                                if (Existing.Data.RarityData == Existing2.Data.RarityData)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (I >= Multiplier3 || Home.LockedSpellCount > 1)
                                        {
                                            if (Existing.Data.RarityData == Spells[K].Data.RarityData)
                                            {
                                                break;
                                            }
                                        }
                                    }

                                    if (J >= Spells.Count)
                                    {
                                        goto End;
                                    }
                                }

                                Spell.AddMaterial(Spells[K].Count);
                                Spells.RemoveAt(K);
                            }
                        }

                        End:

                        if (I >= Multiplier1)
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Randomizes the reward.
        /// </summary>
        public static Reward RandomizeReward(Chest Chest, Home Home)
        {
            TreasureChestData ChestData = Chest.Data;
            Reward Reward = new Reward();

            Reward.Spells = RewardRandomizer.RandomizeSpells(ChestData, Home);

            int MaxGold = ChestData.MaxGold;
            int MinGold = ChestData.MinGold;

            if (MaxGold > 0)
            {
                if (MaxGold == MinGold)
                {
                    Reward.Gold = MaxGold;
                }
                else
                {
                    Reward.Gold = XorShift.Next(MinGold, MaxGold);
                }
            }

            return Reward;
        }

        /// <summary>
        ///     Randomizes spells.
        /// </summary>
        public static List<Spell> RandomizeSpells(TreasureChestData Data, Home Home)
        {
            List<Spell> Spells = new List<Spell>();

            if (Data.GuaranteedSpellsData.Length > 0)
            {
                for (int I = 0; I < Data.GuaranteedSpellsData.Length; I++)
                {
                    Spell Spell = RewardRandomizer.CreateSpell(Data.GuaranteedSpellsData[I]);
                    Spell.SetMaterialCount(1);
                    Spells.Add(Spell);
                }
            }

            int RandomSpellCount = Data.RandomSpellCount;
            SpellSet SpellSet = new SpellSet(Data.ArenaData, null);
            CsvTable RaritiesTable = CsvFiles.Get(Gamefile.Rarities);
            int[] CountByRarity = new int[RaritiesTable.Datas.Count];

            for (int I = 1; I < CountByRarity.Length; I++)
            {
                int Chance = Data.GetChanceForRarity((RarityData) RaritiesTable.Datas[I]);

                if (Chance > 0)
                {
                    if (RandomSpellCount > 0)
                    {
                        int Cnt = Data.RandomSpellCount / Chance;

                        CountByRarity[I] = Cnt;
                        RandomSpellCount -= Cnt;

                        if (XorShift.Next(Chance) < Data.RandomSpellCount % Chance)
                        {
                            ++CountByRarity[I];
                            --RandomSpellCount;
                        }
                    }
                }
            }

            CountByRarity[0] = RandomSpellCount;

            for (int I = 0; I < CountByRarity.Length; I++)
            {
                int J = 0;
                int K = 0;

                while (K < CountByRarity[I])
                {
                    if (J++ >= 5000)
                    {
                        break;
                    }

                    SpellData RandomSpellData = SpellSet.GetRandomSpell(RaritiesTable.GetWithInstanceId<RarityData>(I));

                    if (RandomSpellData != null)
                    {
                        Spell Spell = RewardRandomizer.CreateSpell(RandomSpellData);

                        if (Spells.Count < 1)
                        {
                            Spell.AddMaterial(1);
                            Spells.Add(Spell);

                            ++K;
                        }
                        else
                        {
                            Spell Existing = Spells.Find(T => T.Equals(Spell));

                            if (Existing != null)
                            {
                                if ((Home.HasSpell(RandomSpellData) ^ true) | (J - 1 >= 1000))
                                {
                                    Existing.AddMaterial(1);
                                    ++K;
                                }
                            }
                            else
                            {
                                Spell.AddMaterial(1);
                                Spells.Add(Spell);

                                ++K;
                            }
                        }
                    }
                }
            }

            RewardRandomizer.CombineSpells(Spells, Data.DifferentSpellCount, CountByRarity, Home);

            for (int I = 0; I < Spells.Count; I++)
            {
                // TODO : Implement Sort Spells.
            }

            return Spells;
        }
    }
}