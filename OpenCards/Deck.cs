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
        public String Title = "";
        public List<Card> WhiteCards = new List<Card>();
        public List<BlackCard> BlackCards = new List<BlackCard>();

        
        public Deck()
        { }

        public Deck(String title)
        {
            Title = title;
        }

        //Add a deck to this deck
        public void Add(Deck booster)
        {
            WhiteCards.AddRange(booster.WhiteCards);
            BlackCards.AddRange(booster.BlackCards);
        }

        public void Add(Deck booster, String newTitle)
        {
            Add(booster);
            Title = newTitle;
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
