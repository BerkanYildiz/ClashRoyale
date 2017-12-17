namespace ClashRoyale.Server.Logic.Reward
{
    using System.Collections.Generic;

    using ClashRoyale.Server.Crypto.Randomizers;
    using ClashRoyale.Server.Extensions.Game;
    using ClashRoyale.Server.Extensions.Utils;
    using ClashRoyale.Server.Files.Csv;
    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.Home;
    using ClashRoyale.Server.Logic.Home.Spells;

    internal class RewardRandomizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RewardRandomizer"/> class.
        /// </summary>
        public RewardRandomizer()
        {
            // RewardRandomizer.
        }

        /// <summary>
        /// Creates a spell with spell data.
        /// </summary>
        internal static Spell CreateSpell(SpellData Data)
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
        /// Combines a list of spells.
        /// </summary>
        internal static void CombineSpells(List<Spell> Spells, int Count, int[] CountByRarity, XorShift Random, Home Home)
        {
            if (Count > 0)
            {
                if (Spells.Count >= 2)
                {
                    int RndCatchupChance = Random.Next(100);
                    int CatchupChance = Globals.ChestCatchupChance;

                    int Multiplier1 = 500 * Spells.Count;
                    int Multiplier2 = 250 * Spells.Count;
                    int Multiplier3 = 1000 * Spells.Count / 3;

                    int I = 0;

                    while (Spells.Count >= Count)
                    {
                        int Rnd = Random.Next(Spells.Count);
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

                                            if (I >= Multiplier2 | RndCatchupChance >= CatchupChance | LvlIdx1 <= LvlIdx2)
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
        /// Randomizes the reward.
        /// </summary>
        internal static Reward RandomizeReward(Chest Chest, XorShift Random, Home Home)
        {
            TreasureChestData ChestData = Chest.Data;
            Reward Reward = new Reward();

            Reward.Spells = RewardRandomizer.RandomizeSpells(ChestData, Random, Home);

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
                    Reward.Gold = Random.Next(MinGold, MaxGold);
                }
            }

            return Reward;
        }

        /// <summary>
        /// Randomizes spells.
        /// </summary>
        internal static List<Spell> RandomizeSpells(TreasureChestData Data, XorShift Random, Home Home)
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
            CsvTable RaritiesTable = CsvFiles.Get(Gamefile.Rarity);
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
                        
                        if (Random.Next(Chance) < Data.RandomSpellCount % Chance)
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

                    SpellData RandomSpellData = SpellSet.GetRandomSpell(Random, RaritiesTable.GetWithInstanceId<RarityData>(I));

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
                                if (Home.HasSpell(RandomSpellData) ^ true | J - 1 >= 1000)
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

            RewardRandomizer.CombineSpells(Spells, Data.DifferentSpellCount, CountByRarity, Random, Home);

            for (int I = 0; I < Spells.Count; I++)
            {
                // TODO : Implement Sort Spells.
            }

            return Spells;
        }
    }
}