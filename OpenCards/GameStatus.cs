using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCards
{
    [Serializable]
    public class GameStatus
    {
        public BlackCard currentBlack = new BlackCard();
        public List<Player> Players = new List<Player>();
        public List<Response> Responses = new List<Response>();
        public bool GameOver = false;

        public GameStatus()
        { }
    }
}
