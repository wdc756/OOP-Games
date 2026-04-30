using System;
using System.Collections.Generic;
using System.Linq;


//DO NOT KILL (REMOVE)
//Programming Duck - Muffin
//⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣤⡶⠿⠿⠷⣶⣄⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣰⡿⠁ ⠀⢀⣀⡀⠙⣷⡀⠀⠀⠀
//⠀⠀⠀⡀⠀⠀⠀⠀⠀⢠⣿⠁⠀⠀⠀⠘⠿⠃⠀ ⢸⣿⣿⣿⣿⡇
//⠀⣠⡿⠛⢷⣦⡀ ⠀⠈⣿⡄⠀⠀⠀⠀⠀ ⠀⠀⣸⣿⣿⣿⠟
//⢰⡿⠁⠀⠀⠙⢿⣦⣤⣤⣼⣿⣄⠀⠀⠀⠀⠀⢴⡟⠛⠋⠁⠀
//⣿⠇⠀⠀⠀⠀⠀⠉⠉⠉⠉⠉⠁⠀⠀⠀⠀⠈⣿⡀⠀⠀⠀
//⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀ ⠀⢹⡇⠀⠀⠀
//⣿⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀ ⣼⡇⠀⠀⠀
//⠸⣷⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀ ⢠⡿⠀⠀⠀⠀
// ⠹⣷⣤⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀ ⠀⣀⣰⡿⠁⠀⠀⠀⠀
//⠀⠀⠀⠉⠙⠛⠿⠶⣶⣶⣶⣶⣶⠶⠿⠟⠛⠉⠀⠀⠀⠀⠀⠀

// class Program
// {
//     static void Main(string[] args)
//     {
//        // Start the game via the BlackJack class logic
//        BlackJack game = new BlackJack();
//        game.startGame();
//     }
// }

namespace Games.Games
{
    public class BlackJack
    {
        //pay attention to understand
        //I've learned a lot of things lol



        ///--\--/--\--/--\--/--\--/--\--/--\--/--\--/--\--/--\--/--\--/--\--/--\--/--\--/--\--/--\--/--\--/--\--/--\--/--\--/--\--/--\--/--\
        //|
        //|                                                          Rules
        //|
        //##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##
        //#  - The player has 1,000 chips, they need to get to 2,000, aka double
        //#
        //#  - The player can choose the amount of chips to deal, likely with three inputs: one to decrease, one to increase, one to submit
        //#  - Might only make the player be able to deal in increments of 50, 100, and 500.
        //#
        //#  - Once the game starts, the ai's card is hidden
        //#  - The player can see both their cards   
        //#  - The player can hit multiple times, but if they go over 21, they bust
        //#  - Once the player stands (if they haven't lost) the ai will reveal it's cards
        //#  - The ai will hit until it gets higher than the player (it wins or busts)
        //#  - IF the ai's cards are already high (17+), the ai loses if it's lower than the player (loses)
        //#
        //#  - If the player wins, they get double the amount they betted
        //#  - If the player loses, they lose that amount the betted
        //#
        //#  - If they player is bankrupt, start the game again
        //##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##=##

        //"UI" imagery

        //--Attributes--

        //Intialize deck of cards, shuffle (randomize), have it preloaded

        // List<Card> cards =
        // {
        //     new Card()
        // };
        // List<Card> usedCards =
        // {
        //     new Card()
        // };
        
        List<Card> cards = new List<Card>();
        List<Card> usedCards = new List<Card>();
        int chips = 1000;
        int bettingAmount;

        //handsys
        List<Card> playerCards = new List<Card>();
        List<Card> houseCards = new List<Card>();

        //create deck
        public BlackJack()
        {
            InitializeDeck();
        }

        //INITIALIZE dweck 
        private void InitializeDeck()
        {
            string[] suits = { "Spades", "Hearts", "Clubs", "Diamonds" };
            string[] symbols = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            int[] values = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11 };

            cards.Clear();// clears

            //just intilizaing all the cards you can
            for (int s = 0; s < suits.Length; s++)
            {
                for (int i = 0; i < symbols.Length; i++)
                {
                    cards.Add(new Card(symbols[i], suits[s], values[i]));
                }
            }
            Shuffle();//randomize
        }

        private void Shuffle()
        {
            Random rng = new Random();//random variable
            cards = cards.OrderBy(x => rng.Next()).ToList();
            //x (each item) is randomized
        }

        public void startGame()
        {
            //"UI" -- Prompting
            Console.WriteLine("----Black Jack----");
            Console.WriteLine("----Goal: 2,000---");
            Console.WriteLine($"Current Chips: {chips}");
            Console.WriteLine("-----------");
            Console.WriteLine("Enter your betting amount:");

            //input
            string input = Console.ReadLine();
            
            //check that the input is a valid number and isn't nothing or greater than what they have
            while (!int.TryParse(input, out bettingAmount) || bettingAmount < 1 || bettingAmount > chips)
            {
                if (bettingAmount < 1)
                    Console.WriteLine("You must bet a number higher than 0.");
                else if (bettingAmount > chips)
                    Console.WriteLine("You cannot bet more chips than you have.");
                else
                    Console.WriteLine("You must bet a valid number");
                
                input = Console.ReadLine();
            }   

            //starts the game loop, also clears the screen
            Console.Clear();
            DealInitialCards();//basically gets each hand ready
            gameLoop();
        }

        private void DealInitialCards()
        {
            //clear both hands
            playerCards.Clear();
            houseCards.Clear();
            
            //give two cards to each hand
            playerCards.Add(DrawCard());
            houseCards.Add(DrawCard());
            playerCards.Add(DrawCard());
            houseCards.Add(DrawCard());
        }

        private Card DrawCard()
        {
            if (cards.Count == 0) //if cards are empty
            {
                cards.AddRange(usedCards);//give all the cards
                usedCards.Clear();//remove because usedCards is therotically empty
                Shuffle();//shuffle aka randomize
            }
            //grab a card, remove it from cards and return it
            Card c = cards[0];
            cards.RemoveAt(0);
            return c;
        }

        private int CalculateScore(List<Card> hand)
        {
            int total = hand.Sum(c => c.number);//use every card's number
            int aces = hand.Count(c => c.cardSymbol == "A");//only if it's an ace

            while (total > 21 && aces > 0)// aces are considered a 1 if they would bring the player over
            {
                total -= 10;
                aces--;
            }
            return total;
        }


        //GAME LOOP
        public void gameLoop()
        {
            //amount player cards add to
            int playerAmount = CalculateScore(playerCards);
            //amount house cards add to
            int houseAmount = CalculateScore(houseCards);

            bool playerStanding = false;


            //the loop until busted or player stands
            while (playerAmount < 21 && !playerStanding)
            {
                Console.Clear();//cleans line, makes it look updated

                //"UI"

                Console.WriteLine($"Chips: {chips} | Bet: {bettingAmount}");
                Console.WriteLine($"Cards in Deck Left: {cards.Count}");

                //house handing showing: 
                Console.WriteLine("\n--- HOUSE HAND ---");
                // Show only first card, hide the rest for now
                Card.DisplayHand(new List<Card> { houseCards[0] }, true); //make it hidden
                Console.WriteLine("-----------");

                //player hand showing
                Console.WriteLine("\n--- YOUR HAND ---");
                Card.DisplayHand(playerCards);
                Console.WriteLine($"Total: {playerAmount}");
                Console.WriteLine("-----------");

                //Enter 1 to hit and 2 to stand
                //loop to make sure the player only enters a number
                Console.WriteLine("1 for HIT : 2 to STAND");
                string input = Console.ReadLine();
                int choice;

                if (int.TryParse(input, out choice) && (choice == 1 || choice == 2))
                {
                    if (choice == 1)
                    {
                        //add card from cards to playerCard
                        playerCards.Add(DrawCard());
                        //recalculate playerAmount
                        playerAmount = CalculateScore(playerCards);
                    }
                    else
                    {
                        playerStanding = true;
                    }
                }
                else
                {
                    Console.WriteLine("Must enter 1 for HIT and 2 to STAND");
                    Console.ReadKey();//no annoying specific button
                }
            }

            // Logic after player finishes hitting/standing
            Console.Clear();
            Console.WriteLine("--- REVEALING HOUSE ---");
            Card.DisplayHand(houseCards);
            houseAmount = CalculateScore(houseCards);

            if (playerAmount > 21) // the player busted
            {
                Console.WriteLine($"Bust! You had {playerAmount}.");
                Console.WriteLine("You lost! LOL");
                chips -= bettingAmount;
                Console.WriteLine("-- " + bettingAmount);
            }
            else
            {
                //reveal house's cards
                //if houseAmount < less than player, hit (looop) until they get the same or higher than player or bust
                while (houseAmount < 17 || (houseAmount < playerAmount && houseAmount <= 21))
                {
                    Console.WriteLine("House hits...");
                    houseCards.Add(DrawCard());
                    houseAmount = CalculateScore(houseCards);
                    Card.DisplayHand(houseCards);
                    System.Threading.Thread.Sleep(800); //a little pause for polish
                }

                if (houseAmount > 21)
                {
                    //if bust, give player double
                    Console.WriteLine("House Busts! You win!");
                    chips += bettingAmount;
                }
                else if (houseAmount > playerAmount)
                {
                    //if higher, take away 
                    Console.WriteLine($"House wins with {houseAmount} vs {playerAmount}");
                    chips -= bettingAmount;
                }
                else if (houseAmount == playerAmount)
                {
                    //if same, give bettingAmount back
                    //if houseAmount == playerAmount,
                    //give player betting amount back
                    Console.WriteLine("Push! (Tie)");
                }
                else
                {
                    Console.WriteLine("You win!");
                    chips += bettingAmount;
                }
            }

            //add house's cards and player's cards to usedCards list
            usedCards.AddRange(playerCards);
            usedCards.AddRange(houseCards);

            Console.WriteLine($"Current Chips: {chips}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            //if the amount of cards left in cards is left than half, add all usedCards to Cards, reshuffle

            if (chips >= 2000 || chips <= 0)
            {
                EndGame();
            }
            else
            {
                Console.Clear();
                startGame(); // Loop back to start to place a new bet
            }
        }

        public void EndGame()
        {
            if (chips >= 2000)
            {
                Console.WriteLine("You win. Move to the next game.");
                //move to next game
            }
            else if (chips <= 0)
            {
                Console.WriteLine("You went bankrupt!");
                chips = 1000; // Reset for the "start again" rule
                startGame();
            }
        }
    }

    public class Card
    {
        public string cardSymbol;
        public string suit;
        public int number;

        public Card(string c, string s, int n)
        {
            cardSymbol = c; // can only be a number 2-9, faces (10), and aces (1 OR 11)
            suit = s;
            number = n;
        }

        //  helper method to display cards side-by-side (becuz vertical is ugly)
        // you guys don't understand how annoying the formatting was
        //default value (optional so I don't have to change both values + makes hidden part of code method call stand out)
        
        public static void DisplayHand(List<Card> hand, bool hideSecond = false) 
        {
            string[][] cardLines = new string[hand.Count + (hideSecond ? 1 : 0)][];
            
            for (int i = 0; i < hand.Count; i++) // for the normal cards
            {
                cardLines[i] = hand[i].GetVisualLines();
            }
            
            if (hideSecond) // only add the hidden one there needs to be
            {
                cardLines[1] = GetHiddenCardLines();
            }

            //actually getting it formatted
            //cardIdx means which cards it is and what part
            //splitting it up and stacking it
            for (int row = 0; row < 6; row++)
            {
                for (int cardIdx = 0; cardIdx < cardLines.Length; cardIdx++)
                {
                    Console.Write(cardLines[cardIdx][row] + "  ");
                }
                Console.WriteLine();
            }
        }


        //flexible card 
        public string[] GetVisualLines()
        {
            string s = " ";
            if (suit == "Spades") s = " ♠"; //   /^\\
            if (suit == "Hearts") s = " ♥"; // (v)
            if (suit == "Clubs") s = " ♣"; // (&)
            if (suit == "Diamonds") s = " ♦"; //<>

            return new string[]
            {
                " _______ ",
                $"|{cardSymbol,-2}     |",
                "|       |",
                $"|  {s}   |",
                "|       |",
                $"|_____{cardSymbol,2}|"
            };
        }


        //to save code space and easier to putrint
        private static string[] GetHiddenCardLines()
        {
            return new string[]
            {
                " _______ ",
                "|?      |",
                "|  ###  |",
                "|  ###  |",
                "|      ?|",
                "|_______|"
            };
        }

    }
}

//CODE GRAVEYARD -- RIP




    // public void Display()
    // {

    //     switch (suit)
    //     {
    //         case "Spades":
    //             Console.WriteLine("  _______ ");
    //             Console.WriteLine($" |{cardSymbol,-2} .   |");
    //             Console.WriteLine(" |  /.\\  |");
    //             Console.WriteLine(" | (_._) |");
    //             Console.WriteLine(" |   |   |");
    //             Console.WriteLine($" |_____{cardSymbol,2}|");
    //             break;
    //         case "Hearts":
    //             break;
    //         case "Clubs":
    //             break;
    //         case "Diamonds":
    //             break;
    //         case null:
    //             Console.WriteLine("Missing Suit");
    //             break;
    //     }
    // }

    //           _____
    //          |A .  | _____
    //          | /.\ ||A ^  | _____
    //          |(_._)|| / \ ||A _  | _____
    //          |  |  || \ / || ( ) ||A_ _ |
    //          |____V||  .  ||(_'_)||( v )|
    //                 |____V||  |  || \ / |
    //                        |____V||  .  |
    //                               |____V|


