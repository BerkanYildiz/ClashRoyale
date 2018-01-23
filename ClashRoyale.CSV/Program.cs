namespace ClashRoyale.CSV
{
    using System;
    using System.Data.Entity.Design.PluralizationServices;
    using System.Globalization;
    using System.IO;
    using System.Text;

    using Microsoft.VisualBasic.FileIO;

    using SearchOption = System.IO.SearchOption;

    internal class Program
    {
        public static PluralizationService PluralizationService = PluralizationService.CreateService(new CultureInfo("en-us"));

        public static string Template;

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        private static void Main()
        {
            Program.Template = File.ReadAllText("Template.txt");
            foreach (string path in Directory.GetFiles("Gamefiles/", "*.csv", SearchOption.AllDirectories))
            {
                string directory = Path.GetDirectoryName(path);

                if (directory.Contains("csv_client") || directory.Contains("csv_logic"))
                {
                    Directory.CreateDirectory(directory.Replace("Gamefiles", "Output"));
                    Program.CreateData(path);
                }
            }
        }

        /// <summary>
        /// Creates the data.
        /// </summary>
        /// <param name="csvPath">The CSV path.</param>
        private static void CreateData(string csvPath)
        {
            string fileName = Path.GetFileName(csvPath);
            string[] words = fileName.Replace(".csv", string.Empty).Split('_');
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = Program.PluralizationService.Singularize(words[i]);
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
            }

            string dataName = string.Concat(words) + "Data";
            using (StringReader reader = new StringReader(File.ReadAllText(csvPath)))
            {
                using (TextFieldParser parser = new TextFieldParser(reader))
                {
                    parser.Delimiters = new[]
                                            {
                                                ","
                                            };
                    string[] columns = parser.ReadFields();
                    string[] types = parser.ReadFields();
                    bool[] isArray = new bool[columns.Length];
                    while (!parser.EndOfData)
                    {
                        string[] values = parser.ReadFields();
                        if (string.IsNullOrEmpty(values[0]))
                        {
                            for (int i = 1; i < columns.Length; i++)
                            {
                                if (!isArray[i])
                                {
                                    isArray[i] = !string.IsNullOrEmpty(values[i]);
                                }
                            }
                        }
                    }

                    StringBuilder properties = new StringBuilder();
                    for (int i = 0; i < columns.Length; i++)
                    {
                        string column = columns[i];
                        if (!column.Equals("Name"))
                        {
                            if (column.Equals("-") || column.Equals("_"))
                            {
                                continue;
                            }

                            if (column.Length == 1)
                            {
                                if (column[0] >= '0' && column[0] <= '9')
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (column.Length == 0)
                                {
                                    continue;
                                }
                            }

                            column = column.Replace("-", string.Empty).Replace("_", string.Empty);
                            string type;
                            switch (types[i])
                            {
                                case "string":
                                case "String":
                                    {
                                        type = "string";
                                        break;
                                    }

                                case "int":
                                case "Int":
                                    {
                                        type = "int";
                                        break;
                                    }

                                case "Bool":
                                case "bool":
                                case "boolean":
                                case "Boolean":
                                    {
                                        type = "bool";
                                        break;
                                    }

                                default:
                                    {
                                        Console.WriteLine("Column type " + types[i] + " not valid. File: " + csvPath);
                                        return;
                                    }
                            }

                            if (isArray[i])
                            {
                                type += "[]";
                            }

                            properties.AppendLine("        public " + type + " " + column);
                            properties.AppendLine("        {");
                            properties.AppendLine("            get; set;");
                            properties.AppendLine("        }");
                            properties.AppendLine(string.Empty);
                        }
                    }

                    File.WriteAllText(csvPath.Replace("Gamefiles", "Output").Replace(fileName, dataName) + ".cs", Program.Template.Replace("#PROPERTIES#", properties.ToString()).Replace("#NAME#", dataName));
                }
            }
        }
    }
}