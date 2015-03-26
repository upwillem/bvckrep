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
            //TODO:Notify parents
            return con.id;
        }
        /// <summary>
        /// During this methode the state of a specific connnection is checked
        /// </summary>
        /// <param name="callId">id to identify a specific call</param>
        /// <returns>connectionstate</returns>
        public static string GetConnetionState(int callId)
        {
            var con = Connections.Single(x => x.id == callId);
            return con.ConnectionState;
        }
        /// <summary>
        /// Set the stream availble in a connection
        /// </summary>
        /// <param name="callId">connection identification token</param>
        /// <param name="streamowner">sender of the stream</param>
        /// <param name="stream">stream in list of a bytearray</param>
        public static void SetStream(int callId,string streamowner, List<byte[]> stream)
        {
            var con = Connections.Select(x => x.id == callId);
            Connection connection = (Connection)con;
            connection.SetStream(streamowner, stream);
        }        
        /// <summary>
        /// Gets a specific stream of a connnection
        /// </summary>
        /// <param name="callId">connection identification token</param>
        /// <param name="streamowner">identefies wich stream to return</param>
        /// <returns>specific stream from streamowner</returns>
        public static List<byte[]> GetStream(int callId, string streamowner)
        {
            Connection connection = FindConnectionById(callId);
            List<byte[]>returnstream = new List<byte[]>();
            connection.Streams.TryGetValue(streamowner, out returnstream);
            return returnstream;
        }
        
        public static void AnwserCall(string sender, int callId, string answer)
        {
            var con = Connections.Select(x => x.id == callId);
            Connection connection = (Connection)con;
            connection.ChangeConnectionState(sender, answer);            
        }

        public static void EndCall(int callId)
        {
            Connection connection = FindConnectionById(callId);
            if (connection.EndConnection())
            {
                Connections.TryTake(out connection);                
            }

        }

        private static Connection FindConnectionById(int callId)
        {
            var con = Connections.Select(x => x.id == callId);
            Connection connection = (Connection)con;
            return connection;
        }
    }
}
