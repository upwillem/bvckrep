using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;

namespace Bu
{
    /// <summary>
    /// OWNER: Ralph Lazarus 1227319
    /// Logs certain activities to the database
    /// </summary>
    public class Logger
    {
        public enum Activity
        {
            CreateMainAccount,
            ConnectionMade,
            ConnectionAddedContact,
            ConnectionEstablished,
            ConnectionEnded
        };

        /// <summary>
        /// This array contains the text to add to the database.
        /// The indexes of this array and the Activity-enum are identical.
        /// </summary>
        protected static string[] activityString = {
            "Main account created",
            "Connection made",
            "Added contact to connection",
            "Connection established",
            "Connection ended"
        };

        /// <summary>
        /// Logs an activity to the database
        /// </summary>
        /// <param name="accountId">The account id of the person who needs to be logged</param>
        /// <param name="activity">The specific activity to log</param>
        /// <returns>True, for now</returns>
        public static bool SetLog(int accountId, Activity activity)
        {
            Mysql.Query(String.Format("INSERT INTO logs (account_id,activity) VALUES({0},'{1}')", accountId, activityString[(int)activity]));

            return true;
        }

        /// <summary>
        /// Logs an activity to the database, including the contact ID
        /// </summary>
        /// <param name="accountId">The account id of the person who needs to be logged</param>
        /// <param name="activity">The specific activity to log</param>
        /// <param name="contactId">The account id of the contact</param>
        /// <returns>True, for now</returns>
        public static bool SetLog(int accountId, Activity activity, int contactId)
        {
            Mysql.Query(String.Format("INSERT INTO logs (account_id,activity,contact_id) VALUES({0},'{1}',{2})", accountId, activityString[(int)activity], contactId));

            return true;
        }
    }
}
