using System;
using System.Collections.Generic;

namespace Games.Games
{
    public class Menu : Game
    {
        private Dictionary<String, Type> _games;
        private int _selectedGame;
        private bool _gameWasSelected;

        public Menu()
        {
            // Set time between updates to 0.5s
            _updateTime = 50;
        }



        public override void Reset()
        {
            _games = new Dictionary<String, Type>
            {
                { "Pong", typeof(Pong) },
                { "BlackJack", typeof(BlackJack) },
                { "Snake", typeof(Snake) },
            };
            _selectedGame = 0;
            _gameWasSelected = false;
        }




        private void UpdateSelection(ConsoleKeyInfo key)
        {
            // Update position
            if (key.Key == ConsoleKey.UpArrow) _selectedGame--;
            else if (key.Key == ConsoleKey.DownArrow) _selectedGame++;
            
            // Check bounds
            if (_selectedGame < 0) _selectedGame = _games.Count - 1;
            else if (_selectedGame >= _games.Count) _selectedGame = 0;
        }

        private void WasGameSelected(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Enter) _gameWasSelected = true;
        }
        
        public override void Input(ConsoleKeyInfo key)
        {
            UpdateSelection(key);
            WasGameSelected(key);
        }




        public override bool Update()
        {
            if (_gameWasSelected) return false;
            return true;
        }

        
        
        private void RenderTitle()
        {
            ScreenManager.Print(1, 1, "Game selection:");
            ScreenManager.Print(1, 2, "---------------");
        }

        private void RenderOption(int number, string name)
        {
            ScreenManager.Print(5, 4 + number, name);
        }

        private void RenderOptions()
        {
            int i = 0;
            foreach (KeyValuePair<String, Type> kvp in _games)
            {
                RenderOption(i, kvp.Key);
                i++;
            }
        }

        private void RenderSelection()
        {
            ScreenManager.Print(1, _selectedGame + 4, "->");
        }

        private void RenderExitInstructions()
        {
            ScreenManager.Print(1, _games.Count + 5, "To exit press Esc");
        }

        public override void Render()
        {
            ScreenManager.ClearScreen();
            RenderTitle();
            RenderOptions();
            RenderSelection();
            RenderExitInstructions();
        }



        public override bool Pause()
        {
            return true; // Nothing to pause on Menu
        }

        
        
        public override void End()
        {
            // Get game based on selected index
            int i = 0;
            Game game = null;
            foreach (KeyValuePair<String, Type> kvp in _games)
            {
                if (i == _selectedGame)
                {
                    Type type = kvp.Value;
                    game = (Game)Activator.CreateInstance(type);
                    break;
                }
                i++;
            }
            
            // Update global context with new selected game
            _context.TransitionTo(game);
        }
    }
}