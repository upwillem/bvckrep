using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bu
{
    /// <summary>
    /// OWNER: Roel Larik 1236830 & Ralph
    /// Allows interraction with the accounts.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Stores the MySQL object.
        /// </summary>
        private Mysql mysql;

        /// <summary>
        /// Creating the MySQL connection.
        /// </summary>
        public Account()
        {
            mysql = new Mysql();
        }

        /// <summary>
        /// Creates a SHA512 hash.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private static string MakeSHA512Hash(string inputString)
        {
            HashAlgorithm algorithm = SHA512.Create();
            byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        /// <summary>
        /// Adds a new main account to the database.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="name"></param>
        public void AddMainAccount(string username, string password, string email, string name, string phone)
        {
            // Make the password SHA512.
            string passwordHash = MakeSHA512Hash(password);

            // Prepare the SQL statement.
            string query = "INSERT INTO accounts (parent_id, username, password, email, name, phone) VALUES('0', '" + Mysql.MySQLEscape(username) + "', '" + Mysql.MySQLEscape(passwordHash) + "', '" + Mysql.MySQLEscape(email) + "', '" + Mysql.MySQLEscape(name) + "', '" + Mysql.MySQLEscape(phone) + "')";

            // Execute the query.
            mysql.Query(query);
        }

        /// <summary>
        /// Adds a sub-account.
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <param name="profleImage"></param>
        public void AddSubAccount(int parentId, string username, string password, string name, byte[] profleImage)
        {
            // Make the password SHA512.
            string passwordHash = MakeSHA512Hash(password);

            // Prepare the SQL statement.
            string query = "INSERT INTO accounts (parent_id, username, password, name, photo) VALUES('" + Mysql.MySQLEscape(parentId) + "', '" + Mysql.MySQLEscape(username) + "', '" + Mysql.MySQLEscape(passwordHash) + "', '" + Mysql.MySQLEscape(name) + "', '" + profleImage + "')";

            // Execute the query.
            mysql.Query(query);
        }

        /// <summary>
        /// Returns whether an account exists.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool AccountExists(string username)
        {
            Dal.Mysql mysql = new Mysql();
            return mysql.Exists("accounts", "username", username);
        }
    }
}
