using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Dal;

namespace Bu
{
    /// <summary>
    /// OWNER: Aron Huntjens 1209361
    /// This class instanctiate a connection. A connection is a class maintain all participants in a connection to send and receive data from and to eachother.
    /// </summary>
    public class Connection
    {

        /*Connection state for specific participant in a connection
         * -connected       in current connection
         * -disconnected    left or refused to join a connection
         * -connecting      invited to join a connection
         * 
         * 
         * connectionstate for a connection
         * -establishing    not enough members to join a connection
         * -established     able to send and receive data
         * -connectionended connection ended
         */ 


        //initial maker of the connection;
        private string owner;

        private ConcurrentDictionary<string, List<byte[]>> audiostreams;
        private ConcurrentDictionary<string, List<byte[]>> videostreams;

        private System.Timers.Timer aTimer;

        /// <summary>
        /// Current state of the connection
        /// </summary>
        public string ConnectionState { get; private set; }
        /// <summary>
        /// Unique connection idtentification token
        /// </summary>
        public string Id { get; private set; }
        /// <summary>
        /// All participants in the connection
        /// </summary>   
        public ConcurrentDictionary<string, string> Participants { get; private set; }   
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="sender">Sender reference to identify the connection's owner</param>        
        public Connection(string sender)
        {
            Id = Guid.NewGuid().ToString();
            Participants = new ConcurrentDictionary<string, string>();            
            bool added = Participants.TryAdd(sender, "connected");
            if (added)
            {
                owner = sender;                
                addConnectionToAccount(sender);
                ConnectionState = "establishing";
            }
            audiostreams = new ConcurrentDictionary<string, List<byte[]>>();
            videostreams = new ConcurrentDictionary<string, List<byte[]>>();
            Logger.SetLog(Convert.ToInt32(owner), Logger.Activity.ConnectionMade);

            //Start timer
            StartConnectionTimeout();
        }

        /// <summary>
        /// OWNER: Ralph Lazarus 1227319
        /// </summary>
        private void StartConnectionTimeout()
        {
            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += ConnectionTimeout;
            aTimer.Interval = 60000;
            aTimer.Enabled = true;
        }

        /// <summary>
        /// OWNER: Ralph Lazarus 1227319
        /// </summary>
        /// <param name="source">timer objectsource</param>
        /// <param name="e">eventarguments</param>
        private void ConnectionTimeout(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("It is time to check the connection of {0} (60 seconds have passed)", Id);
            Console.WriteLine("Status: {0}", ConnectionState);

            if (ConnectionState != "established")
            {
                ConnectionState = "connectionended";
            }

            Console.WriteLine("New status: {0}", ConnectionState);

            aTimer.Stop();
        }
        
        
        /// <summary>
        /// Add a client to a list of connection participants 
        /// </summary>
        /// <param name="recipient"></param> 
        public void AddParticipant(string participant)
        {
           bool added = Participants.TryAdd(participant, "connecting");
           if (added)
           {
               addConnectionToAccount(participant);
               Logger.SetLog(Convert.ToInt32(owner), Logger.Activity.ConnectionAddedContact, Convert.ToInt32(participant));
           }

        }

        /// <summary>
        /// Change the current connection state
        /// </summary>
        /// <param name="sender">senderId</param>
        /// <param name="answer">anwser to the connection</param>
        public void ChangeConnectionState(string participant, string answer)
        {
            string oldValue;
            Participants.TryGetValue(participant, out oldValue);
            Participants.TryUpdate(participant, answer, oldValue);
                                    
            //check connectionstate 
            int connected = 0;
            foreach (var currentParticipant in Participants)
            {
                if (currentParticipant.Value == "connected")
                    connected++;
                if (connected >= 2)                {
                    ConnectionState = "established";
                    Logger.SetLog(Convert.ToInt32(owner), Logger.Activity.ConnectionEstablished);
                    break;
                }
            }
            if (connected < 2)
            {
                ConnectionState = "connectionended";
                Logger.SetLog(Convert.ToInt32(owner), Logger.Activity.ConnectionEnded);
            }          
            
        }
        /// <summary>
        /// This methode is capable of getting a threadsafe connectionstate
        /// </summary>
        /// <returns>current connectionstate of a connection</returns>
        public string GetConnectionState()
        {
            return ConnectionState;
        }
        /// <summary>
        /// This methode is capable of getting a threadsafe connectionstate of a specific participant
        /// </summary>
        /// <param name="participant">participant to get the connectionState from</param>
        /// <returns>connectionState of a specific participant</returns>
        public string GetConnectionState(string participant)
        {
            string result;
            Participants.TryGetValue(participant, out result);
            return result;

        }       

        /// <summary>
        /// set every participant to a disconnected state
        /// </summary>
        /// <returns>return true when correctly done</returns>
        public void EndConnection()
        {
            foreach (var item in Participants)
            {
                Participants.TryUpdate(item.Key, "disconnected", null);
                Mysql.Query("DELETE FROM connections_users WHERE connection_id='" + Id + "'");
            }          
        }

        /// <summary>
        /// add connection to account in DAL
        /// </summary>
        /// <param name="participant">particiapant to add to connection</param>
        private void addConnectionToAccount(string participant)
        {
            Mysql.Query("INSERT INTO connections_users(connection_id,user_id) VALUES('" + Mysql.MySQLEscape(Id) + "','"+ Mysql.MySQLEscape(participant) +"')");
        }

        /// <summary>
        /// this method sets a stream
        /// </summary>
        /// <param name="sender">sender who is setting the stream</param>
        /// <param name="recipient">recepient of the stream</param>
        /// <param name="stream">stream to set</param>
        /// <param name="connectId">current connectionId</param>
        /// <param name="audio">audio identification</param>
        public void SetStream(string sender, string recipient, List<byte[]> stream, string connectId, bool audio)
        {
            //check if connection id is same as current conenction id (security check)
            if (connectId != this.Id)
            {
                return;
            }

            if (audio)
            {
                List<byte[]> memberstream;               
                audiostreams.TryRemove(sender, out memberstream);                
                memberstream = stream;
                audiostreams.TryAdd(sender,memberstream);
            }
            else
            {
                List<byte[]> memberstream;
                videostreams.TryRemove(sender, out memberstream);                
                memberstream = stream;
                videostreams.TryAdd(sender, memberstream);
            }
        }

        /// <summary>
        /// gets a specific stream
        /// </summary>
        /// <param name="sender">who whants to get the stream</param>
        /// <param name="recipient">stream owner (optional who's stream to return )</param>
        /// <param name="connectionId">curren connection id</param>
        /// <param name="audio">audio identification</param>
        /// <returns>stream</returns>
        public List<byte[]> GetStream(string sender, string recipient, string connectionId, bool audio)
        {
            string otherPersonsId = "-1";
            //recipient is optional, default returns stream from first person who not is sender.
            if (recipient == "" || String.IsNullOrEmpty(recipient) || String.IsNullOrWhiteSpace(recipient))
            {
                 otherPersonsId = Participants.Keys.First(key => key != sender);
            }
            else
            {
                otherPersonsId = recipient;
            }


            List<byte[]> memberstream;
            
            //check if connection id is same as current conenction id (security check)
            if (connectionId != this.Id)
            {
                memberstream = new List<byte[]>();
                return memberstream;
            }

        
            if (audio)
            {
                if (!audiostreams.TryRemove(otherPersonsId, out memberstream))
                {
                    memberstream = new List<byte[]>();
                }
            }
            else
            {
                if (!videostreams.TryRemove(otherPersonsId, out memberstream))
                {
                    memberstream = new List<byte[]>();
                }
            }
            return memberstream;
        }

        /// <summary>
        /// This mehtod is used to set a video message.
        /// </summary>
        /// <param name="sender">who is setting the message</param>
        /// <param name="recipient">who is receiving the message</param>
        /// <param name="video">videomessage to set</param>
        /// <param name="connectId">optional connectionId for future purpose</param>
        /// <param name="audio">audio identification</param>
        public static void SetVideo(string sender, string recipient, List<byte[]> video, string connectId, bool audio)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// this method is used to get a specific videoMessage      
        /// </summary>
        /// <param name="sender">sender who wants to get the stream</param>
        /// <param name="recipient">original recipient from the videoMessage</param>
        /// <param name="connectId">connectId from the specific videoMessage to get</param>
        /// <param name="audio">audio identification</param>
        /// <returns></returns>
        public static List<byte[]> GetVideo(string sender, string recipient, string connectId, bool audio)
        {
            throw new NotImplementedException();
        }
    }
}
