namespace ClashRoyale.Files.Csv
{
    using System;
    using Newtonsoft.Json;

    public class CsvConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter Writer, object Value, JsonSerializer Serializer)
        {
            CsvData CsvData = (CsvData) Value;

            if (CsvData != null)
            {
                Writer.WriteValue(CsvData.GlobalId);
            }
            else
            {
                Writer.WriteValue(0);
            }
        }

        public override object ReadJson(JsonReader Reader, Type ObjectType, object ExistingValue, JsonSerializer Serializer)
        {
            int Id = (int) (long) Reader.Value;

            if (Id != 0)
            {
                CsvData CsvData = CsvFiles.GetWithGlobalId(Id);

                if (ObjectType == typeof(CsvData) || CsvData.GetType() == ObjectType)
                {
                    return CsvData;
                }

                Logging.Error(this.GetType(), "CsvData.GetType() !=  ObjectType. Data:" + CsvData.GetType() + ", objectType:" + ObjectType + ".");
            }

            return null;
        }

        /// <summary>
        ///     Determines whether the specified object type can be converted to <see cref="CsvData" />.
        /// </summary>
        /// <param name="ObjectType">Type of the object.</param>
        public override bool CanConvert(Type ObjectType)
        {
            return ObjectType.BaseType == typeof(CsvData) || ObjectType == typeof(CsvData);
        }
    }
}