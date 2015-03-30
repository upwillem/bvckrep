using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bu;
using System.Text.RegularExpressions;

namespace Cc
{
    public class AccountHandler
    {
        /// <summary>
        /// Validates the request of creating a new main account.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password1"></param>
        /// <param name="password2"></param>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static List<string> CreateMainAccount(string username, string password1, string password2, string email, string name, string phoneNumber)
        {
            List<string> response = new List<string>();
            
            // Password validation.
            if (password1.Length < 6 || password2.Length < 6)
            {
                response.Add("error;Password must be 6 characters long.");
            }

            if (password1 != password2)
            {
                response.Add("error;Passwords do not match.");
            }

            Account ac = new Account();

            // Username validation.
            if (username.Length < 4 || username.Length > 25)
            {
                response.Add("error;Account must be between 4 and 25 characters.");
            }

            if (Account.AccountExists(username))
            {
                response.Add("error;Account already exists.");
            }

            // Name validation.
            if (name.Length < 2 || name.Length > 60)
            {
                response.Add("error;Name must be between 2 and 60 characters.");
            }

            // E-mail validation.
            if (!IsValidEmailAddress(email))
            {
                response.Add("error;This is now a valid e-mail address.");
            }

            // Check for errors.
            if (response.Count == 0)
            {
                ac.AddMainAccount(username, password1, email, name);
                response.Add("success;Account registered.");
            }

            return response;
        }

        /// <summary>
        /// Validates an e-mail address
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <returns></returns>
        public static bool IsValidEmailAddress(string mailAddress)
        {
            Regex mailIDPattern = new Regex(@"[\w-]+@([\w-]+\.)+[\w-]+");

            if (!string.IsNullOrEmpty(mailAddress) && mailIDPattern.IsMatch(mailAddress))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
