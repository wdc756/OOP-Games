using System;
using System.Collections.Generic;
using System.Linq;
// to let the user see they won for a couple seconds
using System.Threading;

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



namespace Games.Games
{
    // Tracks which phase of BlackJack we're currently in.
    // Each phase maps to different Input/Update/Render behaviour.
    public enum BlackJackPhase
    {
        Betting,        // Player is selecting their bet amount
        PlayerTurn,     // Player is choosing to Hit or Stand
        HouseReveal,    // House is revealing cards + auto-hitting
        RoundOver,      // Win/loss/tie result is shown, waiting for keypress
        GameOver        // Player hit 2000 or went bankrupt
    }

    public class BlackJack : Game
    {
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
        
        

        // --- Deck ---
        private List<Card> _cards     = new List<Card>();
        private List<Card> _usedCards = new List<Card>();

        // --- Hands ---
        private List<Card> _playerCards = new List<Card>();
        private List<Card> _houseCards  = new List<Card>();

        // --- Economy ---
        private int _chips;
        private int _bet;

        // Bet increment steps available with Up/Down arrow
        private readonly int[] _betIncrements = { 50, 100, 500 };
        private int _betIncrementIndex; // which increment is selected (cycles through _betIncrements)

        // --- Game state ---
        private BlackJackPhase _phase;
        private string _resultMessage; // shown on the RoundOver screen
        private string _chipDeltaMessage; // e.g. "-- 200" or "++ 200"

        // Controls how long the house-reveal animation pauses between hits (ms).
        // Update() is called on a timer, so each Update() during HouseReveal
        // processes one house action and returns true to keep going.
        private bool _houseActionPending;



        // -----------------------------------------------------------------------
        //  Game overrides
        // -----------------------------------------------------------------------

        public BlackJack()
        {
            // Slower tick because not a realtime game (0.25s)
            _updateTime = 250;
        }

        public override void Reset()
        {
            ScreenManager.ClearScreen();
            _chips = 1000;
            _bet = 50;
            _betIncrementIndex = 0;
            _resultMessage = "";
            _chipDeltaMessage = "";
            _houseActionPending = false;
            _phase = BlackJackPhase.Betting;

            InitializeDeck();
        }


        

        public override void Input(ConsoleKeyInfo key)
        {
            switch (_phase)
            {
                case BlackJackPhase.Betting:
                    HandleBettingInput(key);
                    break;

                case BlackJackPhase.PlayerTurn:
                    HandlePlayerTurnInput(key);
                    break;

                // HouseReveal is fully automatic — no player input processed
                case BlackJackPhase.HouseReveal:
                    break;

                case BlackJackPhase.RoundOver:
                    // Any key advances to next round (or ends game)
                    _phase = BlackJackPhase.Betting;
                    break;

                case BlackJackPhase.GameOver:
                    // Any key returns to the main menu (Update() will return false)
                    break;
            }
        }

        private void HandleBettingInput(ConsoleKeyInfo key)
        {
            int increment = _betIncrements[_betIncrementIndex];

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    _bet = Math.Min(_bet + increment, _chips);
                    break;

                case ConsoleKey.DownArrow:
                    _bet = Math.Max(_bet - increment, increment); // never bet below one increment
                    break;

                case ConsoleKey.LeftArrow:
                case ConsoleKey.RightArrow:
                    // Cycle through available bet increment sizes
                    _betIncrementIndex = (_betIncrementIndex + 1) % _betIncrements.Length;
                    break;

                case ConsoleKey.Enter:
                    StartRound();
                    break;
            }
        }

        private void HandlePlayerTurnInput(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.H: // Hit
                    _playerCards.Add(DrawCard());
                    // If player hits exactly 21 or busts, move straight to reveal
                    if (CalculateScore(_playerCards) >= 21)
                        BeginHouseReveal();
                    break;

                case ConsoleKey.S: // Stand
                    BeginHouseReveal();
                    break;
            }
        }




        public override bool Update()
        {
            switch (_phase)
            {
                case BlackJackPhase.HouseReveal:
                    ProcessHouseRevealStep();
                    break;

                case BlackJackPhase.GameOver:
                    return false; // signals Program.cs to call End()
            }

            return true;
        }

        // Each call to Update() during HouseReveal processes exactly one step,
        // giving the house-hitting sequence a visible animated feel.
        private void ProcessHouseRevealStep()
        {
            int playerScore = CalculateScore(_playerCards);
            int houseScore  = CalculateScore(_houseCards);
            bool playerBusted = playerScore > 21;

            // If the player already busted, the house doesn't need to act
            if (playerBusted)
            {
                FinishRound();
                return;
            }

            // House hits if below 17, or below the player's score and not busted
            if (houseScore < 17 || (houseScore < playerScore && houseScore <= 21))
            {
                _houseCards.Add(DrawCard()); // one card per Update() tick = animation
            }
            else
            {
                FinishRound();
            }
        }


        

        public override void Render()
        {
            ScreenManager.ClearScreen();

            switch (_phase)
            {
                case BlackJackPhase.Betting:
                    RenderBettingScreen();
                    break;

                case BlackJackPhase.PlayerTurn:
                    RenderGameScreen(hideHouseCard: true);
                    RenderPlayerTurnHud();
                    break;

                case BlackJackPhase.HouseReveal:
                    RenderGameScreen(hideHouseCard: false);
                    RenderHouseRevealHud();
                    break;

                case BlackJackPhase.RoundOver:
                    RenderGameScreen(hideHouseCard: false);
                    RenderRoundOverHud();
                    break;

                case BlackJackPhase.GameOver:
                    RenderGameOverScreen();
                    break;
            }
        }

        private void RenderBettingScreen()
        {
            int increment = _betIncrements[_betIncrementIndex];

            ScreenManager.Print(1, 1,  "---- BLACK JACK ----");
            ScreenManager.Print(1, 2,  "---- Goal: 2,000 ---");
            ScreenManager.Print(1, 4,  $"Chips : {_chips}");
            ScreenManager.Print(1, 5,  $"Bet   : {_bet}");
            ScreenManager.Print(1, 7,  $"Step  : {increment}  (Left/Right to change)");
            ScreenManager.Print(1, 8,  "Up/Down to adjust bet");
            ScreenManager.Print(1, 9,  "Enter to deal");
        }

        private void RenderGameScreen(bool hideHouseCard)
        {
            ScreenManager.Print(1, 1, $"Chips: {_chips}  |  Bet: {_bet}");
            ScreenManager.Print(1, 2, $"Cards left in deck: {_cards.Count}");

            // House hand
            ScreenManager.Print(1, 4, "--- HOUSE HAND ---");
            RenderHandAt(3, 5, _houseCards, hideHouseCard);

            // Player hand
            int playerRow = 13; // enough room for one row of cards (6 lines) + spacing
            ScreenManager.Print(1, playerRow, "--- YOUR HAND ---");
            RenderHandAt(3, playerRow + 1, _playerCards, false);

            int playerScore = CalculateScore(_playerCards);
            ScreenManager.Print(1, playerRow + 8, $"Your total: {playerScore}");
        }

        // Renders a hand of cards using the fancy ASCII card display, starting at (x, y).
        // Each card is 9 chars wide + 2 spaces gap, so they're laid out horizontally.
        private void RenderHandAt(int startX, int startY, List<Card> hand, bool hideSecond)
        {
            if (hand.Count == 0) return;

            // Build visual lines for each card
            string[][] cardLines = new string[hand.Count][];
            for (int i = 0; i < hand.Count; i++)
                cardLines[i] = (hideSecond && i == 1)
                    ? Card.GetHiddenCardLines()
                    : hand[i].GetVisualLines();

            // Print row-by-row (6 rows per card)
            for (int row = 0; row < 6; row++)
            {
                int col = startX;
                for (int cardIdx = 0; cardIdx < cardLines.Length; cardIdx++)
                {
                    string line = cardLines[cardIdx][row];
                    // Guard: don't print past screen edge
                    if (col + line.Length < ScreenManager.GetScreenWidth())
                        ScreenManager.Print(col, startY + row, line);
                    col += line.Length + 2;
                }
            }
        }

        private void RenderPlayerTurnHud()
        {
            ScreenManager.Print(1, 22, "H = Hit   S = Stand");
        }

        private void RenderHouseRevealHud()
        {
            int houseScore = CalculateScore(_houseCards);
            ScreenManager.Print(1, 22, $"House total: {houseScore}  (House is playing...)");
        }

        private void RenderRoundOverHud()
        {
            int houseScore  = CalculateScore(_houseCards);
            int playerScore = CalculateScore(_playerCards);

            ScreenManager.Print(1, 22, $"House: {houseScore}  You: {playerScore}");
            ScreenManager.Print(1, 23, _resultMessage);
            ScreenManager.Print(1, 24, _chipDeltaMessage);
            ScreenManager.Print(1, 25, $"Chips: {_chips}");
            ScreenManager.Print(1, 26, "Press any key to continue...");
        }

        private void RenderGameOverScreen()
        {
            if (_chips >= 2000)
            {
                ScreenManager.Print(1, 1, "You reached 2,000 chips!");
                ScreenManager.Print(1, 2, "You win! Well done.");
            }
            else
            {
                ScreenManager.Print(1, 1, "You went bankrupt!");
                ScreenManager.Print(1, 2, "Better luck next time.");
            }
            ScreenManager.Print(1, 4, "Press any key to return to the menu...");
        }


        

        public override bool Pause()
        {
            return false; // Esc during blackjack exits program
        }


        public override void End()
        {
            if (_chips >= 2000)
            {
                //give the user a second to see that they won
                Thread.Sleep(3000);
                _context.TransitionTo(new Pong());
            }
            else
                _context.TransitionTo(new BlackJack()); // bankrupt, restart blackjack
        }


        

        private void StartRound()
        {
            DealInitialCards();
            _phase = BlackJackPhase.PlayerTurn;

            // If dealt 21 immediately, skip straight to reveal
            if (CalculateScore(_playerCards) == 21)
                BeginHouseReveal();
        }

        private void BeginHouseReveal()
        {
            _phase = BlackJackPhase.HouseReveal;
        }

        private void FinishRound()
        {
            int playerScore = CalculateScore(_playerCards);
            int houseScore  = CalculateScore(_houseCards);
            bool playerBusted = playerScore > 21;
            bool houseBusted  = houseScore  > 21;

            if (playerBusted)
            {
                _resultMessage    = $"Bust! You had {playerScore}. You lose.";
                _chipDeltaMessage = $"-- {_bet}";
                _chips -= _bet;
            }
            else if (houseBusted)
            {
                _resultMessage    = "House busts! You win!";
                _chipDeltaMessage = $"++ {_bet}";
                _chips += _bet;
            }
            else if (houseScore > playerScore)
            {
                _resultMessage    = $"House wins! ({houseScore} vs {playerScore})";
                _chipDeltaMessage = $"-- {_bet}";
                _chips -= _bet;
            }
            else if (playerScore > houseScore)
            {
                _resultMessage    = $"You win! ({playerScore} vs {houseScore})";
                _chipDeltaMessage = $"++ {_bet}";
                _chips += _bet;
            }
            else
            {
                _resultMessage    = "Push! (Tie)";
                _chipDeltaMessage = "Bet returned.";
                // chips unchanged
            }

            // Move used cards to discard pile
            _usedCards.AddRange(_playerCards);
            _usedCards.AddRange(_houseCards);

            // Check end conditions
            if (_chips >= 2000 || _chips <= 0)
            {
                // Show bankrupt reset note in result, then go to GameOver
                if (_chips <= 0)
                {
                    _resultMessage += " (Bankrupt — chips reset to 1,000)";
                    _chips = 1000;
                }
                _phase = BlackJackPhase.GameOver;
            }
            else
            {
                // Cap the bet to what the player can still afford before next round
                _bet = Math.Min(_bet, _chips);
                _phase = BlackJackPhase.RoundOver;
            }
        }

        

        private void InitializeDeck()
        {
            string[] suits = { "Spades", "Hearts", "Clubs", "Diamonds" };
            string[] symbols = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            int[] values = {   2,   3,   4,   5,   6,   7,   8,   9,   10,  10,  10,  10,  11 };

            _cards.Clear();
            for (int s = 0; s < suits.Length; s++)
                for (int i = 0; i < symbols.Length; i++)
                    _cards.Add(new Card(symbols[i], suits[s], values[i]));

            Shuffle();
        }

        private void Shuffle()
        {
            Random rng = new Random();
            _cards = _cards.OrderBy(x => rng.Next()).ToList();
        }

        private void DealInitialCards()
        {
            _playerCards.Clear();
            _houseCards.Clear();

            _playerCards.Add(DrawCard());
            _houseCards.Add(DrawCard());
            _playerCards.Add(DrawCard());
            _houseCards.Add(DrawCard());
        }

        private Card DrawCard()
        {
            // Reshuffle discard pile back in when deck runs out
            if (_cards.Count == 0)
            {
                _cards.AddRange(_usedCards);
                _usedCards.Clear();
                Shuffle();
            }
            Card c = _cards[0];
            _cards.RemoveAt(0);
            return c;
        }

        private int CalculateScore(List<Card> hand)
        {
            int total = hand.Sum(c => c.number);
            int aces  = hand.Count(c => c.cardSymbol == "A");

            // Aces count as 1 if treating them as 11 would bust
            while (total > 21 && aces > 0)
            {
                total -= 10;
                aces--;
            }
            return total;
        }
    }


    
    public class Card
    {
        public string cardSymbol;
        public string suit;
        public int    number;

        public Card(string c, string s, int n)
        {
            cardSymbol = c;
            suit       = s;
            number     = n;
        }

        // Fancy ASCII card — suits use unicode symbols
        public string[] GetVisualLines()
        {
            string s = " ";
            if (suit == "Spades")   s = " ♠"; //  /^\
            if (suit == "Hearts")   s = " ♥"; // (v)
            if (suit == "Clubs")    s = " ♣"; // (&)
            if (suit == "Diamonds") s = " ♦"; // <>

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

        // Hidden card back — used for the house's face-down card
        public static string[] GetHiddenCardLines()
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

        // Original side-by-side display helper — retained for any console-only use
        public static void DisplayHand(List<Card> hand, bool hideSecond = false)
        {
            string[][] cardLines = new string[hand.Count + (hideSecond ? 1 : 0)][];
            for (int i = 0; i < hand.Count; i++)
                cardLines[i] = hand[i].GetVisualLines();

            if (hideSecond)
                cardLines[1] = GetHiddenCardLines();

            for (int row = 0; row < 6; row++)
            {
                for (int cardIdx = 0; cardIdx < cardLines.Length; cardIdx++)
                    Console.Write(cardLines[cardIdx][row] + "  ");
                Console.WriteLine();
            }
        }
    }
}