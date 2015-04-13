using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bu
{
    /// <summary>
    /// Owners: Aron Huntjens 1209361, Luc Schnabel 1207776
    /// </summary>
    public class AccountData
    {
        private static AccountData instance;

        //constructor
        private AccountData() { }

        //public access to the instance of this class
        public static AccountData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountData();
                }
                return instance;
            }
        }

        /// <summary>
        /// Aron Huntjens 1209361, Luc Schnabel 1207776,
        /// All received values of the account
        /// </summary>
        #region Properties values of accountdata
        private string username;

        public string Username
        {
            get { return "Hansje"; }
            set { username = value; }
        }

        public string AccountId { get; set; }

        public string ParentId { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        public string Phonenumber { get; set; }

        public string Photo { get; set; }

        public string ConnectionId;

        public string Log { get; set; }

        public string Children { get; set; }

        public string Contacts { get; set; } 

        public string ConnectionStatus { get; set; }

        public string ConnectionEstablishedStatus { get; set; }
        #endregion

    }
}
