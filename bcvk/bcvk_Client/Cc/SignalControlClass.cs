using Bu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cc
{
    public class SignalControlClass
    {
        private SignalCommunicationService signalCommunicationService;

        /// <summary>
        /// Constructor
        /// </summary>
        public SignalControlClass() 
        {
            signalCommunicationService = new SignalCommunicationService();
        }
    }
}
