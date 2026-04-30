using System;

namespace Games
{
    public abstract class Game
    {
        // State-driven design context (used to switch to other states if needed)
        protected Context _context;
        
        // Time between Update() calls
        protected Double updateTime;

        public void SetContext(Context context)
        {
            _context = context;
            updateTime = 1.0;
        }



        // Get the time (s) between Update() calls
        public double UpdateTime()
        {
            return updateTime;
        }
        
        
        
        // Reset game to initial state (including display)
        public abstract void Reset();

        // Render the current state of the game to the console
        public abstract void Render();

        // Handle single-key inputs (only called when inputs are given)
        public abstract void Input(ConsoleKeyInfo key);
        
        // Main update loop (game logic) - returns if game ended
        public abstract bool Update();
    }
}