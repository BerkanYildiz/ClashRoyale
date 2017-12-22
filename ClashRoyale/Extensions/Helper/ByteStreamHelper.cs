namespace ClashRoyale.Extensions.Helper
{
    using System;
    using System.Collections.Generic;

    using ClashRoyale.Files.Csv;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Maths;

    public static class StreamHelper
    {
        /// <summary>
        /// Adds a data logic long.
        /// </summary>
        public static void WriteLogicLong(this ChecksumEncoder Stream, int High, int Low)
        {
            Stream.WriteVInt(High);
            Stream.WriteVInt(Low);
        }

        /// <summary>
        /// Adds a <see cref="List{T}"/> of int.
        /// </summary>
        public static void EncodeIntList(this ChecksumEncoder Stream, List<int> List)
        {
            Stream.WriteVInt(List.Count);
            List.ForEach(Stream.WriteVInt);
        }

        /// <summary>
        /// Adds a data data reference.
        /// </summary>
        public static void EncodeData(this ChecksumEncoder Stream, CsvData CsvData)
        {
            if (CsvData != null)
            {
                Stream.WriteVInt(CsvData.Type);
                Stream.WriteVInt(CsvData.Instance);
            }
            else
            {
                Stream.WriteVInt(0);
            }
        }

        /// <summary>
        /// Adds a constant size int array.
        /// </summary>
        public static void EncodeConstantSizeIntArray(this ChecksumEncoder Stream, int[] Array, int Size)
        {
            for (int I = 0; I < Size; I++)
            {
                Stream.WriteVInt(Array[I]);
            }
        }

        /// <summary>
        /// Adds a data data reference.
        /// </summary>
        public static void EncodeLogicData(this ChecksumEncoder Stream, CsvData CsvData, int BaseDataType)
        {
            if (CsvData != null)
            {
                int Id = 1;

                for (int I = 0; I < CsvData.Type - BaseDataType; I++)
                {
                    Id += CsvFiles.Get(BaseDataType + I).Datas.Count;
                }

                Stream.WriteVInt(Id + CsvData.Instance);
            }
            else
            {
                Stream.WriteVInt(0);
            }
        }

        /// <summary>
        /// Encodes a logic long.
        /// </summary>
        public static void EncodeLogicLong(this ChecksumEncoder Stream, LogicLong Long)
        {
            Long.Encode(Stream);
        }

        /// <summary>
        /// Encodes a collection of spells.
        /// </summary>
        public static void EncodeSpellList(this ChecksumEncoder Stream, List<SpellData> Spells)
        {
            if (Spells.Count < 200)
            {
                Stream.WriteVInt(Spells.Count);
                Spells.ForEach(Stream.EncodeData);
            }
            else
            {
                Stream.WriteVInt(0);
            }
        }

        /// <summary>
        /// Decodes a <see cref="List{T}"/> of int.
        /// </summary>
        public static CsvData DecodeIntList(this ByteStream Stream, ref List<int> List)
        {
            // TODO !

            if (List.Count > 0)
            {
                List.Clear();
            }

            int Count = Stream.ReadVInt();

            for (int I = 0; I < Count; I++)
            {
                List.Add(Stream.ReadVInt());
            }

            return null;
        }

        /// <summary>
        /// Decodes a collection of spells.
        /// </summary>
        public static void DecodeSpellList(this ByteStream Stream, ref List<SpellData> Spells)
        {
            int Count = Stream.ReadVInt();
            
            if (Count > -1)
            {
                if (Count >= 200)
                {
                    throw new Exception("DecodeSpellList: List size too big. (" + Count + ")");
                }

                Spells = new List<SpellData>(Count);

                for (int I = 0; I < Count; I++)
                {
                    Spells.Add(Stream.DecodeData<SpellData>());
                }
            }
        }

        /// <summary>
        /// Reads a data reference.
        /// </summary>
        public static CsvData DecodeData(this ByteStream Stream)
        {
            int Type = Stream.ReadVInt();

            if (Type > 0)
            {
                CsvTable Table = CsvFiles.Get(Type);

                if (Table != null)
                {
                    return Table.GetWithInstanceId(Stream.ReadVInt());
                }

                Logging.Error(typeof(StreamHelper), "ReadData() - Table " + Type + " doesn't exists.");
            }

            return null;
        }

        /// <summary>
        /// Reads a data reference.
        /// </summary>
        public static T DecodeData<T>(this ByteStream Stream) where T : CsvData
        {
            int Type = Stream.ReadVInt();

            if (Type > 0)
            {
                CsvTable Table = CsvFiles.Get(Type);

                if (Table != null)
                {
                    return Table.GetWithInstanceId(Stream.ReadVInt()) as T;
                }

                Logging.Error(typeof(StreamHelper), "ReadData() - Table " + Type + " doesn't exists.");
            }

            return null;
        }

        /// <summary>
        /// Reads a data reference.
        /// </summary>
        public static CsvData DecodeLogicData(this ByteStream Stream, int BaseType)
        {
            int Id = Stream.ReadVInt();

            if (Id > 0)
            {
                while (true)
                {
                    CsvTable Table = CsvFiles.Get(BaseType++);

                    if (Id <= Table.Datas.Count)
                    {
                        return Table.GetWithInstanceId(Id - 1);
                    }

                    Id -= Table.Datas.Count;
                }
            }

            return null;
        }

        /// <summary>
        /// Reads a data reference.
        /// </summary>
        public static T DecodeLogicData<T>(this ByteStream Stream, int BaseType) where T : CsvData
        {
            int Id = Stream.ReadVInt();

            if (Id > 0)
            {
                while (true)
                {
                    CsvTable Table = CsvFiles.Get(BaseType++);

                    if (Id <= Table.Datas.Count)
                    {
                        return Table.GetWithInstanceId(Id - 1) as T;
                    }

                    Id -= Table.Datas.Count;
                }
            }

            return null;
        }

        /// <summary>
        /// Reads a data reference.
        /// </summary>
        public static LogicLong DecodeLogicLong(this ByteStream Stream)
        {
            LogicLong LogicLong = new LogicLong();
            LogicLong.Decode(Stream);
            return LogicLong;
        }
    }
}