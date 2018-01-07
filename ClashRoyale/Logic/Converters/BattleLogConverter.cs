namespace ClashRoyale.Logic.Converters
{
    using System;

    using ClashRoyale.Logic.Battle;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class BattleLogConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter Writer, object Value, JsonSerializer Serializer)
        {
            BattleLog Log = (BattleLog) Value;

            if (Value != null)
            {
                Log.SaveJson(true).WriteTo(Writer);
            }
            else
            {
                Writer.WriteNull();
            }
        }

        public override object ReadJson(JsonReader Reader, Type ObjectType, object ExistingValue, JsonSerializer Serializer)
        {
            BattleLog Log = (BattleLog) ExistingValue;

            if (Log == null)
            {
                Log = new BattleLog();
            }

            Log.LoadJson(JToken.Load(Reader));

            return Log;
        }

        public override bool CanConvert(Type ObjectType)
        {
            return ObjectType == typeof(BattleLog);
        }
    }
}
