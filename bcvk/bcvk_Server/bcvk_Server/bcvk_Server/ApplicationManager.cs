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
        public void CreateMainAccount(string username, string password1, string password2, string email, string name)
        {
            //throw new NotImplementedException();
            AccountHandler.CreateMainAccount(username, password1, password2, email, name);
        }

        public void CreateSubAccount(string username, string password1, string password2, string name, byte[] profileImage)
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

        public int DoCall(string sender, string recipient)
        {
            return -1;
            CommunicationHandler.DoConnect(sender, recipient);
        }

        public void AnswerCall(string sender, string recipient, int connectionId, string answer)
        {
            //replace -1 with connectionId
            CommunicationHandler.AnwserConnection(sender, "-1", answer);          
        }

        ///
        public string GetCallStatus(int connectionId)
        {
            return "";
            //replace -1 with connectionId
            CommunicationHandler.GetConnetionState("-1");
        }

        //TODO: overload GetCallStatus from specific Id

        public void EndCall(string sender, string recipient, int connectionId)
        {
            //replace -1 with connectionId
            CommunicationHandler.EndConnection("-1");
        }
        #endregion

    #region stream
        public void SendStream(string sender, string recipient, byte[] stream)
        {
            throw new NotImplementedException();
        }

        public void SendVideo(string sender, string recipient, byte[] video)
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
