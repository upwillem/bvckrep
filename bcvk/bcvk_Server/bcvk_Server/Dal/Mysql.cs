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
    /// OWNER: Roel Larik 1236830 & Ralph Lazarus 1227319
    /// This class creates an instance of a MySQL connection.
    /// </summary>
    public class Mysql
    {
        /// <summary>
        /// Executes a query on the database.
        /// </summary>
        /// <param name="query">The SQL statement to execute</param>
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
        /// <returns>The data selected from the database</returns>
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
        /// Returns whether a result exists with a specific condition.
        /// </summary>
        /// <param name="table">The table-name from the database</param>
        /// <param name="field">The column/field-name from the database</param>
        /// <param name="value">The value from the database which will be checked</param>
        /// <returns>True if the value exists, false if the value doesn't exist</returns>
        public static bool Exists(string table, string field, string value)
        {
            string test = String.Format("SELECT {1} FROM {0} WHERE {1} = '{2}'", MySQLEscape(table), MySQLEscape(field), MySQLEscape(value));
            List<string[]> list = Select(test);
            return (list.Count > 0);
        }

        /// <summary>
        /// Returns whether a results exists with two conditions.
        /// </summary>
        /// <param name="table">The table-name from the database</param>
        /// <param name="field1">The first column/field-name from the database</param>
        /// <param name="value1">The first value from the database which will be checked</param>
        /// <param name="field2">The second column/field-name from the database</param>
        /// <param name="value2">The second value from the database which will be checked</param>
        /// <returns>True if a row exists with field1=value1 and field2=value2, false if not</returns>
        public static bool Exists(string table, string field1, string value1, string field2, string value2)
        {
            string query = String.Format("SELECT {1} FROM {0} WHERE {1} = {3} AND {2} = {4}", MySQLEscape(table), field1, field2, MySQLEscape(value1), MySQLEscape(value2));
            List<string[]> list = Select(query);
            return (list.Count > 0);
        }

        /// <summary>
        /// Returns a specific field if a results exists with two conditions.
        /// </summary>
        /// <param name="table">The table-name from the database</param>
        /// <param name="field1">The first column/field-name from the database</param>
        /// <param name="value1">The first value from the database which will be checked</param>
        /// <param name="field2">The second column/field-name from the database</param>
        /// <param name="value2">The second value from the database which will be checked</param>
        /// <param name="field3">The column/field which contains the return value</param>
        /// <returns>The first value in the selected column/field</returns>
        public static string Value(string table, string field1, string value1, string field2, string value2, string field3)
        {
            string query = String.Format("SELECT {5} FROM {0} WHERE {1} = {2} AND {3} = {4}", table, field1, value1, field2, value2, field3);
            List<string[]> list = Select(query);
            return list[0][0];
        }

        /// <summary>
        /// Escapes a string to prevent SQL injections.
        /// </summary>
        /// <param name="str">The string to be escaped</param>
        /// <returns>The escaped string</returns>
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
