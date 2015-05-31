using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenCards
{
    [Serializable]
    public class Player
    {
        public String Name = "";
        public int Score = 0;
        public String Status = "";
        public String IP;
        public int Port;
        [XmlIgnore] public Response Response;
        
        public Player()
        { }

        public Player(String name)
        {
            Name = name;
        }

        public override String ToString()
        {
            String result = "";

            result += Convert.ToString(Score);
            result += "\t: ";
            result += Name;
            result += "\t";
            result += Status;

            return result;
        }
    }
}
