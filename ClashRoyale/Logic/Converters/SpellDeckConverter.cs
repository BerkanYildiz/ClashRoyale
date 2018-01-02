namespace ClashRoyale.Logic.Converters
{
    using System;

    using ClashRoyale.Logic.Home.Spells;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class SpellDeckConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter Writer, object Value, JsonSerializer Serializer)
        {
            SpellDeck Deck = (SpellDeck) Value;

            if (Deck != null)
            {
                Deck.Save().WriteTo(Writer);
            }
            else
                Writer.WriteNull();
        }

        public override object ReadJson(JsonReader Reader, Type ObjectType, object ExistingValue, JsonSerializer Serializer)
        {
            SpellDeck Deck = (SpellDeck) ExistingValue;

            if (Deck == null)
            {
                Deck = new SpellDeck();
            }

            Deck.Load(JArray.Load(Reader));

            return Deck;
        }

        public override bool CanConvert(Type ObjectType)
        {
            return ObjectType == typeof(SpellDeck);
        }
    }
}
