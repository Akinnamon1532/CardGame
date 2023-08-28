using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Author: Andrew Kinnamon
/// Date: 3/21/2023
/// Purpose/Description: This class runs the game of Spades
/// </summary>
namespace CardsProject
{
    internal class Spades
    {
        private static int winningScore = 200;


        /// <summary>
        /// This is the method that the Game class will call to begin the game of Spades
        /// </summary>
        public static void Run()
        {
            //clear the console
            Console.Clear();
            //build and shuffle the deck
            Deck.BuildDeck();
            Deck.Shuffle();

            //get the number of human and AI players from the user
            int[] playerInfo = GetPlayersSpades();
            int totalPlayers = playerInfo[0];
            int humanPlayers = playerInfo[1];

            //if there are 6 players remove the 4 2's to make the Deck divisible by 6
            if (totalPlayers == 6)
            {
                LinkedList<Cards> tempDeck = new(Deck.deck);
                foreach (Cards card in Deck.deck)
                {
                    if (Cards.GetCardValue(card) == 2)
                    {
                        tempDeck.Remove(card);
                    }
                }
                Deck.deck = tempDeck;
            }

            //create the players that we need
            Player.CreatePlayers(totalPlayers, humanPlayers);

            //Play the game
            Play(humanPlayers);
        }


        /// <summary>
        /// This method gets the number of human and computer players from the user
        /// </summary>
        /// <returns>Returns an integer array contaning the player data</returns>
        private static int[] GetPlayersSpades()
        {
            //ask the user how many human players will be playing
            Console.WriteLine("Spades can be played with 2, 4, or 6 total players");
            Console.WriteLine("How many human players?");

            //verify that their response is valid
            string? humanPlayers = Console.ReadLine();
            while (!(Game.IsInt(humanPlayers) && ValidateNumberOfHumanPlayers(humanPlayers)))
            {
                humanPlayers = Console.ReadLine();
            }
            Console.Clear();
            int existingPlayers = Convert.ToInt32(humanPlayers);

            //calculate the possible number of computer players can be in the game
            LinkedList<int> AIs = NumOfAI(existingPlayers);
            string? computerPlayers = "";
            Console.WriteLine("There are " + existingPlayers + " human players in the game\r\n");

            //put all the possible number of computer players in a string
            if (AIs.Count > 0 && existingPlayers < 6)
            {
                string AIplayersNums = "";
                int i = 1;
                foreach (int extraPlayers in AIs)
                {
                    AIplayersNums += " " + Convert.ToString(extraPlayers);
                    if (AIs.Count > i + 1)
                    {
                        AIplayersNums += ",";
                    }
                    else if (AIs.Count == i + 1)
                    {
                        AIplayersNums += ", or";
                    }
                    i++;
                }

                //tell the user what the options are for the possible number of computer players, and get their response
                Console.WriteLine("You can add" + AIplayersNums + " computer players. How many would you like to add?");
                computerPlayers = Console.ReadLine();

                //verify that their response is valid
                while (!(Game.IsInt(computerPlayers) && ValidateNumberOfComputerPlayers(computerPlayers, AIs)))
                {
                    computerPlayers = Console.ReadLine();
                }
            }
            Console.Clear();

            //put the player data in an array and return it
            int totalPlayers = Convert.ToInt32(computerPlayers) + existingPlayers;
            int[] playerInfo = { totalPlayers, existingPlayers };
            return playerInfo;
        }


        /// <summary>
        /// This method gets a string and checks if it is a valid number of human players
        /// </summary>
        /// <param name="userResponse">The string to be checked</param>
        /// <returns>Returns a bool indicating whether or not it is valid</returns>
        private static bool ValidateNumberOfHumanPlayers(String? userResponse)
        {
            bool valid = false;
            //make sure it falls in the range between 6 and 1
            if (Convert.ToInt32(userResponse) <= 6)
            {
                if (Convert.ToInt32(userResponse) >= 1)
                {
                    valid = true;
                }
                else
                {
                    //tell the user if it is too low
                    Console.WriteLine("Response too low. Please enter a higher number of human players");
                }
            }
            else
            {
                //tell the user if it is too high
                Console.WriteLine("Response too high. Please enter a lower number of human players");
            }
            return valid;
        }


        /// <summary>
        /// This method gets a string and checks if it is a valid number of computer players
        /// </summary>
        /// <param name="userResponse">The string to be checked</param>
        /// <param name="validOptions">A list of valid options</param>
        /// <returns>Returns a bool indicating whether or not it is valid</returns>
        private static bool ValidateNumberOfComputerPlayers(String? userResponse, LinkedList<int> validOptions)
        {
            bool valid = false;
            //check the response against the list of valid options
            foreach (int option in validOptions)
            {
                if (Convert.ToInt32(userResponse) == option)
                {
                    valid = true;
                    break;
                }
                else if (option == validOptions.Last())
                {
                    //tell the user if their response was invalid
                    Console.WriteLine("Your response was not one of the options. Please enter in one of the listed options");
                }
            }
            return valid;
        }


        /// <summary>
        /// This method gets a number of already existing players and returns a list of all the
        /// options of additional players that can be added
        /// </summary>
        /// <param name="existingPlayers">The number of existing players</param>
        /// <returns>Returns a list of all the options of additional players that can be added</returns>
        private static LinkedList<int> NumOfAI(int existingPlayers)
        {
            LinkedList<int> AI = new();
            //if its already an even number add zero as an option
            if (existingPlayers % 2 == 0)
            {
                AI.AddFirst(0);
            }
            //subtract it from 6, 4, and 2 and add the results that are positive to the list
            for (int i = 1; i <= 3; i++)
            {
                if (!(2 * i - existingPlayers < 1))
                {
                    AI.AddLast(2 * i - existingPlayers);
                }
            }
            return AI;
        }


        /// <summary>
        /// This method plays the game of spades
        /// </summary>
        /// <param name="humanPlayers">The number of human players</param>
        private static void Play(int humanPlayers)
        {
            //use an array because you can make it circular
            Player[] players = Game.Players.ToArray();
            ClearScreen();
            int roundStarter = 0;

            //this loop loops until a player gets the number of points required to win
            while (CheckForWinner(players) == -1)
            {
                bool spadesBroken = false;
                int startingPlayer = roundStarter;
                //shuffle and deal a new hand
                Deck.Shuffle();
                foreach (Player player in Game.Players)
                {
                    Player.DealNewHand(player);
                    if (Player.IsPlayerHuman(player))
                    {
                        Player.OrganizeHand(player);
                    }
                }
                //get the players bids for the hand
                GetBids(humanPlayers);
                //this loop loops though each hand of the game
                while (Player.GetPlayerHand(players[0]).Count > 0)
                {
                    ClearScreen();
                    bool lead = true;
                    //keep track of what the first card played was as well as who is leading each hand
                    Cards? leadCard = null;
                    Player? winningPlayer = null;
                    Cards? winningCard = null;
                    int firstPlayer = startingPlayer;
                    //this loop loops through each round of the hand
                    for (int i = 0; i < players.Length; i++)
                    {
                        Cards playedCard;
                        Player player = players[startingPlayer];

                        //check if the player is human or not and make the player choose their next card
                        if (Player.IsPlayerHuman(player))
                        {
                            Console.Write("Player" + Player.GetPlayerNumber(player) + " ");
                            playedCard = ChooseCard(player, leadCard, lead, spadesBroken);
                        }
                        else
                        {
                            playedCard = AIChooseCard(player, leadCard, lead, spadesBroken);
                        }

                        //if it is the first card played update the variables accordingly
                        if (lead == true)
                        {
                            winningPlayer = player;
                            winningCard = playedCard;
                            leadCard = playedCard;
                        }

#pragma warning disable CS8604 //Suppress CS8604 warning
                        //check if the card that was played takes the lead
                        if (NewLeader(winningCard, playedCard))
                        {
                            winningPlayer = player;
                            winningCard = playedCard;
                        }

                        //check if spades have been broken
                        if (Cards.GetCardSuit(playedCard).Equals(Deck.spades))
                        {
                            spadesBroken = true;
                        }

                        //add card to the tabel and put it back into the deck
                        Game.Table.AddLast(playedCard);
                        Deck.deck.AddLast(playedCard);
                        lead = false;
                        ClearScreen(firstPlayer);
                        startingPlayer = NextPlayer(startingPlayer);
                    }
                    //update player scores
                    Player.SetPlayerRoundWins(winningPlayer, Player.GetPlayerRoundWins(winningPlayer) + 1);
                    //set the person who won that round as the player who will start the next one
                    ClearScreen(firstPlayer);
                    startingPlayer = Player.GetPlayerNumber(winningPlayer) - 1;
                    Console.ReadKey(true);
                    Game.Table.Clear();
#pragma warning restore CS8604
                }
                roundStarter = NextPlayer(roundStarter);
                //award points and clear player data
                foreach (Player player in Game.Players)
                {
                    AwardPoints(player);
                    Player.SetPlayerBid(player, 0);
                    Player.SetPlayerRoundWins(player, 0);
                }
                ClearScreen();
                Console.ReadKey(true);
            }
            Player victor = players[CheckForWinner(players)];
            Console.WriteLine("The winner is Player " + Player.GetPlayerNumber(victor) + "!");
            Console.ReadKey(true);
        }


        /// <summary>
        /// This method gets bids from all the players
        /// </summary>
        /// <param name="humanPlayers">The number of human players</param>
        private static void GetBids(int humanPlayers)
        {
            //calculate the higest bid someone can make
            int maxBids = 52 / Game.Players.Count;
            Random random = new();
            int i = 1;
            //loop through all the players
            foreach (Player player in Game.Players)
            {
                if (humanPlayers >= i)
                {
                    //loop through each card for the player and add their value and suit to the string
                    string display = "Player" + i + " Your cards are:\r\n";
                    foreach (Cards card in Player.GetPlayerHand(player))
                    {
                        display = string.Concat(display, Cards.CardToString(card), "\r\n");
                    }
                    //print the string
                    Console.Write(display);

                    //ask the player to bid
                    Console.WriteLine("\r\nWhat is your bid?");
                    String? bid = Console.ReadLine();

                    //verify their resonse is valid
                    while (!(Game.IsInt(bid) && Convert.ToInt32(bid) <= 26 && Convert.ToInt32(bid) >= 0))
                    {
                        Console.WriteLine("bid must be between " + maxBids + " and zero");
                        bid = Console.ReadLine();
                    }
                    //update the players bid
                    Player.SetPlayerBid(player, Convert.ToInt32(bid));
                    ClearScreen();
                }
                else
                {
                    //have the computer choose a random bid
                    int bid = random.Next(0, 8);
                    Player.SetPlayerBid(player, bid);
                    //show the user what the computer player bid
                    Console.WriteLine("Player" + i + " bid: " + bid + "\r\nPress any key to continue");
                    Console.ReadKey(true);
                    ClearScreen();
                }
                i++;
            }
        }


        /// <summary>
        /// This method has the human player select a card that they wish to play
        /// </summary>
        /// <param name="player">The human player</param>
        /// <param name="leadCard">The first card to get played in the round</param>
        /// <param name="lead">Whether or not this is the first card of the round</param>
        /// <param name="spadesBroken">Whether or not spades has been broken</param>
        /// <returns>Returns the card that was selected by the player</returns>
        private static Cards ChooseCard(Player player, Cards? leadCard, bool lead, bool spadesBroken)
        {
            LinkedList<Cards> hand = Player.GetPlayerHand(player);
            //display the players cards and ask the player to choose which one they'd like to play
            string display = "Which card would you like to play?\r\n";
            string cardNumberPresentor = ".)  ";
            Cards[] tempHand = hand.ToArray();
            int i;
            for (i = 0; i < tempHand.Length; i++)
            {
                if (i >= 9)
                {
                    cardNumberPresentor = ".) ";
                }
                display = string.Concat(display, i + 1, cardNumberPresentor, Cards.CardToString(tempHand[i]), " \r\n");
            }
            Console.WriteLine(display);

            //get the user's choice and make sure it is an integer in the right range
            Cards chosenCard = tempHand[ValidateCardChoiceInput(i)];

            //make sure the card the player chose can be played on this turn
            if (lead == true)
            {
                while (!LeadCardChoiceCheck(player, chosenCard, spadesBroken))
                {
                    chosenCard = tempHand[ValidateCardChoiceInput(i)];
                }
            }
            else if (leadCard != null)
            {
                while (!CardChoiceCheck(player, chosenCard, leadCard))
                {
                    chosenCard = tempHand[ValidateCardChoiceInput(i)];
                }
            }
            //remove the card from the players hand and return it
            Player.TakeFromHand(player, chosenCard);
            return chosenCard;
        }


        /// <summary>
        /// This method has the computer player select a card that they wish to play
        /// </summary>
        /// <param name="player">The computer player</param>
        /// <param name="leadCard">The first card to get played in the round</param>
        /// <param name="lead">Whether or not this is the first card of the round</param>
        /// <param name="spadesBroken">Whether or not spades has been broken</param>
        /// <returns>Returns the card that was selected by the player</returns>
        private static Cards AIChooseCard(Player player, Cards? leadCard, bool lead, bool spadesBroken)
        {
            Cards? chosenCard = null;
            LinkedList<Cards> hand = Player.GetPlayerHand(player);
            //go through each card in the players hand until you find one that can be played
            foreach (Cards card in hand)
            {
                if (lead == true && LeadCardChoiceCheck(player, card, spadesBroken))
                {
                    chosenCard = card;
                    break;
                }
                else if (leadCard != null && CardChoiceCheck(player, card, leadCard))
                {
                    chosenCard = card;
                    break;
                }
            }
            //throw exception if chosenCard is still null, this should never get thrown
            if (chosenCard == null)
            {
                throw new Exception("chosenCard is null");
            }
            //remove the card from the players hand and return it
            Player.TakeFromHand(player, chosenCard);
            return chosenCard;
        }


        /// <summary>
        /// This method checks if the played card can be played according to the rules of spades if it isn't lead
        /// </summary>
        /// <param name="player">The player that is wanting to play the card</param>
        /// <param name="playedCard">The card that the player wants to play</param>
        /// <param name="leadCard">The first card to get played in the round</param>
        /// <returns>Returns whether or not that card can be played</returns>
        private static bool CardChoiceCheck(Player player, Cards playedCard, Cards leadCard)
        {
            bool valid = true;
            string leadSuit = Cards.GetCardSuit(leadCard);
            string playedSuit = Cards.GetCardSuit(playedCard);
            if (!playedSuit.Equals(leadSuit))
            {
                //if this cards suit doesn't match the lead card's suit, loop though the players hand
                LinkedList<Cards> hand = Player.GetPlayerHand(player);
                foreach (Cards card in hand)
                {
                    //If the player has any cards that do match the lead card's suit it's invalid
                    string suit = Cards.GetCardSuit(card);
                    if (suit.Equals(leadSuit))
                    {
                        valid = false;
                        Console.WriteLine("That card cannot be played right now, choose again");
                        break;
                    }
                }
            }
            return valid;
        }


        /// <summary>
        /// This method checks if the played card can be played according to the rules of spades if it is lead
        /// </summary>
        /// <param name="playedCard">The card that the player wants to play</param>
        /// <param name="spadesBroken">Whether or not spades have been broken</param>
        /// <returns>Returns whether or not that card can be played</returns>
        private static bool LeadCardChoiceCheck(Player player, Cards playedCard, bool spadesBroken)
        {
            bool hasSpades = false;
            bool valid = true;
            string playedSuit = Cards.GetCardSuit(playedCard);

            LinkedList<Cards> hand = Player.GetPlayerHand(player);
            foreach (Cards card in hand)
            {
                //if the player has only spades playing a spade is valid
                string suit = Cards.GetCardSuit(card);
                if (suit.Equals(Deck.spades))
                {
                    hasSpades = true;
                    break;
                }
            }

            //if they are playing a spade and spades have not been broken it is invalid
            if (playedSuit.Equals(Deck.spades) && !spadesBroken && hasSpades)
            {
                valid = false;
                Console.WriteLine("That card cannot be played since spades have not yet been broken, choose again");
            }
            return valid;
        }


        /// <summary>
        /// Clear the screen and display the player data
        /// </summary>
        private static void ClearScreen(int playerNumber = -1)
        {
            Console.Clear();
            //put the players scores, bids, and rounds won in a string
            string currentScore = "Scores:\t\t";
            string bags = "Players Bags:\t";
            string bids = "Players Bids:\t";
            string roundsWon = "Rounds won:\t";
            foreach (Player player in Game.Players)
            {
                int PlayerNumber = Player.GetPlayerNumber(player);
                int PlayerScore = Player.GetPlayerScore(player);
                int PlayerBags = Player.GetPlayerBags(player);
                int PlayerBid = Player.GetPlayerBid(player);
                int PlayerWins = Player.GetPlayerRoundWins(player);

                currentScore = string.Concat(currentScore, "Player", PlayerNumber, ": ", PlayerScore);
                bags = string.Concat(bags, "Player", PlayerNumber, ": ", PlayerBags);
                bids = string.Concat(bids, "Player", PlayerNumber, ": ", PlayerBid);
                roundsWon = string.Concat(roundsWon, "Player", PlayerNumber, ": ", PlayerWins);

                if (player != Game.Players.Last())
                {
                    currentScore = string.Concat(currentScore, ", ");
                    bags = string.Concat(bags, ", ");
                    bids = string.Concat(bids, ", ");
                    roundsWon = string.Concat(roundsWon, ", ");
                }
            }
            //display the players data
            Console.WriteLine(currentScore + "\r\n\r\n" + bags + "\r\n\r\n" + bids + "\r\n\r\n" + roundsWon + "\r\n");

            if (playerNumber > -1)
            {
                //put the cards that have been played in a string
                string cardsPlayed = "Cards in play:\r\n";
                int i = 1;
                foreach (Cards card in Game.Table)
                {
                    cardsPlayed = string.Concat(cardsPlayed, "\t\tPlayer", playerNumber + 1, ": ", Cards.CardToString(card));
                    if (i < Game.Players.Count)
                    {
                        cardsPlayed = string.Concat(cardsPlayed, "\r\n");
                    }
                    playerNumber = NextPlayer(playerNumber);
                    i++;
                }
                //display the cards
                Console.WriteLine(cardsPlayed + "\r\n");
            }
        }


        /// <summary>
        /// This method checks to see if any players have gotten enough points to win
        /// </summary>
        /// <param name="players">An array containing all the players</param>
        /// <returns>Returns either the index of the winner or -1</returns>
        private static int CheckForWinner(Player[] players)
        {
            int winner = -1;
            //check each players points
            for (int i = 0; i < players.Length; i++)
            {
                //if a player has enough to win, label them the winner
                Player player = players[i];
                if (Player.GetPlayerScore(player) >= winningScore)
                {
                    winner = i;
                }
            }
            return winner;
        }


        /// <summary>
        /// This method checks to see if players input is in the valid range
        /// </summary>
        /// <param name="highestValue">the highest value that the user input can be</param>
        /// <returns>Returns an int of valid user input</returns>
        private static int ValidateCardChoiceInput(int highestValue)
        {
            //get the user input
            String? choice = Console.ReadLine();
            //loop until they give input that is an int and in the correct range
            while (!(Game.IsInt(choice) && Convert.ToInt32(choice) > 0 && Convert.ToInt32(choice) <= highestValue))
            {
                Console.WriteLine("Please choose one of the listed options");
                choice = Console.ReadLine();
            }
            int confirmedChoice = Convert.ToInt32(choice) - 1;
            return confirmedChoice;
        }


        /// <summary>
        /// This method checks if a card takes the lead over the current leading card
        /// </summary>
        /// <param name="currentLeader">The card that is currently in the lead</param>
        /// <param name="challenger">The card that might take the lead</param>
        /// <returns>Returns whether or not the challenger takes the lead</returns>
        private static bool NewLeader(Cards currentLeader, Cards challenger)
        {
            bool newLeader = false;
            //check if they have the same suit
            if (Cards.GetCardSuit(currentLeader).Equals(Cards.GetCardSuit(challenger)))
            {
                //if the challenger has a higher value it is the new leader
                if (Cards.GetCardValue(currentLeader) < Cards.GetCardValue(challenger) && Cards.GetCardValue(currentLeader) != 1)
                {
                    newLeader = true;
                }
                else if (Cards.GetCardValue(challenger) == 1)
                {
                    newLeader = true;
                }
            }
            // if they are different suits, but the challenger is a spade, it is the new leader
            else if (Cards.GetCardSuit(challenger).Equals(Deck.spades) && !Cards.GetCardSuit(currentLeader).Equals(Deck.spades))
            {
                newLeader = true;
            }
            return newLeader;
        }


        /// <summary>
        /// This moves the index to the next player, making the array circular
        /// </summary>
        /// <param name="player">the index of the current player</param>
        /// <returns>Returns the index of the next player</returns>
        private static int NextPlayer(int player)
        {
            player++;
            //if the incremented index is outside the array, set it to 0
            if (player + 1 > Game.Players.Count)
            {
                player = 0;
            }
            return player;
        }


        /// <summary>
        /// This awards points to the player according to the player data
        /// </summary>
        /// <param name="player">the player who is getting awarded points</param>
        private static void AwardPoints(Player player)
        {
            int pointChange = 0;
            int bids = Player.GetPlayerBid(player);
            int roundsWon = Player.GetPlayerRoundWins(player);
            //award 10 points per bid completed
            if (roundsWon >= bids && bids > 0)
            {
                pointChange += bids * 10;
                pointChange += roundsWon - bids;
                Player.SetPlayerBags(player, Player.GetPlayerBags(player) + roundsWon - bids);
            }
            //remove 10 points per bid if the player didn't hit their bid
            else
            {
                pointChange -= bids * 10;
            }
            //take away 100 points for every 10 bags
            if (Player.GetPlayerBags(player) >= 10)
            {
                pointChange -= 100;
                Player.SetPlayerBags(player, Player.GetPlayerBags(player) - 10);
            }
            //if a player goes nill they either win or loose 100 points
            if (bids == 0)
            {
                if (roundsWon == 0)
                {
                    pointChange += 100;
                }
                else
                {
                    pointChange -= 100;
                }
            }

            Player.SetPlayerScore(player, Player.GetPlayerScore(player) + pointChange);
        }
    }
}

//Rules Source: https://bicyclecards.com/how-to-play/spades