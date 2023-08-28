using Microsoft.Win32.SafeHandles;
using System;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// Author: Daniel Emerson, Andrew Kinnamon
/// Date 3/3/2023
/// Purpose: to be a menu interface, where the player can choose the game they want to play
/// </summary>
namespace CardsProject
{
    internal class Game
    {
        /* This is a public static readonly linked list that holds all the player objects */
        public static readonly LinkedList<Player> Players = new();
        /* This is a public static readonly linked list that holds all the Card objects that have been played */
        public static readonly LinkedList<Cards> Table = new();

        /// <summary>
        /// Runs the program
        /// </summary>
        static void Main()
        {
            bool showMenu = true;
            while (showMenu)
            {
                MainMenu();
                //clear the player and deck linked lists so that this game wont spill into the next
                Game.Players.Clear();
                Deck.deck.Clear();
            }
        }

        /// <summary>
        /// Runs the program
        /// </summary>
        /// <returns>Returns a bool indicating whether to run again or not</returns>
        public static void MainMenu() //Making the menu interface
        {
            //Display a users options
            Console.Clear();
            Console.WriteLine("Select a game: ");
            Console.WriteLine("1) War");
            Console.WriteLine("2) Spades");
            Console.WriteLine("3) Exit");
            //Since this is just for development I am commenting out the option for dummy
            //Console.WriteLine("5) Dummy");
            Console.Write("\r\nChoose a Game: ");
            string? choice = Console.ReadLine();

            //Check to see if the user input was one of the options
            //*Comment this out if you need to use the dummy game
            while(!(IsInt(choice) && Convert.ToInt32(choice) > 0 && Convert.ToInt32(choice) < 4))
            {
                Console.WriteLine("Please choose one of the options above");
                choice = Console.ReadLine();
            }

            //Run the game the player chooses
            switch (choice)
            {
                case "1":
                    War.Run();
                    break;
                case "2":
                    Spades.Run();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
            }
        }


        /// <summary>
        /// This method gets a string and checks if it can be turned into an int
        /// </summary>
        /// <param name="inputString">The string to be checked</param>
        /// <returns>Returns a bool indicating whether or not it can be an int</returns>
        public static bool IsInt(String? inputString)
        {
            bool valid = false;
            //check if the inputString can be parsed as an int
            if (int.TryParse(inputString, out _))
            {
                valid = true;
            }
            else
            {
                Console.WriteLine("Response invalid. Please enter a number");
            }
            return valid;
        }
    }
}