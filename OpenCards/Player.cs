using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCards
{
    [Serializable]
    public class Player
    {
        public String Name = "";
        public int Score = 0;
        public String Status = "";
        
        public Player()
        { }

        public Player(String name)
        {
            Name = name;
        }
    }
}
