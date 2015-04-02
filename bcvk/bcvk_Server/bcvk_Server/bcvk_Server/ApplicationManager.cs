using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift;
using bcvkSignal;
using bcvkStream;
using Cc;

namespace bcvk_Server
{
    public class ApplicationManager: Signal.Iface, Stream.Iface
    {
    #region account
        public List<string> CreateMainAccount(string username, string password1, string password2, string email, string name, string phoneNumber)
        {
            //throw new NotImplementedException();
            AccountHandler.CreateMainAccount(username, password1, password2, email, name, phoneNumber);
            return new List<string>();
        }

        public List<string> CreateSubAccount(int parentId, string username, string password1, string password2, string name, byte[] profileImage)
        {
            return AccountHandler.CreateSubAccount(parentId, username, password1, password2, name, profileImage);
        }

        public List<string> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void LogOut(string username)
        {
            throw new NotImplementedException();
        }

        public List<string> GetAccountData(string username)
        {
            return AccountHandler.GetAccountData(username);
        }

        public bool ToggleBlock(string sender, string recipient)
        {
            throw new NotImplementedException();
        }
        public bool AcceptContact(string sender, string recipient)
        {
            return false;
        }
        public bool DeleteContact(string sender, string recipient)
        {
            return false;
        }
        public bool AddContact(string sender, string recipient)
        {
            return false;
        }
    #endregion

    #region callsignaling

        /// <summary>
        /// Try to establish a new call as an connection
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="recipient">recepient</param>
        /// <returns>connectionId</returns>
        public string DoCall(string sender, string recipient)
        {
            return CommunicationHandler.DoConnect(sender, recipient);
        }

        /// <summary>
        /// This method is to give a response to a specific call
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="recipient">recipient not always required</param>
        /// <param name="connectionId">connection identification token</param>
        /// <param name="answer">anwser to give as an respond</param>
        public void AnswerCall(string sender, string recipient, string connectionId, string answer)
        {
            CommunicationHandler.AnwserConnection(sender, connectionId, answer);          
        }
        /// <summary>
        /// Get the connetionState of a connection (established / establishing / connectionended)
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public string GetCallStatus(string connectionId)
        {
            return CommunicationHandler.GetConnectionState(connectionId);
        }

        public string GetParticipantCallStatus(string connectionId, string participant)
        {
            return CommunicationHandler.GetConnectionState(connectionId,participant);
        } 

        public void EndCall(string sender, string recipient, string connectionId)
        {  
            CommunicationHandler.EndConnection(connectionId);
        }
        #endregion

    #region stream
        public void SendStream(string sender, string recipient, List<byte[]> stream, string connectId, bool audio)
        {
            throw new NotImplementedException();
        }

        public void SendVideo(string sender, string recipient, List<byte[]> video, string connectId, bool audio)
        {
            throw new NotImplementedException();
        }

        public byte[] GetStream(string sender, string recipient, string connectId, bool audio)
        {
            throw new NotImplementedException();
        }

        public byte[] GetVideo(string sender, string recipient, string connectId, bool audio)
        {
            throw new NotImplementedException();
        }
        #endregion 
    }
}
