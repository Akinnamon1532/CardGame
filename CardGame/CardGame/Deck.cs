using System;
using System.Data;
using System.Diagnostics;
using System.Linq;

/// <summary>
/// Author: Andrew Kinnamon
/// Date: 3/21/2023
/// Purpose/Description: This class creates and manages the deck
/// </summary>
namespace CardsProject
{
    internal class Deck
    {
        //add spaces to the names of the suits so that they take up the same ammount of space
        public const string spades = "Spades ";
        public const string clubs = "Clubs  ";
        public const string hearts = "Hearts ";
        public const string dimonds = "Dimonds";
        /* This is a private static variable to control the size of a deck*/
        private static readonly int numberOfSuit = 13;
        /* This is a static variable for the size of the deck*/
        public static readonly int deckSize = numberOfSuit * 4;
        /* This is a private static readonly linked list that holds all the card objects */
        public static LinkedList<Cards> deck = new();


        /// <summary>
        /// This method creates decks of 52 cards with 4 suits and puts them in the linked list
        /// </summary>
        /// <param name="numberOfDecks">The number of decks that need to be made</param>
        public static void BuildDeck(int numberOfDecks = 1)
        {
            //safe guard against negative values and 0
            if (numberOfDecks < 1)
            {
                numberOfDecks = 1;
            }

            //loop through building a deck for however many decks you are building
            int value = 1;
            for (int i = 1; i <= numberOfDecks; i++)
            {
                //use a boolean to create the first card in a new deck
                Boolean newDeck = true;
                //loop through and create a deck
                while (newDeck == true || deck.Count % deckSize > 0)
                {
                    //everytime the value gets higher than the number of cards in a suit, reset it to 1
                    if (value > numberOfSuit)
                    {
                        value = 1;
                    }

                    //cycle through the suits
                    string Suit;
                    if (deck.Count == 0 || deck.Count % deckSize < numberOfSuit)
                    {
                        Suit = spades;
                    }
                    else if (deck.Count % deckSize >= numberOfSuit && deck.Count % deckSize < numberOfSuit * 2)
                    {
                        Suit = clubs;
                    }
                    else if (deck.Count % deckSize >= numberOfSuit * 2 && deck.Count % deckSize < numberOfSuit * 3)
                    {
                        Suit = hearts;
                    }
                    else
                    {
                        Suit = dimonds;
                    }

                    //using the suit and value, create the card and add it to the deck
                    Cards card = Cards.CreateCard(value, Suit);
                    deck.AddLast(card);
                    value++;
                    newDeck = false;
                }
            }
        }


        /// <summary>
        /// This method puts the cards in a random order
        /// </summary>
        public static void Shuffle()
        {
            //move the cards to an array
            Cards[] tempDeck = deck.ToArray();
            //randomize the array
            Random random = new();
            tempDeck = tempDeck.OrderBy(x => random.Next()).ToArray();
            //move the cards back into a linked list
            deck = new LinkedList<Cards>(tempDeck);
        }


        /// <summary>
        /// This method gets top card and removes it from the beck
        /// </summary>
        /// <returns>Returns the top card</returns>
        public static Cards GetCard()
        {
            Cards topCard = deck.First();
            deck.RemoveFirst();
            return topCard;
        }

        
        /// <summary>
        /// This method recieves the number of cards to give, creates a linked list containing that number of cards, and returns it.
        /// </summary>
        /// <param name="numCards">The number of cards that will be held by the linked list</param>
        /// <returns>Returns a linked list containing cards from the deck</returns>
        public static LinkedList<Cards> Deal(int numCards)
        {
            LinkedList<Cards> hand = new();
            //use a for loop to fill the linked list
            for (int i = 1; i <= numCards; i++)
            {
                hand.AddLast(GetCard());
            }
            return hand;
        }
    }
}
