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

        public override String ToString()
        {
            String result = "================ Game Status ===============\n";

            result += currentBlack.ToString();
            result += "\n--------------------------------------------\n";

            foreach (Response response in Responses)
            {
                result += response.ToString();
            }

            result += "\n--------------------------------------------\n";

            //Players.Sort();
            Players.Sort((x, y) => y.Score.CompareTo(x.Score));

            foreach (Player player in Players)
            {
                result += player.ToString();
                result += "\n";
            }



            return result;
        }
    }
}
