using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Network
{
    class Message
    {
        private string    objectType;
        private object    message;
        private string    timeSent;
        private string    timeReceived;
        public  TcpClient recipient;
        public  int       ping;


        public Message()
        { }

        public Message(TcpClient recip, string type, object msg)
        {
            recipient = recip;
            objectType = type;
            message = msg;
        }

        public void setObjectType(string type)    { objectType = type; }
        public void setMessage(object msg)        { message = msg; }
        //public void setTimeSent();
    }
}
