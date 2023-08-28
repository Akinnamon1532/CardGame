using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Author: Andrew Kinnamon
/// Date: 3/22/2023
/// Purpose/Description: This class serves as a temporary player class
/// </summary>
namespace CardsProject
{
    internal class Player
    {
        /* This atribute is the hand of a given player */
        private LinkedList<Cards> Hand { get; set; } = new();
        /* This atribute is the number of cards in a players hand */
        private int HandSize { get; set; }
        /* This atribute is the players bid */
        private int Bid { get; set; }
        /* This atribute is whether or not the player is human */
        //private bool Human { get; set; }
        private bool Human { get; set; }
        /* This atribute is the players identifying number */
        private int PlayerNumber { get; set; }
        /* This atribute is the players current score */
        private int PlayerScore { get; set; }
        /* This atribute is the current number of rounds that the player has won */
        private int PlayerRoundsWon { get; set; }
        /* This atribute is the current number of bags the player has */
        private int Bags { get; set; }


        /// <summary>
        /// This method creates however many players that the parameter dictates
        /// </summary>
        /// <param name="numTotalPlayers">The total number of players that will be created</param>
        /// <param name="numHumanPlayers">The number of human players that will be created</param>
        /// <param name="handSize">The number of cards a player will hold</param>
        public static void CreatePlayers(int numTotalPlayers, int numHumanPlayers, int handSize = 0)
        {
            //if handSize is 0 or was left empty deal the entire deck
            bool wholeDeck = false;
            bool human = true;
            if (handSize == 0)
            {
                wholeDeck = true;
            }
            int cardsInDeck = Deck.deck.Count();
            handSize = cardsInDeck / numTotalPlayers;
            int temp;
            //take into consideration extra cards
            int extra = cardsInDeck - (handSize * numTotalPlayers);
            int humanCounter = 0;
            for (int i = 1; i <= numTotalPlayers; i++, humanCounter++)
            {
                //give the first number of people an extra card
                temp = handSize;
                if (extra > 0 && wholeDeck)
                {
                    temp = handSize + 1;
                    extra--;
                }

                //when you have created all the human players make the rest computers
                if(humanCounter >= numHumanPlayers)
                {
                    human = false;
                }

                //create the player and give them their cards
                Player player = new()
                {
                    PlayerNumber = i,
                    Human = human,
                    Hand = Deck.Deal(temp),
                    HandSize = temp
                };
                Game.Players.AddLast(player);
                if(player.Human)
                {
                    OrganizeHand(player);
                }
            }
        }


        /// <summary>
        /// This method organizes a players cards by suit and value
        /// </summary>
        /// <param name="player">The player whose cards get organized</param>
        public static void OrganizeHand(Player player)
        {
            LinkedList<Cards> hand = player.Hand;
            //create lists to filter the cards into based on their suit
            List<Cards> spades = new();
            List<Cards> clubs = new();
            List<Cards> dimonds = new();
            List<Cards> hearts = new();
            //filter the cards into the corrosponding list
            foreach (Cards card in hand)
            {
                switch (Cards.GetCardSuit(card))
                {
                    case Deck.spades:
                        spades.Add(card);
                        break;
                    case Deck.clubs:
                        clubs.Add(card);
                        break;
                    case Deck.hearts:
                        hearts.Add(card);
                        break;
                    case Deck.dimonds:
                        dimonds.Add(card);
                        break;
                }
            }

            //conver the lists to arrays and sort them
            Cards[] tempSpades = Cards.SortArrayOfCards(spades.ToArray());
            Cards[] tempClubs = Cards.SortArrayOfCards(clubs.ToArray());
            Cards[] tempDimonds = Cards.SortArrayOfCards(dimonds.ToArray());
            Cards[] tempHearts = Cards.SortArrayOfCards(hearts.ToArray());
            
            //combine all the sorted arrays
            Cards[] tempHand = tempSpades.Concat(tempClubs).ToArray();
            tempHand = tempHand.Concat(tempDimonds).ToArray();
            tempHand = tempHand.Concat(tempHearts).ToArray();

            //clear the hand list and fill it with the sorted array
            hand.Clear();
            player.Hand = new LinkedList<Cards>(tempHand);
        }


        /// <summary>
        /// This method takes a player and returns that players hand
        /// </summary>
        /// <param name="player">The player whose hand will be returned</param>
        /// <returns>Returns the hand of the player</returns>
        public static LinkedList<Cards> GetPlayerHand(Player player)
        {
            return player.Hand;
        }


        /// <summary>
        /// This method takes a player and deals them a new hand
        /// </summary>
        /// <param name="player">The player who will be delt a new hand</param>
        public static void DealNewHand(Player player)
        {
            //if there are cards in the deck deal a new hand
            if(Deck.deck.Count == 0)
            {
                return;
            }
            player.Hand = Deck.Deal(player.HandSize);
        }


        /// <summary>
        /// This method takes a player and returns that players identifying number
        /// </summary>
        /// <param name="player">The player whose number will be returned</param>
        /// <returns>Returns the number of the player</returns>
        public static int GetPlayerNumber(Player player)
        {
            return player.PlayerNumber;
        }


        /// <summary>
        /// This method takes a player and returns that players bid
        /// </summary>
        /// <param name="player">The player whose bid will be returned</param>
        /// <returns>Returns the bid of the player</returns>
        public static int GetPlayerBid(Player player)
        {
            return player.Bid;
        }


        /// <summary>
        /// Sets a players bit to be an int
        /// </summary>
        /// <param name="player">The player whose bid will be set</param>
        /// <param name="bit">The new bid value</param>
        public static void SetPlayerBid(Player player, int bid)
        {
            player.Bid = bid;
        }


        /// <summary>
        /// This method takes a player and returns whether or not it is human
        /// </summary>
        /// <param name="player">The player is getting checked</param>
        /// <returns>Returns whether or not it is human</returns>
        public static bool IsPlayerHuman(Player player)
        {
            return player.Human;
        }


        /// <summary>
        /// This method takes a player and returns that players score
        /// </summary>
        /// <param name="player">The player whose score will be returned</param>
        /// <returns>Returns that players score</returns>
        public static int GetPlayerScore(Player player)
        {
            return player.PlayerScore;
        }


        /// <summary>
        /// This method takes a player and updates the players score
        /// </summary>
        /// <param name="player">The player whose score will be updates</param>
        /// <param name="points">The number of points the player has</param>
        public static void SetPlayerScore(Player player, int points)
        {
            player.PlayerScore = points;
        }


        /// <summary>
        /// This method takes a player and returns how many bags that player has
        /// </summary>
        /// <param name="player">The player whose number of bags will be returned</param>
        /// <returns>Returns how many bags that player has</returns>
        public static int GetPlayerBags(Player player)
        {
            return player.Bags;
        }


        /// <summary>
        /// This method takes a player and updates how many bags the player has
        /// </summary>
        /// <param name="player">The player whose bags will be updated</param>
        /// <param name="bags">The number of bags the player has</param>
        public static void SetPlayerBags(Player player, int bags)
        {
            player.Bags = bags;
        }


        /// <summary>
        /// This method takes a player and returns how many rounds that player has won
        /// </summary>
        /// <param name="player">The player whose number of round wins will be returned</param>
        /// <returns>Returns how many rounds that player has won</returns>
        public static int GetPlayerRoundWins(Player player)
        {
            return player.PlayerRoundsWon;
        }


        /// <summary>
        /// This method takes a player and updates how many rounds the player has won
        /// </summary>
        /// <param name="player">The player whose number of round wins will be updated</param>
        public static void SetPlayerRoundWins(Player player, int wins)
        {
            player.PlayerRoundsWon = wins;
        }


        /// <summary>
        /// This method adds a card to the end of a players hand
        /// </summary>
        /// <param name="player">The player who recieves the card</param>
        /// <param name="card">The card the player is recieving</param>
        public static void AddToHand(Player player, Cards card)
        {
            player.Hand.AddLast(card);
        }


        /// <summary>
        /// This method takes a card out of a players hand
        /// </summary>
        /// <param name="player">The player who is losing the card</param>
        /// <param name="card">The card the player is losing</param>
        public static void TakeFromHand(Player player, Cards card)
        {
            player.Hand.Remove(card);
        }
    }
}
