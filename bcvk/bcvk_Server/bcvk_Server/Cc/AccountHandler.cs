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
        public static void CreateMainAccount(string username, string password1, string password2, string email, string name)
        {
            List<string> response = new List<string>();
            
            if (password1.Length < 6 || password2.Length < 6)
            {
                response.Add("error;Password must be 6 characters long.");
            }

            if (password1 != password2)
            {
                response.Add("error;Passwords do not match.");
            }

            Account ac = new Account();

            if (username.Length < 4 || username.Length > 25)
            {
                response.Add("error;Account must be between 4 and 25 characters.");
            }

            if (Account.AccountExists(username))
            {
                response.Add("error;Account already exists.");
            }

            if (name.Length < 2 || name.Length > 60)
            {
                response.Add("error;Name must be between 2 and 60 characters.");
            }

            
            ac.AddMainAccount(username, password1, email, name);
            response.Add("error;Passwords do not match.");
        }
        /*
        private bool IsValidEmail(string strIn)
        {
            bool invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names. 
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format. 
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }*/
    }
}
