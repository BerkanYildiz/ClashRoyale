namespace ClashRoyale.Files.Csv.Tilemaps
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualBasic.FileIO;

    public class TilemapData
    {
        public int[] Map;

        public int MapHeight;
        public int MapWidth;
        public List<Object> Objects;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TilemapData" /> class.
        /// </summary>
        public TilemapData(string Path)
        {
            this.Name = System.IO.Path.GetFileName(Path);

            List<MapRow> MapRows = new List<MapRow>();
            List<LayoutRow> LayoutRows = new List<LayoutRow>();

            bool ReadLayout = false;

            using (TextFieldParser FieldParser = new TextFieldParser(Path))
            {
                FieldParser.SetDelimiters(",");

                while (!FieldParser.EndOfData)
                {
                    string[] Value = FieldParser.ReadFields();

                    if (!string.IsNullOrEmpty(Value[0]))
                    {
                        if (Value[0] == "Map")
                        {
                            ReadLayout = false;

                            MapRow Row = new MapRow(Value[0]);

                            string[] Column = FieldParser.ReadFields().Skip(1).ToArray();
                            string[] ValueType = FieldParser.ReadFields().Skip(1).ToArray();

                            for (int I = 0; I < Column.Length; I++)
                            {
                                if (!string.IsNullOrEmpty(Column[I]) && !string.IsNullOrEmpty(ValueType[I]))
                                {
                                    Row.Columns.Add(Column[I]);
                                    Row.ValueType.Add(ValueType[I]);
                                }
                            }

                            MapRows.Add(Row);
                        }
                        else if (Value[0] == "Layout")
                        {
                            ReadLayout = true;
                        }
                    }
                    else
                    {
                        if (!ReadLayout)
                        {
                            if (MapRows.Count > 0)
                            {
                                MapRow Row = MapRows[MapRows.Count - 1];

                                for (int I = 0; I < Row.Columns.Count; I++)
                                {
                                    if (!string.IsNullOrEmpty(Value[1 + I]))
                                    {
                                        Row.Values.Add(Value.Skip(1).Take(Row.Columns.Count).ToArray());
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(Value[1]))
                            {
                                LayoutRow LayoutRow = new LayoutRow(Value[1]);

                                string[] Column = FieldParser.ReadFields().Skip(2).ToArray();
                                string[] ValueType = FieldParser.ReadFields().Skip(2).ToArray();

                                for (int I = 0; I < Column.Length; I++)
                                {
                                    if (!string.IsNullOrEmpty(Column[I]) && !string.IsNullOrEmpty(ValueType[I]))
                                    {
                                        LayoutRow.Columns.Add(Column[I]);
                                        LayoutRow.ValueType.Add(ValueType[I]);
                                    }
                                }

                                LayoutRows.Add(LayoutRow);
                            }
                            else
                            {
                                if (LayoutRows.Count > 0)
                                {
                                    LayoutRow Row = LayoutRows[LayoutRows.Count - 1];

                                    for (int I = 0; I < Row.Columns.Count; I++)
                                    {
                                        if (!string.IsNullOrEmpty(Value[2 + I]))
                                        {
                                            Row.Values.Add(Value.Skip(2).Take(Row.Columns.Count).ToArray());
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                this.Objects = new List<Object>(64);

                if (LayoutRows.Count > 0)
                {
                    LayoutRow Row = LayoutRows[0];

                    for (int J = 0; J < Row.Values.Count; J++)
                    {
                        int X = int.Parse(Row.GetValueAt("x", J));
                        int Y = int.Parse(Row.GetValueAt("y", J));

                        this.Objects.Add(new Object(X, Y, Row.Name));
                    }
                }

                if (MapRows.Count > 0)
                {
                    this.MapWidth = MapRows[0].Columns.Count;
                    this.MapHeight = MapRows[0].Values.Count;

                    this.Map = new int[this.MapHeight * this.MapWidth];

                    for (int X = 0; X < this.MapWidth; X++)
                    {
                        for (int Y = 0; Y < this.MapHeight; Y++)
                        {
                            if (!string.IsNullOrEmpty(MapRows[0].Values[Y][X]))
                            {
                                this.Map[Y + this.MapWidth * X] = int.Parse(MapRows[0].Values[Y][X]);
                            }
                            else
                            {
                                this.Map[Y + this.MapWidth * X] = -1;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Gets the global identifier.
        /// </summary>
        public int GlobalId
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        ///     Gets the tilemap name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets if the specified tile is water.
        /// </summary>
        public bool IsWater(int X, int Y)
        {
            return (this.Map[X + this.MapWidth * Y] & 32) >> 5 > 0;
        }
    }
}