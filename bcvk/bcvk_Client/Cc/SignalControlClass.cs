using Bu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cc
{
    public class SignalControlClass
    {
        private SignalCommunicationService signalCommunicationService;
        public event Action<string> initContacts;
        public event Action<string> beingCalled;

        public SignalControlClass() 
        {
            //AccountData.Instance.Username = username;
            signalCommunicationService = new SignalCommunicationService();
            signalCommunicationService.accountDataReady += signalCommunicationService_accountDataReady;
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// sets all values of 'AccountData'.
        /// </summary>
        /// <param name="accountDataList">list of all accountdata</param>
        private void signalCommunicationService_accountDataReady(List<string> accountDataList)
        {
            AccountData acc = AccountData.Instance;
            string oldContacts = acc.Contacts;
            foreach (string data in accountDataList)
            {
                #region switch cases accountdatalist
                switch (data.Split(';')[0])
                {
                    case "accountId":
                        acc.AccountId = data.Split(';')[1];
                        break;
                    case "parentId":
                        acc.ParentId = data.Split(';')[1];
                        break;
                    case "username":
                        acc.Username = data.Split(';')[1];
                        break;
                    case "email":
                        acc.Email = data.Split(';')[1];
                        break;
                    case "displayName":
                        acc.DisplayName = data.Split(';')[1];
                        break;
                    case "phonenumber":
                        acc.Phonenumber = data.Split(';')[1];
                        break;
                    case "photo":
                        acc.Photo = data.Split(';')[1];
                        break;
                    case "connection":
                        acc.ConnectionId = data.Split(';')[1];
                        break;
                    case "log":
                        acc.Log = data.Split(';')[1];
                        break;
                    case "children":
                        acc.Children = data.Split(';')[1];
                        break;
                    case "contacts":
                        acc.Contacts = data.Split(';')[1];
                        break;
                    default:
                        break;
                }
                #endregion
            }

            if (oldContacts != acc.Contacts)
                initContacts(acc.Contacts);

            if (AccountData.Instance.ConnectionId == "connecting")
            {
                beingCalled("Je wordt gebeld");
            }
        } 

        /// <summary>
        /// Luc Schnabel 1207776,
        /// creates the thread of polling the accountdata
        /// </summary>
        /// <param name="username">username of the current user</param>
        public void StartPoll()
        {
            signalCommunicationService.StartPoll(AccountData.Instance.Username);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// stop the running threads
        /// </summary>
        /// <returns>bool if stopped</returns>
        public bool StopPoll()
        {
            return signalCommunicationService.AbortsThreads();
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// answer the received call
        /// </summary>
        /// <param name="sender">the sender of the answer</param>
        /// <param name="contact">contact to send the answer to</param>
        /// <param name="connectionId">the connectionId of the call</param>
        /// <param name="answer">the answer of the sender</param>
        public void AnswerCall(string sender, string contact, string connectionId, string answer)
        {
            signalCommunicationService.AnswerCall(sender, contact, connectionId, answer);
        }

        /// <summary>
        /// Luc Schnabel 1207776, Aron Huntjens 1209361
        /// Call a client
        /// </summary>
        /// <param name="contact">client to call</param>
        public string DoCall(string contact) 
        {
            signalCommunicationService.DoCall(contact);
            return "Aan het bellen";
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// add a contact to the list
        /// </summary>
        /// <param name="recipient">contact</param>
        /// <returns>success or failure</returns>
        public bool AddContact(string recipient)
        {
            return signalCommunicationService.AddContact(recipient);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// accept a request to add a contact
        /// </summary>
        /// <param name="recipient">contact</param>
        /// <returns>success or failure</returns>
        public bool AcceptContact(string recipient)
        {
            return signalCommunicationService.AcceptContact(recipient);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// delete a contact
        /// </summary>
        /// <param name="recipient">contact</param>
        /// <returns>success or failure</returns>
        public bool DeleteContact(string recipient)
        {
            return signalCommunicationService.DeleteContact(recipient);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// end the call
        /// </summary>
        public void EndCall()
        {
            signalCommunicationService.EndCall();
        }

        /// <summary>
        /// Toggles the settings to block or deblock a contact
        /// </summary>
        /// <param name="recipient">contact to block</param>
        /// <returns>success or failure</returns>
        public bool ToggleBlock(string recipient)
        {
            return signalCommunicationService.ToggleBlock(recipient);
        }

        /// <summary>
        /// Luc Schnabel 1207776
        /// create a main account. This accoutn can edit the subaccount
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password1">password</param>
        /// <param name="password2">password repeat</param>
        /// <param name="email">email address</param>
        /// <param name="name">displayname</param>
        /// <param name="phoneNumber">phone number</param>
        /// <returns></returns>
        public List<string> CreateMainAccount(string username, string password1, string password2, string email, string name, string phoneNumber)
        {
            return signalCommunicationService.CreateMainAccount(username, password1, password2, email, name, phoneNumber);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// create a sub account. This account can chat with other sub accounts
        /// </summary>
        /// <param name="parentId">id of the parent</param>
        /// <param name="username">username</param>
        /// <param name="password1">password</param>
        /// <param name="password2">password repeat</param>
        /// <param name="name">displayname</param>
        /// <param name="profileImage">image for the profile</param>
        /// <returns></returns>
        public List<string> CreateSubAccount(int parentId, string username, string password1, string password2, string name, byte[] profileImage)
        {
            return signalCommunicationService.CreateSubAccount(parentId, username, password1, password2, name, profileImage);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// login the user in
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        public List<string> Login(string username, string password)
        {
            return signalCommunicationService.Login(username, password);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// log the user out
        /// </summary>
        /// <param name="username">username</param>
        public void LogOut(string username)
        {
            signalCommunicationService.LogOut(username);
        }
    }
}
