using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Author: Andrew Kinnamon
/// Date: 3/21/2023
/// Purpose/Description: This class holds the cards attributes
/// </summary>
namespace CardsProject
{
    public class Cards
    {
        /* This atribute is the numerical value of a given card */
        private int Value { get; set; }
        /* This atribute is the suit of a given card */
        private string Suit { get; set; } = "";


        /// <summary>
        /// This method recieves the value and suit for a card and creates the card
        /// </summary>
        /// <param name="value">The number value of the card</param>
        /// <param name="suit">The suit the card belongs to</param>
        /// <returns>Returns the created card</returns>
        public static Cards CreateCard(int value, string suit)
        {
            //construct the card
            Cards card = new()
            {
                Value = value,
                Suit = suit,
            };
            return card;
        }


        /// <summary>
        /// This method takes a card and returns that cards value
        /// </summary>
        /// <param name="card">The card whose value will be returned</param>
        /// <returns>Returns the value of the card</returns>
        public static int GetCardValue(Cards card)
        {
            return card.Value;
        }


        /// <summary>
        /// This method takes a card and returns that cards suit
        /// </summary>
        /// <param name="card">The card whose suit will be returned</param>
        /// <returns>Returns the suit of the card</returns>
        public static string GetCardSuit(Cards card)
        {
            return card.Suit;
        }


        /// <summary>
        /// This method takes a card and returns a string that lists its value and suit
        /// </summary>
        /// <param name="card">The card that the string will describe</param>
        /// <returns>Returns a string that lists its value and suit</returns>
        public static string CardToString(Cards card)
        {
            //build the string for each card, use spaces to make each string the same length
            string value = "  " + Convert.ToString(card.Value) + "  ";
            switch (card.Value)
            {
                case 1:
                    value = " Ace ";
                    break;
                case 10:
                    value = "  10 ";
                    break;
                case 11:
                    value = " Jack";
                    break;
                case 12:
                    value = "Queen";
                    break;
                case 13:
                    value = " King";
                    break;
            }
            string suit = card.Suit;
            return value + " of " + suit;
        }


        /// <summary>
        /// This method takes an array of cards and sorts them by the cards value
        /// </summary>
        /// <param name="cards">The array of card that is getting sorted</param>
        /// <returns>Returns a sorted array of cards</returns>
        public static Cards[] SortArrayOfCards(Cards[] cards)
        {
            //use the built in sort method to sort the array
            cards = cards.OrderBy(card => card.Value).ToArray();
            return cards;
        }
    }
}