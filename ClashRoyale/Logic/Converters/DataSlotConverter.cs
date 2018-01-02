namespace ClashRoyale.Logic.Converters
{
    using System;

    using ClashRoyale.Logic.Player.Items;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class DataSlotConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter Writer, object Value, JsonSerializer Serializer)
        {
            DataSlot DataSlot = (DataSlot) Value;

            if (DataSlot != null)
            {
                DataSlot.Save().WriteTo(Writer);
            }
            else
            {
                Writer.WriteValue(0);
            }
        }

        public override object ReadJson(JsonReader Reader, Type ObjectType, object ExistingValue, JsonSerializer Serializer)
        {
            DataSlot DataSlot = (DataSlot) ExistingValue;

            if (DataSlot == null)
            {
                DataSlot = new DataSlot();
            }

            DataSlot.Load(JObject.Load(Reader));

            return DataSlot;
        }

        public override bool CanConvert(Type ObjectType)
        {
            return ObjectType.BaseType == typeof(DataSlot) || ObjectType == typeof(DataSlot);
        }
    }
}
