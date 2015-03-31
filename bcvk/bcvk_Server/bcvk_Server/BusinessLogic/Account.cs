using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

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
            string query = "INSERT INTO accounts (username, password, email, name, phone) VALUES('" + Mysql.MySQLEscape(username) + "', '" + Mysql.MySQLEscape(passwordHash) + "', '" + Mysql.MySQLEscape(email) + "', '" + Mysql.MySQLEscape(name) + "', '" + Mysql.MySQLEscape(phone) + "')";

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
            string query = "INSERT INTO accounts (parent_id, username, password, name, photo) VALUES(" + parentId + ", '" + Mysql.MySQLEscape(username) + "', '" + Mysql.MySQLEscape(passwordHash) + "', '" + Mysql.MySQLEscape(name) + "', '" + profleImage + "')";

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
            Mysql mysql = new Mysql();
            return mysql.Exists("accounts", "username", username);
        }

        /// <summary>
        /// Returns all accounts data.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<string[]> GetAccountData(string username)
        {
            // Create a new MySQL instance.
            Mysql mysql = new Mysql();

            // Prepare the output.
            List<string[]> output = new List<string[]>();

            // Select all accounts data.
            string query = String.Format("SELECT id,parent_id,username,email,password,name,phone,photo FROM accounts WHERE username = '{0}'", Mysql.MySQLEscape(username));
            output = mysql.Select(query);

            // Retrieve all contacts.
            string contacts = String.Format("SELECT * FROM contacts WHERE account_id = '{0}'", Mysql.MySQLEscape(output[0][0]));
            List<string[]> contactList = mysql.Select(contacts);

            var jsonContacts = new JavaScriptSerializer().Serialize(contactList);

            Console.WriteLine(jsonContacts);

            return output;
        }
    }
}
