namespace ClashRoyale.Server.Extensions.Helper
{
    using System;
    using System.Collections.Generic;

    using ClashRoyale.Server.Files.Csv;
    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Math;

    public static class ByteStreamHelper
    {
        /// <summary>
        /// Adds a data logic long.
        /// </summary>
        internal static void WriteLogicLong(this ChecksumEncoder ByteStream, int High, int Low)
        {
            ByteStream.WriteVInt(High);
            ByteStream.WriteVInt(Low);
        }

        /// <summary>
        /// Adds a <see cref="List{T}"/> of int.
        /// </summary>
        internal static void EncodeIntList(this ChecksumEncoder ByteStream, List<int> List)
        {
            ByteStream.WriteVInt(List.Count);
            List.ForEach(ByteStream.WriteVInt);
        }

        /// <summary>
        /// Adds a data data reference.
        /// </summary>
        internal static void EncodeData(this ChecksumEncoder ByteStream, CsvData CsvData)
        {
            if (CsvData != null)
            {
                ByteStream.WriteVInt(CsvData.Type);
                ByteStream.WriteVInt(CsvData.Instance);
            }
            else
            {
                ByteStream.WriteVInt(0);
            }
        }

        /// <summary>
        /// Adds a constant size int array.
        /// </summary>
        internal static void EncodeConstantSizeIntArray(this ChecksumEncoder ByteStream, int[] Array, int Size)
        {
            for (int I = 0; I < Size; I++)
            {
                ByteStream.WriteVInt(Array[I]);
            }
        }
        
        /// <summary>
        /// Adds a data data reference.
        /// </summary>
        internal static void EncodeLogicData(this ChecksumEncoder ByteStream, CsvData CsvData, int BaseDataType)
        {
            if (CsvData != null)
            {
                int Id = 1;

                for (int I = 0; I < CsvData.Type - BaseDataType; I++)
                {
                    Id += CsvFiles.Get(BaseDataType + I).Datas.Count;
                }

                ByteStream.WriteVInt(Id + CsvData.Instance);
            }
            else
            {
                ByteStream.WriteVInt(0);
            }
        }

        /// <summary>
        /// Encodes a logic long.
        /// </summary>
        internal static void EncodeLogicLong(this ChecksumEncoder ByteStream, LogicLong Long)
        {
            Long.Encode(ByteStream);
        }

        /// <summary>
        /// Encodes a collection of spells.
        /// </summary>
        internal static void EncodeSpellList(this ChecksumEncoder ByteStream, List<SpellData> Spells)
        {
            if (Spells.Count < 200)
            {
                ByteStream.WriteVInt(Spells.Count);
                Spells.ForEach(ByteStream.EncodeData);
            }
            else 
                ByteStream.WriteVInt(0);
        }

        /// <summary>
        /// Decodes a <see cref="List{T}"/> of int.
        /// </summary>
        internal static CsvData DecodeIntList(this ByteStream ByteStream, ref List<int> List)
        {
            // TODO !

            if (List.Count > 0)
            {
                List.Clear();
            }

            int Count = ByteStream.ReadVInt();

            for (int I = 0; I < Count; I++)
            {
                List.Add(ByteStream.ReadVInt());
            }

            return null;
        }

        /// <summary>
        /// Decodes a collection of spells.
        /// </summary>
        internal static void DecodeSpellList(this ByteStream ByteStream, ref List<SpellData> Spells)
        {
            int Count = ByteStream.ReadVInt();
            
            if (Count > -1)
            {
                if (Count >= 200)
                {
                    throw new Exception("DecodeSpellList: List size too big. (" + Count + ")");
                }

                Spells = new List<SpellData>(Count);

                for (int I = 0; I < Count; I++)
                {
                    Spells.Add(ByteStream.DecodeData<SpellData>());
                }
            }
        }

        /// <summary>
        /// Reads a data reference.
        /// </summary>
        internal static CsvData DecodeData(this ByteStream ByteStream)
        {
            int Type = ByteStream.ReadVInt();

            if (Type > 0)
            {
                CsvTable Table = CsvFiles.Get(Type);

                if (Table != null)
                {
                    return Table.GetWithInstanceId(ByteStream.ReadVInt());
                }

                Logging.Error(typeof(ByteStreamHelper), "ReadData() - Table " + Type + " doesn't exists.");
            }

            return null;
        }

        /// <summary>
        /// Reads a data reference.
        /// </summary>
        internal static T DecodeData<T>(this ByteStream ByteStream) where T : CsvData
        {
            int Type = ByteStream.ReadVInt();

            if (Type > 0)
            {
                CsvTable Table = CsvFiles.Get(Type);

                if (Table != null)
                {
                    return Table.GetWithInstanceId(ByteStream.ReadVInt()) as T;
                }

                Logging.Error(typeof(ByteStreamHelper), "ReadData() - Table " + Type + " doesn't exists.");
            }

            return null;
        }

        /// <summary>
        /// Reads a data reference.
        /// </summary>
        internal static CsvData DecodeLogicData(this ByteStream ByteStream, int BaseType)
        {
            int Id = ByteStream.ReadVInt();

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
        internal static T DecodeLogicData<T>(this ByteStream ByteStream, int BaseType) where T : CsvData
        {
            int Id = ByteStream.ReadVInt();

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
        internal static LogicLong DecodeLogicLong(this ByteStream ByteStream)
        {
            LogicLong LogicLong = new LogicLong();
            LogicLong.Decode(ByteStream);
            return LogicLong;
        }
    }
}