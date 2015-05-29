using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Network
{
    public class Message
    {
        public Object Payload;
        public Type PayloadType;
        public String Sender;

        public Message()
        {
            Payload = "";
            PayloadType = typeof(String);
            Sender = "";
        }
    }
}
