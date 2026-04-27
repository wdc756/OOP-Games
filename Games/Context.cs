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



        // Use this to handle single-key inputs
        public void KeyInput(string key)
        {
            game.HandleKeyInput(key);
        }

        // Use this to handle multi-char string inputs
        public void Input(string input)
        {
            game.HandleInput(input);
        }
    }
}