using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;

namespace Bu
{
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

        protected static string[] activityString = {
            "Main account created",
            "Connection made",
            "Added contact to connection",
            "Connection established",
            "Connection ended"
        };

        public static bool SetLog(int accountId, Activity activity)
        {
            Mysql.Query(String.Format("INSERT INTO logs (account_id,activity) VALUES({0},'{1}')", accountId, activityString[(int)activity]));

            return true;
        }

        public static bool SetLog(int accountId, Activity activity, int contactId)
        {
            Mysql.Query(String.Format("INSERT INTO logs (account_id,activity,contact_id) VALUES({0},'{1}',{2})", accountId, activityString[(int)activity], contactId));

            return true;
        }
    }
}
