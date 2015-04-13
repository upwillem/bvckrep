using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bu;

namespace Cc
{
    /// <summary>
    /// OWNER: Aron Huntjens 1209361
    /// This class instanctiate a connection. A connection is a class maintain all participants in a connection to send and receive data from and to eachother.
    /// </summary>
    public class CommunicationHandler
    {
        /// <summary>
        /// connection memory
        /// </summary>
        public static List<Connection> Connections = new List<Connection>();

        /// <summary>
        /// During this method a call is set ready to be established.
        /// </summary>
        /// <param name="sender">call owner who is the owner of the call</param>
        /// <param name="recipient">call recipient who is invited to the call</param>
        /// <returns>callId</returns>
        public static string DoConnect(string sender, string recipient)
        {
            Connection con = new Connection(sender);
            con.AddParticipant(recipient);
            Connections.Add(con);
            //TODO:Notify parents
            return con.Id;
        }
        /// <summary>
        /// During this methode the state of a specific connnection is checked
        /// </summary>
        /// <param name="callId">id to identify a specific call</param>
        /// <returns>connectionstate</returns>
        public static string GetConnectionState(string connectionId)
        {
            var con = Connections.Single(x => x.Id == connectionId);
            var status = "connectionended";
            if (con != null)
            {
                status=con.GetConnectionState();;
            }
            return status;
        }
        /// <summary>
        /// this method gives the connectionstate of a specific user in a connection
        /// </summary>
        /// <param name="connectionId">connection identificationcode</param>
        /// <param name="participant">indentification token to get the state of</param>
        /// <returns>connection state of a specific user </returns>
        public static string GetConnectionState(string connectionId, string participant)
        {
            var con = Connections.Single(x => x.Id == connectionId);
            string status = "connectionended";
            if (con != null)
            {
                status=con.GetConnectionState(participant);
            }
            return status;
        }       
        /// <summary>
        /// Giv a respond to a connection 
        /// </summary>
        /// <param name="sender">sender id</param>
        /// <param name="connectionId">connection identification token</param>
        /// <param name="answer">anwser</param>
        public static void AnwserConnection(string sender, string connectionId, string answer)
        {
            Connection connection = Connections.Single(x => x.Id == connectionId);
            if (connection == null)
            {
                return;
            } 
            connection.ChangeConnectionState(sender, answer);            
        }
        /// <summary>
        /// end a specific call
        /// </summary>
        /// <param name="connectionId">call to end</param>
        public static void EndConnection(string connectionId)
        {
            Connection connection = Connections.Single(x => x.Id == connectionId);
            if (connection == null)
            {
                return;
            }
            connection.EndConnection();
            Connections.Remove(connection);           
            
        }             
        /// <summary>
        /// this methode gets a stream
        /// </summary>
        /// <param name="connectionId">identifies a connection</param>
        /// <param name="recipient">who's stream</param>
        /// <param name="sender">who wants to get a stream</param>
        /// <param name="audio">audio identification</param>
        /// <returns></returns>
        public static List<byte[]> GetStream(string connectionId, string recipient, string sender, bool audio)
        {
            var connection = Connections.Single(X => X.Id == connectionId);
            return connection.GetStream(sender, recipient, connectionId, audio);
        }
       /// <summary>
       /// this methode sets a stream
       /// </summary>
       /// <param name="sender">who wants to set a stream (stream owner)</param>
       /// <param name="recipient">stream recipient</param>
       /// <param name="stream">stream to set</param>
       /// <param name="connectId">connection identification</param>
       /// <param name="audio">audio identification</param>
        public static void SetStream(string sender, string recipient, List<byte[]> stream, string connectId, bool audio)
        {
            var connection = Connections.Single(X => X.Id == connectId);
            connection.SetStream(sender, recipient, stream, connectId, audio);
        }
        /// <summary>
        /// this methode sets a videomessage
        /// </summary>
        /// <param name="sender">who is sending the message</param>
        /// <param name="recipient">who is receiving the message</param>
        /// <param name="video">video to send</param>
        /// <param name="connectId">sets a video in a connection (optional)</param>
        /// <param name="audio">audio identification</param>
        public static void SetVideo(string sender, string recipient, List<byte[]> video, string connectId, bool audio)
        {
            Connection.SetVideo(sender, recipient, video, connectId, audio);
        }
        /// <summary>
        /// this methode gets a videomessage
        /// </summary>
        /// <param name="sender">who wants to get a videomessage</param>
        /// <param name="recipient">who's videomessage</param>
        /// <param name="connectId">gets a video of connection (optiona)</param>
        /// <param name="audio">audio identification</param>
        /// <returns></returns>
        public static List<byte[]> getVideo(string sender, string recipient, string connectId, bool audio)
        {
            return Connection.GetVideo(sender, recipient, connectId, audio);
        }
    }
}
