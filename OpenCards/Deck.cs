using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCards
{
    [Serializable]
    public class Deck
    {
        public List<Card> WhiteCards = new List<Card>();
        public List<BlackCard> BlackCards = new List<BlackCard>();
        
        public Deck()
        { }

        //Add a deck to this deck
        public void Add(Deck booster)
        {
            WhiteCards.AddRange(booster.WhiteCards);
            BlackCards.AddRange(booster.BlackCards);
        }

        //Add a white card to this deck
        public void Add(Card booster)
        {
            WhiteCards.Add(booster);
        }

        //Add a black card to this deck
        public void Add(BlackCard booster)
        {
            BlackCards.Add(booster);
        }
    }
}
