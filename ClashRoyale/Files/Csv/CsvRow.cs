namespace ClashRoyale.Files.Csv
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class CsvRow
    {
        public readonly int Offset;

        public readonly CsvReader Reader;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CsvRow" /> class.
        /// </summary>
        /// <param name="Reader">The reader.</param>
        public CsvRow(CsvReader Reader)
        {
            this.Reader = Reader;
            this.Offset = this.Reader.GetColumnRowCount();
        }

        /// <summary>
        ///     Gets the name of this CSV.
        /// </summary>
        public string Name
        {
            get
            {
                return this.Reader.GetValueAt(0, this.Offset);
            }
        }

        /// <summary>
        ///     Gets the size of the specified column.
        /// </summary>
        /// <param name="Name">The name.</param>
        public int GetSize(string Name)
        {
            int i = this.Reader.GetColumnIndexByName(Name);

            if (i != -1)
            {
                return this.Reader.GetArraySizeAt(this, i);
            }

            return 0;
        }

        /// <summary>
        ///     Gets the value at the specified row.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="Offset">The offset.</param>
        public string GetValue(string Name, int Offset)
        {
            return this.Reader.GetValue(Name, Offset + this.Offset);
        }

        /// <summary>
        ///     Reads the data.
        /// </summary>
        /// <param name="Data">The data.</param>
        /// <exception cref="System.Exception"></exception>
        public void LoadData(CsvData Data)
        {
            foreach (PropertyInfo Property in Data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (Property.CanRead && Property.CanWrite)
                {
                    if (Property.PropertyType.IsArray)
                    {
                        Type ElementType = Property.PropertyType.GetElementType();

                        if (ElementType == typeof(bool))
                        {
                            Property.SetValue(Data, this.LoadBoolArray(Property.Name));
                        }
                        else if (ElementType == typeof(int))
                        {
                            Property.SetValue(Data, this.LoadIntArray(Property.Name));
                        }
                        else if (ElementType == typeof(string))
                        {
                            Property.SetValue(Data, this.LoadStringArray(Property.Name));
                        }
                        else
                        {
                            throw new Exception(ElementType + "[] is not a valid array.");
                        }
                    }
                    else if (Property.PropertyType.IsGenericType)
                    {
                        if (Property.PropertyType == typeof(List<>))
                        {
                            Type ListType = typeof(List<>);
                            Type[] Generic = Property.PropertyType.GetGenericArguments();
                            Type ConcreteType = ListType.MakeGenericType(Generic);
                            object NewList = Activator.CreateInstance(ConcreteType);
                            MethodInfo Add = ConcreteType.GetMethod("Add");
                            string IndexerName = ((DefaultMemberAttribute) NewList.GetType().GetCustomAttributes(typeof(DefaultMemberAttribute), true)[0]).MemberName;

                            PropertyInfo IndexProperty = NewList.GetType().GetProperty(IndexerName);

                            for (int i = this.Offset; i < this.Offset + this.GetSize(Property.Name); i++)
                            {
                                string Value = this.GetValue(Property.Name, i - this.Offset);

                                if (Value == string.Empty && i != this.Offset)
                                {
                                    Value = IndexProperty.GetValue(NewList, new object[]
                                    {
                                        i - this.Offset - 1
                                    }).ToString();
                                }

                                if (string.IsNullOrEmpty(Value))
                                {
                                    object Object = Generic[0].IsValueType ? Activator.CreateInstance(Generic[0]) : string.Empty;

                                    Add.Invoke(NewList, new[]
                                    {
                                        Object
                                    });
                                }
                                else
                                {
                                    Add.Invoke(NewList, new[]
                                    {
                                        Convert.ChangeType(Value, Generic[0])
                                    });
                                }
                            }

                            Property.SetValue(Data, NewList);
                        }
                        else if (Property.PropertyType == typeof(CsvData) || Property.PropertyType.BaseType == typeof(CsvData))
                        {
                            this.LoadData((CsvData) Property.GetValue(Property));
                        }
                    }
                    else
                    {
                        string Value = this.GetValue(Property.Name, 0);

                        if (!string.IsNullOrEmpty(Value))
                        {
                            Property.SetValue(Data, Convert.ChangeType(Value, Property.PropertyType));
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Loads the bool array.
        /// </summary>
        /// <param name="Column">The column.</param>
        /// <exception cref="System.Exception">The value is not a valid boolean.</exception>
        private bool[] LoadBoolArray(string Column)
        {
            bool[] Array = new bool[this.GetSize(Column)];

            for (int i = 0; i < Array.Length; i++)
            {
                string Value = this.GetValue(Column, i);

                if (!string.IsNullOrEmpty(Value))
                {
                    if (bool.TryParse(Value, out bool Boolean))
                    {
                        Array[i] = Boolean;
                    }
                    else
                    {
                        throw new Exception("The value is not a valid boolean.");
                    }
                }
            }

            return Array;
        }

        /// <summary>
        ///     Loads the int array.
        /// </summary>
        /// <param name="Column">The column.</param>
        /// <exception cref="System.Exception">The value is not a valid integer.</exception>
        private int[] LoadIntArray(string Column)
        {
            int[] Array = new int[this.GetSize(Column)];

            for (int i = 0; i < Array.Length; i++)
            {
                string Value = this.GetValue(Column, i);

                if (!string.IsNullOrEmpty(Value))
                {
                    if (int.TryParse(Value, out int Number))
                    {
                        Array[i] = Number;
                    }
                    else
                    {
                        throw new Exception("The value is not a valid integer.");
                    }
                }
            }

            return Array;
        }

        /// <summary>
        ///     Loads the string array.
        /// </summary>
        /// <param name="Column">The column.</param>
        private string[] LoadStringArray(string Column)
        {
            string[] Array = new string[this.GetSize(Column)];

            for (int i = 0; i < Array.Length; i++)
            {
                Array[i] = this.GetValue(Column, i);
            }

            return Array;
        }
    }
}