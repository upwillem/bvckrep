using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bu
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

        public Account()
        {
            mysql = new Mysql();
        }

        public void AddAccount(string username, string password, string email, string name)
        {
            string query = "INSERT INTO accounts (username, password, email, name) VALUES('" + username + "', '" + password + "', '" + email + "', '" + name + "')";

            mysql.Query(query);
        }
    }
}
