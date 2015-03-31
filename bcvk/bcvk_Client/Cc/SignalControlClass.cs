﻿using Bu;
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
        
        /// <summary>
        /// Constructor
        /// </summary>
        public SignalControlClass(string username)
        {
            //create singleton object
            AccountData acc = AccountData.Instance;
            acc.Username = username;

            signalCommunicationService = new SignalCommunicationService(username);
            signalCommunicationService.accountDataListReady += signalCommunicationService_accountDataListReady;
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// creates the thread of polling the accountdata
        /// </summary>
        /// <param name="username">username of the current user</param>
        public void StartPoll()
        {
            Thread pollThread = new Thread(() => signalCommunicationService.PollAccountData(AccountData.Instance.Username));
            pollThread.Start();
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// </summary>
        /// <param name="accountDataList"></param>
        private void signalCommunicationService_accountDataListReady(List<string> accountDataList)
        {
            foreach (string data in accountDataList)
            {
                #region switch cases accountdatalist
                switch (data.Split(';')[0])
                {
                    case "accountId":
                        AccountData.Instance.AccountId = data.Split(';')[1];
                        break;
                    case "parentId":
                        AccountData.Instance.ParentId = data.Split(';')[1];
                        break;
                    case "username":
                        AccountData.Instance.Username = data.Split(';')[1];
                        break;
                    case "email":
                        AccountData.Instance.Email = data.Split(';')[1];
                        break;
                    case "displayName":
                        AccountData.Instance.DisplayName = data.Split(';')[1];
                        break;
                    case "phonenumber":
                        AccountData.Instance.Phonenumber = data.Split(';')[1];
                        break;
                    case "photo":
                        AccountData.Instance.Photo = data.Split(';')[1];
                        break;
                    case "log":
                        AccountData.Instance.Log = data.Split(';')[1];
                        break;
                    case "children":
                        AccountData.Instance.Children = data.Split(';')[1];
                        break;
                    case "contact":
                        AccountData.Instance.Contacts = data.Split(';')[1];
                        break;
                    default:
                        break;
                }
                #endregion
            }
        }
    }
}
