using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Dal
{
    /// <summary>
    /// OWNER: Roel Larik 1236830 & Ralph
    /// This class creates an instance of a MySQL connection.
    /// </summary>
    public class Mysql
    {
        /// <summary>
        /// Executes a query on the database.
        /// </summary>
        /// <param name="query"></param>
        public static void Query(string query)
        {
            using (MySqlConnection con = new MySqlConnection(Settings.Default.connectionString))
            {
                using(MySqlCommand com = new MySqlCommand(query, con))
                {
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        /// <summary>
        /// Method to execute the select command.
        /// </summary>
        /// <param name="command">The given SQL statement.</param>
        /// <returns></returns>
        public static List<string[]> Select(string command)
        {
            // Create a list to store the results.
            List<string[]> list = new List<string[]>();

            using (MySqlConnection con = new MySqlConnection(Settings.Default.connectionString))
            {
                // Open the connection.
                con.Open();

                // Store the DataTable object.
                DataTable table = new DataTable();

                // Create the SQL command.
                MySqlCommand cmd = new MySqlCommand(command, con);

                // Create a Data Adapter.
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                table.Clear();
                adapter.Fill(table);

                // Add the results to the output.
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string[] rows = new string[table.Columns.Count];

                    for (int x = 0; x < table.Columns.Count; x++)
                        rows[x] = table.Rows[i][x].ToString();

                    list.Add(rows);
                }

                // Close the connection
                con.Close();
            }

            // Return the list with data.
            return list;
        }

        /// <summary>
        /// Returns whether a result exists.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Exists(string table, string field, string value)
        {
            string test = String.Format("SELECT {1} FROM {0} WHERE {1} = '{2}'", MySQLEscape(table), MySQLEscape(field), MySQLEscape(value));
            List<string[]> list = Select(test);
            return (list.Count > 0);
        }

        /// <summary>
        /// Returns whether a result exists.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="field1"></param>
        /// <param name="value1"></param>
        /// <param name="field2"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool Exists(string table, string field1, string value1, string field2, string value2)
        {
            string query = String.Format("SELECT {1} FROM {0} WHERE {1} = '{3}' AND {2} = '{4}'", MySQLEscape(table), MySQLEscape(field1), MySQLEscape(field2), MySQLEscape(value1), MySQLEscape(value2));
            List<string[]> list = Select(query);
            return (list.Count > 0);
        }

        /// <summary>
        /// Escapes a string to prevent SQL injections.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MySQLEscape(string str)
        {
            return Regex.Replace(str, @"[\x00'""\b\n\r\t\cZ\\%_]",
                delegate(Match match)
                {
                    string v = match.Value;
                    switch (v)
                    {
                        case "\x00":
                            return "\\0";
                        case "\b":
                            return "\\b";
                        case "\n":
                            return "\\n";
                        case "\r":
                            return "\\r";
                        case "\t":
                            return "\\t";
                        case "\u001A":
                            return "\\Z";
                        default:
                            return "\\" + v;
                    }
                });
        }
    }
}
