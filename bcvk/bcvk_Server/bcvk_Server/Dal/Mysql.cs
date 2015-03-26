using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace Dal
{
    /// <summary>
    /// OWNER: Roel Larik 1236830 & Ralph
    /// This class creates an instance of a MySQL connection.
    /// </summary>
    public class Mysql
    {
        /// <summary>
        /// MySQLConnection object.
        /// </summary>
        private MySqlConnection connection;

        /// <summary>
        /// DataTable to store SELECT statements in.
        /// </summary>
        private DataTable table;

        /// <summary>
        /// Constructor of the class receiving the connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        public Mysql(string connectionString)
        {
            // Establish the connection
            connection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Opens the connection with the database.
        /// </summary>
        /// <returns>True if successful, false if not successful</returns>
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password.");
                        break;
                }
                return false;
            }
        }

        /// <summary>
        /// Closes the connection with the database.
        /// </summary>
        /// <returns>True if successful, false if not successful</returns>
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Database connection could not be closed.");
                return false;
            }
        }

        /// <summary>
        /// Executes a query on the database.
        /// </summary>
        /// <param name="query"></param>
        public void Query(string query)
        {
            if (OpenConnection())
            {
                // Set the command with the connection.
                MySqlCommand cmd = new MySqlCommand(query, connection);

                // Execute the command.
                cmd.ExecuteNonQuery();
            }
            CloseConnection();
        }

        /// <summary>
        /// Method to execute the select command.
        /// </summary>
        /// <param name="command">The given SQL statement.</param>
        /// <returns></returns>
        public List<string[]> Select(string command)
        {
            // Create a list to store the results.
            List<string[]> list = new List<string[]>();

            if (OpenConnection())
            {
                // Store the DataTable object.
                table = new DataTable();

                // Create the SQL command.
                MySqlCommand cmd = new MySqlCommand(command, connection);

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
            }

            // Return the list with data.
            CloseConnection();
            return list;
        }
    }
}
