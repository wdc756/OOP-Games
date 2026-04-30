using System;
using Games.Games;

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
            _game.Reset();
        }

        public bool IsMainMenu()
        {
            return _game is Menu;
        }



        // Time in (s) between game updates
        public int UpdateTime()
        {
            return _game.UpdateTime();
        }
        
        
        
        // The proper order for calling functions is:
        //   0. Reset on init - DO NOT RENDER
        //   1. Input
        //   2. Update
        //   3. Render
        //   p: Pause
        //   n: End
        
        // Reset game - DO NOT RENDER
        public void Reset()
        {
            _game.Reset();
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
        
        // Render state of game to console
        public void Render()
        {
            _game.Render();
        }
        
        // Pause state
        public bool Pause()
        {
            return _game.Pause();
        }
        
        // End of game state
        public void End()
        {
            _game.End();
        }
    }
}