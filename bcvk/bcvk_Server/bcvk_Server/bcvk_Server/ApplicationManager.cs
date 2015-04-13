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
            Console.WriteLine(username + " requests for account data"); 
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
            Console.WriteLine(sender + " tries to connect with " + recipient);
            string connectionId=CommunicationHandler.DoConnect(sender, recipient);
            Console.WriteLine("connectionId= "+connectionId);
            return connectionId;
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
        /// <summary>
        /// Get the status from a specific participant in a specific caal
        /// </summary>
        /// <param name="connectionId">connection identificatino token</param>
        /// <param name="participant">participant to get status from</param>
        /// <returns></returns>
        public string GetParticipantCallStatus(string connectionId, string participant)
        {
            return CommunicationHandler.GetConnectionState(connectionId,participant);
        } 
        /// <summary>
        /// end a call
        /// </summary>
        /// <param name="sender">who ants to end the call</param>
        /// <param name="recipient">who to end in the call</param>
        /// <param name="connectionId">connection identification token, to identify a connection to end</param>
        public void EndCall(string sender, string recipient, string connectionId)
        {  
            CommunicationHandler.EndConnection(connectionId);
        }
        #endregion

    #region stream
        /// <summary>
        /// This methode is used to send a stream
        /// </summary>
        /// <param name="sender">who is sending the stream</param>
        /// <param name="recipient">who is receiving the stream</param>
        /// <param name="stream">stream to send</param>
        /// <param name="connectId">connection identification token</param>
        /// <param name="audio">audio identification</param>
        public void SendStream(string sender, string recipient, List<byte[]> stream, string connectId, bool audio)
        {
            Console.WriteLine("account: "+ sender + " sends a stream");
            CommunicationHandler.SetStream(sender, recipient, stream, connectId, audio);
        }
        /// <summary>
        /// this methode is used to send a video
        /// </summary>
        /// <param name="sender">who is sending</param>
        /// <param name="recipient">who is receiving</param>
        /// <param name="video">video to send</param>
        /// <param name="connectId">connection identification token (optional)</param>
        /// <param name="audio">audio identification</param>
        public void SendVideo(string sender, string recipient, List<byte[]> video, string connectId, bool audio)
        {
            CommunicationHandler.SetVideo(sender, recipient, video, connectId, audio);
        }

        /// <summary>
        /// method used to get a stream
        /// </summary>
        /// <param name="sender">who wants to get a stream</param>
        /// <param name="recipient">who's stream to receive </param>
        /// <param name="connectId">stream from wich connection</param>
        /// <param name="audio">audio identification</param>
        /// <returns></returns>
        public List<byte[]> GetStream(string sender, string recipient, string connectId, bool audio)
        {
            Console.WriteLine("account: "+sender + " gets a stream");
            List<byte[]> list = new List<byte[]>();
            list = CommunicationHandler.GetStream(connectId, recipient, sender, audio);
            return list;
        }
        /// <summary>
        /// this methode is used to get a specific videomessage
        /// </summary>
        /// <param name="sender">who wants to get a video</param>
        /// <param name="recipient">get the video from</param>
        /// <param name="connectId">specific videomessage identification token</param>
        /// <param name="audio">audio identification</param>
        /// <returns></returns>
        public List<byte[]> GetVideo(string sender, string recipient, string connectId, bool audio)
        {
            return CommunicationHandler.getVideo(sender, recipient, connectId, audio);
        }
        #endregion 

    }
}
