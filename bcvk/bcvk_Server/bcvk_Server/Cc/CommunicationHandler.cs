﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bu;

namespace Cc
{
    /// <summary>
    /// Aron Huntjens 1209361
    /// </summary>
    public class CommunicationHandler
    {
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
        /// <param name="who">indentification token to get the state of</param>
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
        /// <param name="callId">connection identification token</param>
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
        /// <param name="callId">call to end</param>
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
        /// sets a stream
        /// </summary>
        /// <param name="connectionId">connection identificatino token</param>
        /// <param name="video">stream to set</param>
        /// <param name="audio">audio identifier</param>
        public static void SetStream(string connectionId, List<byte[]>video, bool audio)
        {
            Connection connection = Connections.Single(x => x.Id == connectionId);
            if (connection != null)
            {
                connection.SetStream(video, audio);
            }
        }

        /// <summary>
        /// gets a specific stream
        /// </summary>
        /// <param name="connectionId">connection Identificatino token</param>
        /// <param name="audio">audio idientifier</param>
        /// <returns></returns>
        public static List<byte[]> GetStream(string connectionId, bool audio)
        {
            List<byte[]> stream = new List<byte[]>();
            Connection connection = Connections.Single(x => x.Id == connectionId);
            if (connection != null)
            {
                stream = connection.GetStream(audio);
            }
            return stream;
        }


    }
}
