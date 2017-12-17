namespace ClashRoyale.Server.Logic.Alliance.Stream
{
    using System;

    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Logic.Alliance.Stream.Factory;
    using ClashRoyale.Server.Logic.Home.Spells;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal class StreamEntryConverter : JsonConverter
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
