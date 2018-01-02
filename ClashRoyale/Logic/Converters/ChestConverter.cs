namespace ClashRoyale.Logic.Converters
{
    using System;

    using ClashRoyale.Logic.Home;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class ChestConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter Writer, object Value, JsonSerializer Serializer)
        {
            Chest Chest = (Chest) Value;

            if (Chest != null)
            {
                Chest.Save().WriteTo(Writer);
            }
            else
                Writer.WriteNull();
        }

        public override object ReadJson(JsonReader Reader, Type ObjectType, object ExistingValue, JsonSerializer Serializer)
        {
            Chest Chest = (Chest) ExistingValue;

            if (Chest == null)
            {
                Chest = new Chest();
            }

            Chest.Load(JObject.Load(Reader));

            return Chest;
        }

        public override bool CanConvert(Type ObjectType)
        {
            return ObjectType == typeof(Chest);
        }
    }
}
