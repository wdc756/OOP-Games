namespace Games
{
    public abstract class Game
    {
        protected Context _context;

        public void SetContext(Context context)
        {
            this._context = context;
        }



        public abstract void Update(double deltaTime);

        public abstract void HandleKeyInput(string key);

        public abstract void HandleInput(string input);
    }
}