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
        /// Adds a new main account to the database.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="name"></param>
        public static void AddMainAccount(string username, string password, string email, string name, string phone)
        {
            // Make the password SHA512.
            string passwordHash = MakeSHA512Hash(password);

            // Prepare the SQL statement.
            string query = "INSERT INTO accounts (username, password, email, name, phone) VALUES('" + Mysql.MySQLEscape(username) + "', '" + Mysql.MySQLEscape(passwordHash) + "', '" + Mysql.MySQLEscape(email) + "', '" + Mysql.MySQLEscape(name) + "', '" + Mysql.MySQLEscape(phone) + "')";

            // Execute the query.
            Mysql.Query(query);

            int accountId = GetAccountId(username);
            Logger.SetLog(accountId, Logger.Activity.CreateMainAccount);
        }

        /// <summary>
        /// Adds a sub-account.
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <param name="profleImage"></param>
        public static void AddSubAccount(int parentId, string username, string password, string name, byte[] profleImage)
        {
            // Make the password SHA512.
            string passwordHash = MakeSHA512Hash(password);

            // Prepare the SQL statement.
            string query = "INSERT INTO accounts (parent_id, username, password, name, photo) VALUES(" + parentId + ", '" + Mysql.MySQLEscape(username) + "', '" + Mysql.MySQLEscape(passwordHash) + "', '" + Mysql.MySQLEscape(name) + "', '" + profleImage + "')";

            // Execute the query.
            Mysql.Query(query);
        }

        /// <summary>
        /// Adds a new contact to the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="recipient"></param>
        public static void AddContact(string sender, string recipient)
        {
            // Get the account IDs of the sender and recipient.
            int senderId = GetAccountId(sender);
            int recipientId = GetAccountId(recipient);

            // Prepare the SQL statement.
            string query = String.Format("INSERT INTO contacts (account_id, contact_id) VALUES ('{0}', '{1}')", senderId, recipientId);

            // Execute the query.
            Mysql.Query(query);
        }
        /// <summary>
        /// Delete a contact from the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="recipient"></param>
        public static void DeleteContact(string sender, string recipient)
        {
            // Get the account IDs of the sender and recipient.
            int senderId = GetAccountId(sender);
            int recipientId = GetAccountId(recipient);

            // Prepare the SQL statement.
            string query = String.Format("DELETE FROM contacts WHERE (account_id={0} AND contact_id={1}) OR (account_id={1} AND contact_id={0})", senderId, recipientId);

            // Execute the query.
            Mysql.Query(query);
        }

        /// <summary>
        /// Returns whether an account exists.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool AccountExists(string username)
        {
            return Mysql.Exists("accounts", "username", username);
        }

        /// <summary>
        /// Returns whether a contact already exists.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        public static bool ContactExists(string sender, string recipient)
        {
            // Retrieve the account IDs.
            int senderId = GetAccountId(sender);
            int recipientId = GetAccountId(recipient);

            // Return the bool.
            return Mysql.Exists("contacts", "account_id", senderId.ToString(), "contact_id", recipientId.ToString());
        }

        /// <summary>
        /// Returns all accounts data.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<string> GetAccountData(string username)
        {
            // Select all accounts data.
            string query = String.Format("SELECT id,parent_id,username,email,name,phone,photo FROM accounts WHERE username = '{0}'", Mysql.MySQLEscape(username));
            List<string[]> output = Mysql.Select(query);
            List<string> returnlist = new List<string>(output[0]);

            // Retrieve all contacts.
            string jsonContacts = "";
            if (returnlist[1] != "0") // If not a parent
            {
                string contacts = String.Format("SELECT id, account_id, contact_id, is_blocked, UNIX_TIMESTAMP(time_added) FROM contacts WHERE account_id = '{0}'", Mysql.MySQLEscape(output[0][0]));
                List<string[]> contactList = Mysql.Select(contacts);

                for (int i = 0; i < contactList.Count; i++)
                {
                    for (int j = 0; j < contactList[0].Length; j++)
                    {
                        string key = "";
                        switch (j)
                        {
                            case 0: key = "id"; break;
                            case 1: key = "accountId"; break;
                            case 2: key = "contactId"; break;
                            case 3: key = "isBlocked"; break;
                            case 4: key = "timeAdded"; break;
                            default: break;
                        }
                        key = key + ";";
                        contactList[i][j] = key + contactList[i][j];
                    }
                }

                jsonContacts = new JavaScriptSerializer().Serialize(contactList);
            }

            returnlist.Add("");
            returnlist.Add("");


            string jsonChild = "";

            // Retrieve all children if user is a parent.
            if (returnlist[1] == "0")
            {
                string childrensql = String.Format("SELECT username FROM accounts WHERE parent_id = '{0}'", Mysql.MySQLEscape(returnlist[0]));
                List<string[]> childrenUsernames = Mysql.Select(childrensql);

                List<List<string>> childrenList = new List<List<string>>();

                foreach (string[] item in childrenUsernames)
                {
                    childrenList.Add(GetAccountData(item[0]));
                }
                jsonChild = new JavaScriptSerializer().Serialize(childrenList);
            }

            

            returnlist.Add(jsonChild);

            returnlist.Add(jsonContacts);

            for (int i = 0; i < returnlist.Count; i++)
            {
                string key = "";
                switch (i)
                {
                    case 0: key = "accountId"; break;
                    case 1: key = "parentId"; break;
                    case 2: key = "username"; break;
                    case 3: key = "email"; break;
                    case 4: key = "displayName"; break;
                    case 5: key = "phoneNumber"; break;
                    case 6: key = "photo"; break;
                    case 7: key = "connection"; break;
                    case 8: key = "log"; break;
                    case 9: key = "children"; break;
                    case 10: key = "contacts"; break;
                    default: break;
                }
                key = key + ";";
                returnlist[i] = key + returnlist[i];
            }

            return returnlist;
        }

        /// <summary>
        /// Returns the ID of an account based on a username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private static int GetAccountId(string username)
        {
            string query = String.Format("SELECT id FROM accounts WHERE username = '{0}'", Mysql.MySQLEscape(username));
            List<string[]> output = Mysql.Select(query);

            return Convert.ToInt32(output[0][0]);
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
    }
}
