namespace ClashRoyale.Logic.Converters
{
    using System;

    using ClashRoyale.Logic.Player.Slots;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class CommoditySlotsConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter Writer, object Value, JsonSerializer Serializer)
        {
            CommoditySlots Slots = (CommoditySlots) Value;

            if (Slots != null)
            {
                Slots.Save().WriteTo(Writer);
            }
            else
                Writer.WriteNull();
        }

        public override object ReadJson(JsonReader Reader, Type ObjectType, object ExistingValue, JsonSerializer Serializer)
        {
            CommoditySlots Slots = (CommoditySlots) ExistingValue;

            if (Slots == null)
            {
                Slots = new CommoditySlots();
            }

            Slots.Load(JArray.Load(Reader));

            return Slots;
        }

        public override bool CanConvert(Type ObjectType)
        {
            return ObjectType == typeof(CommoditySlots);
        }
    }
}
