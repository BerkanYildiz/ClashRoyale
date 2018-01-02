namespace ClashRoyale.Logic.Converters
{
    using System;

    using ClashRoyale.Logic.Home.Spells;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class SpellCollectionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter Writer, object Value, JsonSerializer Serializer)
        {
            SpellCollection Collection = (SpellCollection) Value;

            if (Collection != null)
            {
                Collection.Save().WriteTo(Writer);
            }
            else
            {
                Writer.WriteNull();
            }
        }

        public override object ReadJson(JsonReader Reader, Type ObjectType, object ExistingValue, JsonSerializer Serializer)
        {
            SpellCollection Deck = (SpellCollection) ExistingValue;

            if (Deck == null)
            {
                Deck = new SpellCollection();
            }

            Deck.Load(JArray.Load(Reader));

            return Deck;
        }

        public override bool CanConvert(Type ObjectType)
        {
            return ObjectType == typeof(SpellCollection);
        }
    }
}
