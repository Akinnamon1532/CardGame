using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Author: Andrew Kinnamon
/// Date: 3/21/2023
/// Purpose/Description: This class runs the game of war
/// </summary>
namespace CardsProject
{
    internal class War
    {
        /// <summary>
        /// This is the method that the Game class will call to begin the game of war
        /// </summary>
        public static void Run()
        {
            //clear the console
            Console.Clear();
            //build and shuffle the deck
            Deck.BuildDeck();
            Deck.Shuffle();

            //Since war is a 2 player game we create 2 players, 1 being human
            Player.CreatePlayers(2, 1);

            //Play the game
            Play();
        }


        /// <summary>
        /// This is the method that will play the game of war
        /// </summary>
        private static void Play()
        {
            Player Human = Game.Players.First();
            Player Computer = Game.Players.Last();

            LinkedList<Cards> HumanHand = Player.GetPlayerHand(Game.Players.First());
            LinkedList<Cards> ComputerHand = Player.GetPlayerHand(Game.Players.Last());

            //this while loop that will end when one player is out of cards i.e. the game ends
            while (HumanHand.Count != 0 && ComputerHand.Count != 0)
            {
                //tell the player how to play the next round, display the current score, and wait for them to continue
                Console.WriteLine("Press any key to play your next card, or press escape to return to the main menu\t");
                Console.WriteLine("You - " + HumanHand.Count + "\tComputer - " + ComputerHand.Count);
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    return;
                }

                //display each players next card
                Console.Write("Your Card -\t\t");
                Cards humanCard = PlayNextHand(Human, HumanHand);
                int humanCardValue = Cards.GetCardValue(humanCard);
                //Adjust ace to be the highest value card
                if(humanCardValue == 1)
                {
                    humanCardValue = 14;
                }

                Console.Write("The computers Card -\t");
                Cards computerCard = PlayNextHand(Computer, ComputerHand);
                int computerCardValue = Cards.GetCardValue(computerCard);
                //Adjust ace to be the highest value card
                if (computerCardValue == 1)
                {
                    computerCardValue = 14;
                }

                //give the cards to the winner and tell the player who won that round and wait for them to continue
                if (humanCardValue > computerCardValue)
                {
                    //order is important, add the opponets card to the bottom of the deck first then the winner of the hand
                    //if put into the table in the wrong order it will cause the game to never end
                    //Put the cards in the table Linked List
                    Game.Table.AddLast(computerCard);
                    Game.Table.AddLast(humanCard);
                    //give the cards on the table to the winner
                    AddToBottomOfHand(Human);
                    Console.WriteLine("\r\nYou won that round!");
                }
                else if (computerCardValue > humanCardValue)
                {
                    //Put the cards in the table Linked List
                    Game.Table.AddLast(humanCard);
                    Game.Table.AddLast(computerCard);
                    //give the cards on the table to the winner
                    AddToBottomOfHand(Computer);
                    Console.WriteLine("\r\nThe computer won that one");
                }
                //if it is a tie, don't award them to anyone
                else
                {
                    //Put the cards in the table Linked List
                    Game.Table.AddLast(computerCard);
                    Game.Table.AddLast(humanCard);
                    Console.WriteLine("\r\nIt's a tie! Play the next round to see who gets the cards");
                }
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    return;
                }
                //clear the console after every round to keep it clean
                Console.Clear();
            }


            //once the game is over tell the player if they won or lost
            if (HumanHand.Count != 0)
            {
                Console.WriteLine("Congratulations! You won the game of war! Press any key to return to the main menu");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine("You have been defeated! Better luck next time! Press any key to return to the main menu");
                Console.ReadKey(true);
            }
        }


        /// <summary>
        /// This method grabs the first card in the list, prints it, takes it out of the players hand, and returns it
        /// </summary>
        /// <param name="player">The player who holds the card</param>
        /// <param name="hand">The list of cards</param>
        /// <returns>Returns the first card in the list</returns>
        private static Cards PlayNextHand(Player player, LinkedList<Cards> hand)
        {
            //get the players top card
            Cards card = hand.First();
            Console.WriteLine(Cards.CardToString(card));
            //remove the card from the players hand
            Player.TakeFromHand(player, card);
            return card;
        }


        /// <summary>
        /// This method adds all of the cards in the table to the players hand
        /// </summary>
        /// <param name="player">The player who is recieving the cards</param>
        private static void AddToBottomOfHand(Player player)
        {
            //loop through the table linked list
            while (Game.Table.Count != 0)
            {
                //give the cards to the player
                Cards card = Game.Table.First();
                Game.Table.Remove(card);
                Player.AddToHand(player, card);
            }
        }
    }
}
