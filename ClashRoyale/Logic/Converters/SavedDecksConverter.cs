namespace ClashRoyale.Logic.Converters
{
    using System;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class SavedDecksConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter Writer, object Value, JsonSerializer Serializer)
        {
            int[][] Decks = (int[][]) Value;

            Writer.WriteStartArray();

            if (Decks != null)
            {
                for (int I = 0; I < Decks.Length; I++)
                {
                    Writer.WriteStartArray();

                    for (int J = 0; J < Decks[I].Length; J++)
                    {
                        Writer.WriteValue(Decks[I][J]);
                    }

                    Writer.WriteEndArray();
                }
            }

            Writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader Reader, Type ObjectType, object ExistingValue, JsonSerializer Serializer)
        {
            int[][] Decks = (int[][]) ExistingValue;

            if (Decks == null)
            {
                throw new Exception("SavedDecks is NULL");
            }

            JArray Array = JArray.Load(Reader);

            for (int I = 0; I < Array.Count; I++)
            {
                JArray Array2 = (JArray) Array[I];

                for (int J = 0; J < Array2.Count; J++)
                {
                    Decks[I][J] = (int) Array2[J];
                }
            }

            return Decks;
        }

        public override bool CanConvert(Type ObjectType)
        {
            return ObjectType == typeof(int[][]);
        }
    }
}
