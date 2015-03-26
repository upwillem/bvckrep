using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bu
{
    /// <summary>
    /// OWNER: Aron Huntjens 1209361
    /// This class instanctiate a connection. A connection is a class maintain all participants in a connection to send and receive data from and to eachother.
    /// </summary>
    public class Connection
    {
        /*Connection state
         * -connected       in current connection
         * -disconnected    left or refused to join a connection
         * -connecting      invited to join a connection
         * -establishing    not enough members to join a connection
         * -established     able to send and receive data
         * -connectionended connection ended
         */ 

        //initial maker of the connection;
        private string owner;
        /// <summary>
        /// List of all streams in connection 
        /// </summary>
        public ConcurrentDictionary<string, List<byte[]>> Streams { get; private set; }

        /// <summary>
        /// Current state of the connection
        /// </summary>
        public string ConnectionState { get; private set; }


        /// <summary>
        /// Unique connection idtentification token
        /// </summary>
        public int id { get; private set; }
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
            id = Hash();
            Participants = new ConcurrentDictionary<string, string>();
            Streams = new ConcurrentDictionary<string, List<byte[]>>();
            bool added = Participants.TryAdd(sender, "connected");
            if (added)
            {
                owner = sender;              
                addConnectionToAccount(sender);
                ConnectionState = "establishing";
            }       
        }             
        /// <summary>
        /// Add a client to a list of connection participants 
        /// </summary>
        /// <param name="recipient"></param>
        public void AddParticipant(string recipient)
        {
           bool added = Participants.TryAdd(recipient, "connecting");
           if (added)
           {
               addConnectionToAccount(recipient);
           }
        }

        /// <summary>
        /// set the stream availble in the connection for participants
        /// </summary>
        /// <param name="steamowner">sender of the stream</param>
        /// <param name="stream">the list byte array stream</param>
        public void SetStream(string steamowner, List<byte[]> stream)
        {
            Streams.AddOrUpdate(steamowner, stream, null);
        }

        public void ChangeConnectionState(string sender, string answer)
        {
            Participants.AddOrUpdate(sender, answer,null);
                                    
            //check connectionstate 
            int connected = 0;
            foreach (var con in Participants)
            {
                if (con.Value == "connected")
                    connected++;
                if (connected >= 2)                {
                    ConnectionState = "established";
                    break;
                }
            }
            if (connected < 2)
            {
                ConnectionState = "connectionended";
            }
            
            
        }

        /// <summary>
        /// generate unique hash for each call to identify 
        /// </summary>
        /// <returns></returns>
        private static int Hash()
        {
            DateTime datetime = System.DateTime.Now;            
            int hash = (int)datetime.Ticks;
            hash = hash * hash.GetHashCode();
            return hash;
        }
        /// <summary>
        /// Add an instance of this connection to an instance of an account
        /// </summary>
        /// <param name="accountId"></param>
        private void addConnectionToAccount(string accountId)
        {
            throw new NotImplementedException();
            try
            {
                //TODO:
                //Account.AddConnection(accountId, this.id) 
            }
            catch(NullReferenceException nullex)
            {
                //nullreference exception
            }      
                      
        }
        public bool EndConnection()
        {
            foreach (var item in Participants)
            {
                Participants.TryUpdate(item.Key, "disconnected", null);                
            }
            return true;
        }
    }
}
