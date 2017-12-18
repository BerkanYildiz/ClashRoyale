namespace ClashRoyale.Extensions.Helper
{
    using System;

    using ClashRoyale.Files.Csv;

    using Newtonsoft.Json.Linq;

    public class JsonHelper
    {
        public static bool GetJsonArray(JToken Token, string Key, out JArray JArray)
        {
            return (JArray = (JArray) Token[Key]) != null;
        }

        public static bool GetJsonObject(JToken Token, string Key, out JToken JToken)
        {
            return (JToken = Token[Key]) != null;
        }

        public static bool GetJsonString(JToken Token, string Key, out string String)
        {
            return (String = (string) Token[Key]) != null;
        }

        public static bool GetJsonData(JToken Token, string Key, out CsvData CsvData)
        {
            return (CsvData = JsonHelper.GetJsonNumber(Token, Key, out int Id) ? CsvFiles.GetWithGlobalId(Id) : null) != null;
        }

        public static bool GetJsonData<T>(JToken Token, string Key, out T CsvData) where T : CsvData
        {
            return (CsvData = JsonHelper.GetJsonNumber(Token, Key, out int Id) ? CsvFiles.GetWithGlobalId(Id) as T : null) != null;
        }

        public static bool GetJsonBoolean(JToken Token, string Key, out bool Bool)
        {
            JToken KeyValue = Token[Key];

            if (KeyValue != null)
            {
                Bool = (bool) KeyValue;
                return true;
            }
            else
                Bool = false;

            return false;
        }

        public static bool GetJsonNumber(JToken Token, string Key, out int Int)
        {
            JToken KeyValue = Token[Key];

            if (KeyValue != null)
            {
                Int = (int) KeyValue;
                return true;
            }
            else
                Int = 0;

            return false;
        }

        public static bool GetIntArray(JToken Token, string Key, out int[] Array)
        {
            JArray JArray = (JArray) Token[Key];

            if (JArray != null)
            {
                Array = new int[JArray.Count];

                for (int I = 0; I < Array.Length; I++)
                {
                    Array[I] = (int) JArray[I];
                }

                return true;
            }

            Array = null;

            return false;
        }

        public static bool GetJsonDateTime(JToken Token, string Key, out DateTime Time)
        {
            JToken KeyValue = Token[Key];

            if (KeyValue != null)
            {
                Time = (DateTime) KeyValue;
                return true;
            }
            else
                Time = DateTime.UtcNow;

            return false;
        }

        public static void SetIntArray(JObject JObject, string Key, int[] Array)
        {
            JArray JArray = new JArray();

            for (int I = 0; I < Array.Length; I++)
            {
                JArray.Add(Array[I]);
            }

            JObject.Add(Key, JArray);
        }

        public static void SetLogicData(JObject JObject, string Key, CsvData CsvData)
        {
            if (CsvData != null)
            {
                JObject.Add(Key, CsvData.GlobalId);
            }
        }
    }
}