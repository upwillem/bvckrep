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
        /*Connection status:
         * -connected       able to send and receive data
         * -disconnected    unable to send and receive date client disconnected or refused to join a connection
         * -connection      invited to join a connection                
         */ 

        //initial maker of the connection;
        private string owner;
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
            bool added = Participants.TryAdd(sender, "connected");
            if (added)
            {
                owner = sender;
                //TODO: Connection toevoegen aan het account
                addConnectionToAccount(sender);
            }       
        }             
        /// <summary>
        /// Add a client to a list of connection participants 
        /// </summary>
        /// <param name="recipient"></param>
        public void AddParticipant(string recipient)
        {
           bool added = Participants.TryAdd(recipient, "calling");
           if (added)
           {
               addConnectionToAccount(recipient);
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
                //Account acc = Account.GetAccountById(accountId);
                //if (acc == null)
                //{
                //    throw new NullReferenceException("unable to find account");
                //}
                //acc.Connections.Add(this);
            }
            catch (NullReferenceException nullexc)
            {
                ///log nulexc.msg
            }

            
        }
    }
}
