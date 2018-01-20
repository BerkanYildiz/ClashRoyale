namespace ClashRoyale.Logic.Reward
{
    using System.Collections.Generic;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Logic.Home.Spells;
    using Newtonsoft.Json.Linq;

    public class Reward
    {
        public int Diamonds;
        public List<Spell> DiscardedSpells;

        public int Gold;
        public List<Spell> Spells;

        /// <summary>
        ///     Gets the type of this reward.
        /// </summary>
        public virtual int Type
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        ///     Decodes this instance.
        /// </summary>
        public virtual void Decode(ByteStream Stream)
        {
            int Count = Stream.ReadVInt();

            if (Count > -1)
            {
                this.Spells = new List<Spell>(Count);

                for (int I = 0; I < Count; I++)
                {
                    Spell Spell = new Spell(null);
                    Spell.Decode(Stream);
                    this.Spells.Add(Spell);
                }
            }

            Count = Stream.ReadVInt();

            if (Count > -1)
            {
                this.DiscardedSpells = new List<Spell>(Count);

                for (int I = 0; I < Count; I++)
                {
                    Spell Spell = new Spell(null);
                    Spell.Decode(Stream);
                    this.DiscardedSpells.Add(Spell);
                }
            }

            this.Gold = Stream.ReadVInt();
            this.Diamonds = Stream.ReadVInt();
        }

        /// <summary>
        ///     Encodes this instance.
        /// </summary>
        public virtual void Encode(ChecksumEncoder Stream)
        {
            Stream.EnableCheckSum(false);

            if (this.Spells != null)
            {
                Stream.WriteVInt(this.Spells.Count);

                this.Spells.ForEach(Spell => { Spell.Encode(Stream); });
            }
            else
            {
                Stream.WriteVInt(-1);
            }

            if (this.DiscardedSpells != null)
            {
                Stream.WriteVInt(this.DiscardedSpells.Count);

                this.DiscardedSpells.ForEach(Spell => { Spell.Encode(Stream); });
            }
            else
            {
                Stream.WriteVInt(-1);
            }

            Stream.WriteVInt(this.Gold);
            Stream.WriteVInt(this.Diamonds);

            Stream.EnableCheckSum(true);
        }

        /// <summary>
        ///     Creates the specified reward type.
        /// </summary>
        public static Reward CreateFromType(int Type)
        {
            switch (Type)
            {
                case 0:
                {
                    return new Reward();
                }
                case 1:
                {
                    return new DraftReward();
                }
            }

            Logging.Error(typeof(Reward), "CreateFromType() - invalid reward type.");

            return null;
        }

        /// <summary>
        ///     Decodes the reward.
        /// </summary>
        public static Reward DecodeReward(ByteStream Stream)
        {
            Reward Reward = Reward.CreateFromType(Stream.ReadVInt());

            if (Reward != null)
            {
                Reward.Decode(Stream);
            }

            return Reward;
        }

        /// <summary>
        ///     Encodes the reward.
        /// </summary>
        public static void EncodeReward(ChecksumEncoder Stream, Reward Reward)
        {
            Stream.WriteVInt(Reward.Type);
            Reward.Encode(Stream);
        }

        /// <summary>
        ///     Saves this instance to json.
        /// </summary>
        public JObject Save()
        {
            JObject Json = new JObject();
            JArray Spells = new JArray();
            JArray DiscardedSpells = new JArray();

            for (int I = 0; I < this.Spells.Count; I++)
            {
                Spells.Add(this.Spells[I]);
            }

            for (int I = 0; I < this.DiscardedSpells.Count; I++)
            {
                DiscardedSpells.Add(this.DiscardedSpells[I]);
            }

            Json.Add("spells", Spells);
            Json.Add("discarded_spells", DiscardedSpells);

            if (this.Gold > 0)
            {
                Json.Add("gold", this.Gold);
            }

            if (this.Diamonds > 0)
            {
                Json.Add("diamonds");
            }

            if (false)
            {
                JsonHelper.SetLogicData(Json, "skin", null);
            }

            return Json;
        }
    }
}