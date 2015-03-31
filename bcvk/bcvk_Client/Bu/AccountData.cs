using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bu
{
    public class AccountData
    {
        private static AccountData instance;

        private AccountData() { }

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

        public string Username { get; set; }
    }
}
