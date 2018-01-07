namespace ClashRoyale.Logic.Converters
{
    using System;

    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Logic.Alliance.Stream;
    using ClashRoyale.Logic.Alliance.Stream.Factory;
    using ClashRoyale.Logic.Home.Spells;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class StreamEntryConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter Writer, object Value, JsonSerializer Serializer)
        {
            StreamEntry StreamEntry = (StreamEntry)Value;

            if (StreamEntry != null)
            {
                StreamEntry.Save().WriteTo(Writer);
            }
            else
            {
                Writer.WriteNull();
            }
        }

        public override object ReadJson(JsonReader Reader, Type ObjectType, object ExistingValue, JsonSerializer Serializer)
        {
            JObject Json = JObject.Load(Reader);

            if (JsonHelper.GetJsonNumber(Json, "type", out int Type))
            {
                StreamEntry Entry = StreamEntryFactory.CreateStreamEntryByType(Type);
                Entry.Load(Json);
                return Entry;
            }

            return null;
        }

        public override bool CanConvert(Type ObjectType)
        {
            return ObjectType == typeof(SpellCollection);
        }
    }
}
