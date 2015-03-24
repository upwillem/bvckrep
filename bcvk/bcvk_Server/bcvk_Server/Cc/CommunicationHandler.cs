using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bu;

namespace Cc
{
    public class CommunicationHandler
    {
        public static ConcurrentBag<Connection> Connections = new ConcurrentBag<Connection>();

        /// <summary>
        /// During this method a call is set ready to be established.
        /// </summary>
        /// <param name="sender">call owner who is the owner of the call</param>
        /// <param name="recipient">call recipient who is invited to the call</param>
        /// <returns>callId</returns>
        public static int DoCall(string sender, string recipient)
        {
            Connection con = new Connection(sender);
            con.AddParticipant(recipient);
            Connections.Add(con);
            return con.id;
        }

        /// <summary>
        /// During this methode the state of a specific connnection is checked
        /// </summary>
        /// <param name="callId">id to identify a specific call</param>
        /// <returns>connectionstate</returns>
        public static string GetConnetionState(int callId)
        {
            Connection con = Connections.Single(x => x.id == callId);
            return con.ConnectionState;
        }



  
    }
}
