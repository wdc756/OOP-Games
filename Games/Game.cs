using System;

namespace Games
{
    public abstract class Game
    {
        // State-driven design context (used to switch to other states if needed)
        protected Context _context;
        
        // Time between Update() calls (ms)
        protected int _updateTime = 100;

        public void SetContext(Context context)
        {
            _context = context;
        }



        // Get the time (s) between Update() calls
        public int UpdateTime()
        {
            return _updateTime;
        }
        
        
        
        // The proper order for calling functions is:
        //   0. Reset on init (DO NOT RENDER)
        //   1. Input
        //   2. Update
        //   3. Render
        //   p: Pause
        //   n: End
        
        // Reset game to initial state - DO NOT RENDER
        public abstract void Reset();
        
        // Handle single-key inputs (only called when inputs are given)
        public abstract void Input(ConsoleKeyInfo key);
        
        // Main update loop (game logic) - returns if game ended
        //    true == keep running game
        //    false == game is over
        public abstract bool Update();
        
        // Render the current state of the game to the console
        public abstract void Render();
        
        // User pressed escape (pause/options menu) - returns if game has ended
        //     true == return to game
        //     false == go to main game selection menu
        public abstract bool Pause();
        
        // End of game logic (win screen, continue options, etc)
        public abstract void End();
    }
}