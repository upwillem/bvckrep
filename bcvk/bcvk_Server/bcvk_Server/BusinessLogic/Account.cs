﻿using Dal;
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
        public void AddMainAccount(string username, string password, string email, string name)
        {
            string passwordHash = MakeSHA512Hash(password);

            // Prepare the SQL statement.
            string query = "INSERT INTO accounts (parent_id, username, password, email, name) VALUES('0', '" + username + "', '" + passwordHash + "', '" + email + "', '" + name + "')";

            // Execute the query.
            mysql.Query(query);
        }
    }
}
