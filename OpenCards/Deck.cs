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
        public String Version = "";
        public String Author = "";
        public String Fingerprint = "";
        public String MergeChain = "";
        public List<Card> WhiteCards = new List<Card>();
        public List<BlackCard> BlackCards = new List<BlackCard>();
        public List<String> MergedTitles = new List<String>();
        
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
            MergedTitles.Add(booster.Title);
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

        //Remove a white card
        public void Discard(Card card)
        {
            WhiteCards.Remove(card);
        }

        //Remove a black card
        public void Discard(BlackCard card)
        {
            BlackCards.Remove(card);
        }

        //empty all cards from the deck
        public void Clear()
        {
            WhiteCards.Clear();
            BlackCards.Clear();
        }

        //empty all white cards from the deck
        public void ClearWhite()
        {
            WhiteCards.Clear();
        }

        //empty all black cards from the deck
        public void ClearBlack()
        {
            BlackCards.Clear();
        }

    }
}
