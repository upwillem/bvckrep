using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;

namespace Bu
{
    /// <summary>
    /// OWNER: Aron Huntjens 1209361
    /// This class instanctiate a connection. A connection is a class maintain all participants in a connection to send and receive data from and to eachother.
    /// </summary>
    public class Connection
    {

        //TODO MUTEX IMPLEMENTEREN

        /*Connection state for specific users
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
                    break;
                }
            }
            if (connected < 2)
            {
                ConnectionState = "connectionended";
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
            Mysql mysql = new Mysql();
            foreach (var item in Participants)
            {
                Participants.TryUpdate(item.Key, "disconnected", null);
                mysql.Query("DELETE FROM connections_users WHERE connection_id='" + Id + "'");
            }          
        }


        private void addConnectionToAccount(string participant)
        {
            Mysql mysql = new Mysql();
            mysql.Query("INSERT INTO connections_users(connection_id,user_id) VALUES('" + Id + "','"+ participant +"')");
            
            
        }
    }
}
