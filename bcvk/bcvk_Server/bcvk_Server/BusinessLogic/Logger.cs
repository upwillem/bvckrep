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

        public static bool SetLog(int accountId, Activity activity)
        {
            string activityString = "";

            switch (activity)
            {
                case Activity.CreateMainAccount:
                    activityString = "Main account created";
                    break;
                case Activity.ConnectionMade:
                    activityString = "Connection made";
                    break;
                case Activity.ConnectionAddedContact:
                    activityString = "Added contact to connection";
                    break;
                case Activity.ConnectionEstablished:
                    activityString = "Connection established";
                    break;
                case Activity.ConnectionEnded:
                    activityString = "Connection ended";
                    break;
                default:
                    break;
            }

            Mysql.Query("INSERT INTO logs (account_id,activity) VALUES(" + accountId + ", '" + activityString + "')");

            return true;
        }

        public static bool SetLog(int accountId, string activity, int contactId)
        {
            return true;
        }
    }
}
