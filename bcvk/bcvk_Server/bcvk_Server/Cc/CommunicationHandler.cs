using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bu;

namespace Cc
{
    public class CommunicationHandler
    {
        public List<Connection> Connections = new List<Connection>();

        public static int DoCall(string sender, string recipient)
        {
            Connection con = new Connection(sender);
            con.AddParticipant(recipient);        
            return con.id;
        }

        public static string GetCallStatus(int callId, string id)
        {
            //Account acc = Account.GetAccountById(id);
            
            //need account for this;


            return "";
        }

        
    }
}
