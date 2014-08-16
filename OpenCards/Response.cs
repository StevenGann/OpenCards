using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCards
{
    [Serializable]
    public class Response
    {
        public int Size = 0;
        public List<Card> Cards = new List<Card>();
        public bool IsFromCzar = false;

        public Response()
        { }

        public void Add(Card newCard)
        {
            Cards.Add(newCard);
        }
    }
}
