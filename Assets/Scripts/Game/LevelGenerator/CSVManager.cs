using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Game.LevelGenerator
{
    public class CSVManager
    {
        /// <summary>
        /// Class to store one CSV row
        /// </summary>
        public class CsvRow : List<string>
        {
            public string LineText { get; set; }
        }

        /// <summary>
        /// Class to write data to a CSV file
        /// </summary>
        public class CsvFileWriter : StreamWriter
        {
            public CsvFileWriter(Stream stream)
                : base(stream)
            {
            }

            public CsvFileWriter(string filename, bool append)
                : base(filename, append)
            {
            }

            /// <summary>
            /// Writes a single row to a CSV file.
            /// </summary>
            /// <param name="row">The row to be written</param>
            public void WriteRow(CsvRow row)
            {
                StringBuilder builder = new StringBuilder();
                bool firstColumn = true;
                foreach (string value in row)
                {
                    // Add separator if this isn't the first value
                    if (!firstColumn)
                        builder.Append(',');
                    // Implement special handling for values that contain comma or quote
                    // Enclose in quotes and double up any double quotes
                    if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                        builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                    else
                        builder.Append(value);
                    firstColumn = false;
                }
                row.LineText = builder.ToString();
                WriteLine(row.LineText);
            }
        }
        public class CsvFileReader : StreamReader
        {
            public CsvFileReader(Stream stream)
                : base(stream)
            {
            }

            public CsvFileReader(string filename)
                : base(filename)
            {
            }

            /// <summary>
            /// Reads a row of data from a CSV file
            /// </summary>
            /// <param name="row"></param>
            /// <returns></returns>
            public bool ReadRow(CsvRow row)
            {
                row.LineText = ReadLine();
                if (String.IsNullOrEmpty(row.LineText))
                    return false;

                int pos = 0;
                int rows = 0;

                while (pos < row.LineText.Length)
                {
                    string value;

                    // Special handling for quoted field
                    if (row.LineText[pos] == '"')
                    {
                        // Skip initial quote
                        pos++;

                        // Parse quoted value
                        int start = pos;
                        while (pos < row.LineText.Length)
                        {
                            // Test for quote character
                            if (row.LineText[pos] == '"')
                            {
                                // Found one
                                pos++;

                                // If two quotes together, keep one
                                // Otherwise, indicates end of value
                                if (pos >= row.LineText.Length || row.LineText[pos] != '"')
                                {
                                    pos--;
                                    break;
                                }
                            }
                            pos++;
                        }
                        value = row.LineText.Substring(start, pos - start);
                        value = value.Replace("\"\"", "\"");
                    }
                    else
                    {
                        // Parse unquoted value
                        int start = pos;
                        while (pos < row.LineText.Length && row.LineText[pos] != ',')
                            pos++;
                        value = row.LineText.Substring(start, pos - start);
                    }

                    // Add field to list
                    if (rows < row.Count)
                        row[rows] = value;
                    else
                        row.Add(value);
                    rows++;

                    // Eat up to and including next comma
                    while (pos < row.LineText.Length && row.LineText[pos] != ',')
                        pos++;
                    if (pos < row.LineText.Length)
                        pos++;
                }
                // Delete any unused items
                while (row.Count > rows)
                    row.RemoveAt(rows);

                // Return true if any columns read
                return (row.Count > 0);
            }
        }

        //Create e CSV file with all desired columns
        static void createCSV(String filename)
        {
            using (CsvFileWriter writer = new CsvFileWriter(filename, false))
            {
                CsvRow row = new CsvRow();
                row.Add(String.Format("Level ID"));
                row.Add(String.Format("NKeys"));
                row.Add(String.Format("NLocks"));
                row.Add(String.Format("NRooms"));
                row.Add(String.Format("LC"));
                row.Add(String.Format("OpenLocks"));
                row.Add(String.Format("OpenRooms"));
                row.Add(String.Format("Fitness"));
                row.Add(String.Format("Time"));
                writer.WriteRow(row);
            }
        }

        //Save a whole level in the csv file
        public static void SaveCSVLevel(int id, int nkeys, int nlocks, int nrooms, float lc, int openlocks, float openRooms, double fitness, long time, string filename)
        {
            try
            {
                CsvFileReader reader = new CsvFileReader(filename);
                reader.Close();
            }
            catch
            {
                createCSV(filename);
            }
            // Write sample data to CSV file
            using (CsvFileWriter writer = new CsvFileWriter(filename, true))
            {
                CsvRow row = new CsvRow();

                row.Add(String.Format("{0}", id));
                row.Add(String.Format("{0}", nkeys));
                row.Add(String.Format("{0}", nlocks));
                row.Add(String.Format("{0}", nrooms));
                row.Add(String.Format("{0}", lc));
                row.Add(String.Format("{0}", openlocks));
                row.Add(String.Format("{0}", openRooms));
                row.Add(String.Format("{0}", fitness));
                row.Add(String.Format("{0}", time));
                writer.WriteRow(row);
            }
        }
    }
}
