using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bu;

namespace Cc
{
    public class AccountHandler
    {
        public static void CreateMainAccount(string username, string password1, string password2, string email, string name)
        {
            Account ac = new Account();
            if (password1 == password2)
            {
                ac.AddMainAccount(username, password1, email, name);
            }
        }
    }
}
