using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    /// <summary>
    /// OWNER: Roel Larik 1236830
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
        /// Adds a new main account to the database.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="name"></param>
        public void AddMainAccount(string username, string password, string email, string name)
        {
            // Prepare the SQL statement.
            string query = "INSERT INTO accounts (parent_id, username, password, email, name) VALUES('0', '" + username + "', '" + password + "', '" + email + "', '" + name + "')";

            // Execute the query.
            mysql.Query(query);
        }
    }
}
