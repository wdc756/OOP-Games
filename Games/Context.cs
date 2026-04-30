using System;

namespace Games
{
    public class Context
    {
        private Game game = null;

        public Context(Game game)
        {
            TransitionTo(game);
        }

        public void TransitionTo(Game game)
        {
            this.game = game;
            game.SetContext(this);
        }



        // Use this to update time-based games
        public void Update(double deltaTime)
        {
            game.Update(deltaTime);
        }

        // Use this to handle single-key inputs
        public void KeyInput(ConsoleKeyInfo key)
        {
            game.HandleKeyInput(key);
        }
    }
}