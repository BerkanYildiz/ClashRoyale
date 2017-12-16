namespace ClashRoyale.Server.Logic
{
    using System.Collections.Generic;

    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Game;
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Files.Csv;
    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.Spells;

    internal class Summoner : Character
    {
        internal int ManaCount;
        internal int ManaReserve;
        internal int LastUsedSpellIdx;

        internal bool EncodeDeckDataEnabled;

        internal int[] Hand;

        internal List<int> SpellQueue;
        internal List<int> SpellQueue2;

        internal SpellDeck Deck;
        internal Player Player;

        /// <summary>
        /// Gets the last used spell.
        /// </summary>
        internal Spell LastUsedSpell
        {
            get
            {
                if (this.Deck != null)
                {
                    if (this.LastUsedSpellIdx != -1)
                    {
                        return this.Deck[this.LastUsedSpellIdx];
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Summoner"/> class.
        /// </summary>
        public Summoner(CsvData CsvData) : base(CsvData)
        {
            this.ManaCount          = Globals.StartMana;
            this.ManaReserve        = Globals.MaxMana;

            this.Hand               = new int[4];

            this.SpellQueue         = new List<int>(4);
            this.SpellQueue2        = new List<int>(4);
            this.LastUsedSpellIdx   = -1;
        }

        /// <summary>
        /// Calculates the mana cost of the specified spell.
        /// </summary>
        internal int CalculateManaCost(SpellData Data)
        {
            int Cost = Data.ManaCost;

            if (Data.Mirror)
            {
                Spell LastUsed = this.LastUsedSpell;

                if (LastUsed != null)
                {
                    return Math.Min(LastUsed.Data.ManaCost + 1, Globals.MaxMana);
                }
            }
            else
            {
                if (Data.ManaCostFromSummonerMana)
                {
                    return this.ManaCount;
                }
            }

            return Cost;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.WriteBoolean(this.EncodeDeckDataEnabled);
            Stream.WriteBoolean(false); // ?
            Stream.WriteBoolean(true); // ?

            if (this.EncodeDeckDataEnabled)
            {
                Stream.WriteVInt(4);

                Stream.EncodeConstantSizeIntArray(this.Hand, 4);
                Stream.WriteVInt(this.SpellQueue.Count);
                this.SpellQueue.ForEach(Stream.WriteVInt);
                Stream.WriteVInt(this.SpellQueue2.Count);
                this.SpellQueue2.ForEach(Stream.WriteVInt);

                Stream.WriteVInt(-1);
                Stream.WriteVInt(-1);
                Stream.WriteVInt(0);

                if (Globals.QuestsEnabled)
                {
                    Stream.WriteVInt(0); // Count
                    // LogicQuestManager::encode()
                }
            }

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(this.ManaCount);

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            // LogicSpellAIBlackboard::encode

            {
                Stream.WriteVInt(0);
                Stream.WriteVInt(0);
            }

            for (int I = 0; I < 8; I++)
            {
                Stream.WriteVInt(-1);
            }

            Stream.WriteBoolean(false);

            {
                // ?
            }

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
        }

        /// <summary>
        /// Gets the specified by idx.
        /// </summary>
        internal Spell GetSpell(int Idx)
        {
            if (this.Deck != null)
            {
                return this.Deck[Idx];
            }

            return null;
        }

        /// <summary>
        /// Uses the specified spell.
        /// </summary>
        internal void UseSpell(Spell Spell, SpellData SpellData, Vector2 Coordinate, int SpellDeckIndex)
        {
            int Cost = this.CalculateManaCost(SpellData);

            if (SpellDeckIndex >= 0 && SpellDeckIndex < 4)
            {
                int Idx = this.Hand[0];

                for (int I = 0; I < 4; I++)
                {
                    Idx += this.Hand[0];

                    if (this.Deck[Idx] == Spell)
                    {
                        goto Execute;
                    }
                }

                Execute:

                if (SpellData.Mirror)
                {
                    Spell LastSpell = this.LastUsedSpell;

                    if (LastSpell != null)
                    {
                        // 
                    }
                    else
                    {
                        Logging.Warning(this.GetType(), "UseSpell() - Mirror spell used as a first spell.");
                        return;
                    }
                }
            }

            // NOT FINISHED !
        }

        /// <summary>
        /// Uses the specified spell from hand.
        /// </summary>
        internal bool UseSpellFromHand(Spell Spell)
        {
            int Idx = 0;
            int SpellIdx = -1;

            for (int I = 0; I < 4; I++)
            {
                if (this.GetSpell(Idx += this.Hand[I]) == Spell)
                {
                    SpellIdx = I;
                }
            }
            
            if (SpellIdx != -1)
            {
                this.UseSpellFromHand(SpellIdx);
            }

            Logging.Warning(this.GetType(), "UseSpellFromHand() - annot find spell from summoner hand!");

            return false;
        }

        /// <summary>
        /// Uses the specified spell from hand.
        /// </summary>
        internal bool UseSpellFromHand(int Idx)
        {
            if (Idx != -1)
            {
                this.SpellQueue.Add(this.Hand[Idx]);
                this.SpellQueue.RemoveAt(0);
            }
            else
            {
                if (this.Deck != null)
                {
                    Logging.Error(this.GetType(), "UseSpellFromHand() - Cant use spell from hand!");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Sets the player.
        /// </summary>
        internal void SetPlayer(Player Player, SpellDeck Deck, bool Npc)
        {
            if (!Npc)
            {
                this.Deck = Deck;
                this.Player = Player;

                int SpellCnt = Deck.SpellCount;
                byte[] Tmp = new byte[SpellCnt];
                
                for (byte I = 0; I < SpellCnt; I++)
                {
                    Tmp[I] = I;
                }

                int N = SpellCnt;

                while (N > 1)
                {
                    int K = Program.Random.Next(N + 1);
                    byte Value = Tmp[K];
                    Tmp[K] = Tmp[N];
                    Tmp[N] = Value;

                    --N;
                }

                int Cnt = Math.Min(4, SpellCnt);

                for (int I = 0; I < Cnt; I++)
                {
                    this.Hand[I] = Tmp[I];

                    if (I > 0)
                    {
                        this.Hand[I] -= Tmp[I - 1];
                    }
                }
                
                for (int I = Cnt; I < SpellCnt; I++)
                {
                    this.SpellQueue.Add(Tmp[I]);
                }

                // TODO : Mike, check this

                int Idx = 0;

                for (int I = 0; I < SpellCnt; I++)
                {
                    Idx += this.Hand[I];
                }
            }
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        internal void Tick()
        {
            // Tick.
        }
    }
}