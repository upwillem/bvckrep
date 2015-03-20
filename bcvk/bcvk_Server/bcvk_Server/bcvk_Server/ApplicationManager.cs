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

        public int DoCall(string sender, string recipient)
        {
            throw new NotImplementedException();
        }

        public void AnswerCall(string sender, string recipient, int callId, string answer)
        {
            throw new NotImplementedException();
        }

        public string GetCallStatus(int callId)
        {
            throw new NotImplementedException();
        }

        public void EndCall(string sender, string recipient, int callId)
        {
            throw new NotImplementedException();
        }

        public bool ToggleBlock(string sender, string recipient)
        {
            throw new NotImplementedException();
        }

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
    }
}
