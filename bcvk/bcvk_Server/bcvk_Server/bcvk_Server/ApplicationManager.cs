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
        public List<string> CreateMainAccount(string username, string password1, string password2, string email, string name)
        {
            //throw new NotImplementedException();
            AccountHandler.CreateMainAccount(username, password1, password2, email, name);
            return new List<string>();
        }

        public List<string> CreateSubAccount(string username, string password1, string password2, string name, byte[] profileImage)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
        public bool ToggleBlock(string sender, string recipient)
        {
            throw new NotImplementedException();
        }
    #endregion

    #region callsignaling

        public string DoCall(string sender, string recipient)
        {
            return CommunicationHandler.DoConnect(sender, recipient);
        }

        public void AnswerCall(string sender, string recipient, string connectionId, string answer)
        {
            CommunicationHandler.AnwserConnection(sender, connectionId, answer);          
        }

        ///
        public string GetCallStatus(string connectionId)
        {
            return CommunicationHandler.GetConnetionState(connectionId);
        }

        public string GetParticipantCallStatus(string connectionId, string participant)
        {
            return CommunicationHandler.GetConnetionState(connectionId,participant);
        } 

        public void EndCall(string sender, string recipient, string connectionId)
        {  
            CommunicationHandler.EndConnection(connectionId);
        }
        #endregion

    #region stream
        public void SendStream(string sender, string recipient, List<byte[]> stream)
        {
            throw new NotImplementedException();
        }

        public void SendVideo(string sender, string recipient, List<byte[]> video)
        {
            throw new NotImplementedException();
        }

        public byte[] GetStream(string sender, string recipient)
        {
            throw new NotImplementedException();
        }

        public byte[] GetVideo(string sender, string recipient)
        {
            throw new NotImplementedException();
        }
        #endregion 
    }
}
