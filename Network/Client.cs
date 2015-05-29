using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Network
{
    [Serializable]
    public class Client
    {
        public String ExternalIP;
        [XmlIgnore] public String ServerIP;
        public int ExternalPort;
        private int ServerPort;

        public Client()
        {
            ServerPort = 3000;
            ExternalIP = getExternalIp();
        }

        public Client(int port)
        {
            ServerPort = port;
            ExternalIP = getExternalIp();
        }

        // Send a message to the given client
        public void SendMessage(Object obj)
        {
            String s = "There has been an error";
            s = SerializeObjectXML(obj);
            TcpClient client = new TcpClient();


            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ServerIP), ServerPort);

            client.Connect(serverEndPoint);

            NetworkStream clientStream = client.GetStream();

            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(s);
            //Console.WriteLine("Sent: " + s);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }


        //Utility methods
        public static String SerializeObjectXML(Object obj)
        {
            StringWriter stringwriter = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            try
            {
                serializer.Serialize(stringwriter, obj);
                return stringwriter.ToString();
            }
            catch
            {
                //TODO: Add error catching code
            }

            return null;
        }


        private string getExternalIp()
        {
            try
            {
                string externalIP;
                externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                             .Matches(externalIP)[0].ToString();
                return externalIP;
            }
            catch { return null; }
        }
    }
}
