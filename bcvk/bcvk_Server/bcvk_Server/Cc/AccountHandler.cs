using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bu;
using System.Text.RegularExpressions;

namespace Cc
{
    /// <summary>
    /// OWNER: Roel Larik 1236830 & Ralph Lazarus 1227319
    /// Handles requests of accounts.
    /// </summary>
    public class AccountHandler
    {
        /// <summary>
        /// Validates the request of creating a new main account.
        /// </summary>
        /// <param name="username">The username of the new main account</param>
        /// <param name="password1">The password of the new main account</param>
        /// <param name="password2">The validated password of the new main account</param>
        /// <param name="email">The e-mail address of the new main account</param>
        /// <param name="name">The display name of the new main account</param>
        /// <param name="phoneNumber">The phone number of the new main account</param>
        /// <returns>Error or success messages</returns>
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
                Account.AddMainAccount(username, password1, email, name, phoneNumber);
                response.Add("success;Account registered.");
            }

            return response;
        }

        /// <summary>
        /// Validates the request of creating a new sub-account.
        /// </summary>
        /// <param name="parentId">The id of the parent main account</param>
        /// <param name="username">The username of the new sub-account</param>
        /// <param name="password1">The password of the new sub-account</param>
        /// <param name="password2">The validated password of the new sub-account</param>
        /// <param name="name">The display name of the new sub-account</param>
        /// <param name="profileImage">The profile image of the new sub-account</param>
        /// <returns>Error or success messages</returns>
        public static List<string> CreateSubAccount(int parentId, string username, string password1, string password2, string name, byte[] profileImage)
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

            // Check for errors.
            if (response.Count == 0)
            {
                Account.AddSubAccount(parentId, username, password1, name, profileImage);
                response.Add("success;Account registered.");
            }

            return response;
        }

        /// <summary>
        /// Returns all account data of a user.
        /// </summary>
        /// <param name="username">The username of the account</param>
        /// <returns>A list of data, which is prepended by a key and a seperator</returns>
        public static List<string> GetAccountData(string username)
        {
            return Account.GetAccountData(username);
        }

        /// <summary>
        /// Adds a new contact to an account.
        /// </summary>
        /// <param name="sender">The username of the account</param>
        /// <param name="recipient">The username of the new contact</param>
        /// <returns>True if contact is added, false if not</returns>
        public static bool AddContact(string sender, string recipient)
        {
            if (Account.AccountExists(recipient))
            {
                if (!Account.ContactExists(sender, recipient))
                {
                    Account.AddContact(sender, recipient);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Deletes a contact from the contactlist.
        /// </summary>
        /// <param name="sender">The username of the account</param>
        /// <param name="recipient">The username of the contact to be deleted</param>
        /// <returns>True if contact is deleted, false if not</returns>
        public static bool DeleteContact(string sender, string recipient)
        {
            if (Account.AccountExists(recipient))
            {
                Account.DeleteContact(sender, recipient);
                return true;
            }
            return false;
        }

        /// <summary>
        /// An account accepts a contact
        /// </summary>
        /// <param name="sender">The username of the account</param>
        /// <param name="recipient">The username of the contact</param>
        /// <returns>True if contact is accepted, false if not</returns>
        public static bool AcceptContact(string sender, string recipient)
        {
            if (Account.AccountExists(recipient) && Account.AccountExists(sender))
            {
                if (!Account.ContactExists(sender, recipient) && Account.ContactExists(recipient, sender))
                {
                    Account.AcceptContact(sender, recipient);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Toggles a block between a specific relation of contacts.
        /// If a contact is blocked, it will now be unblocked.
        /// If a contact is unblocked, it will now be blocked.
        /// </summary>
        /// <param name="sender">The username of the account</param>
        /// <param name="recipient">The username of the contact to toggle the block on</param>
        /// <returns>True if toggled, false if not</returns>
        public static bool ToggleBlock(string sender, string recipient)
        {
            if (Account.AccountExists(recipient))
            {
                if (Account.ContactExists(sender, recipient))
                {
                    Account.ToggleBlock(sender, recipient);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Validates an e-mail address
        /// </summary>
        /// <param name="mailAddress">The e-mail address to validate</param>
        /// <returns>True if valid, false if invalid</returns>
        private static bool IsValidEmailAddress(string mailAddress)
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
