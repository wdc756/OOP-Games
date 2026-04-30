using System;

namespace Games
{
    public class Context
    {
        private Game _game;

        public Context(Game game)
        {
            TransitionTo(game);
        }

        public void TransitionTo(Game game)
        {
            _game = game;
            _game.SetContext(this);
        }



        // Time in (s) between game updates
        public double UpdateTime()
        {
            return _game.UpdateTime();
        }
        
        // Reset game (including initial render)
        public void Reset()
        {
            _game.Reset();
        }
        
        // Render state of game to console
        public void Render()
        {
            _game.Render();
        }
        
        // Handle single-key inputs (string line inputs handled by game internally)
        public void Input(ConsoleKeyInfo key)
        {
            _game.Input(key);
        }

        // Updates the game logic (does not render)
        public bool Update()
        {
            return _game.Update();
        }
    }
}