using System;

namespace Games.Games
{
    public class Pong : Game
    {
        private int _ballX = ScreenManager.GetScreenWidth() - 1;
        private int _ballY = ScreenManager.GetScreenHeight() / 2;
        private bool _ballMovingRight = false;
        private bool _ballMovingDown = false;
        private int _playerPaddleY = ScreenManager.GetScreenHeight() / 2;
        private int _aiPaddleY = ScreenManager.GetScreenHeight() / 2;



        private void UpdateBall()
        {
            ScreenManager.ClearPoint(_ballX, _ballY);
            if (_ballMovingRight) _ballX++;
            else _ballX--;
            if (_ballMovingDown) _ballY--;
            else _ballY++;
            if (_ballX < 0)
            {
                _ballX = 0;
                _ballMovingRight = true;
            }
            else if (_ballX >= ScreenManager.GetScreenWidth() - 1)
            {
                _ballX = ScreenManager.GetScreenWidth() - 1;
                _ballMovingRight = false;
            }
            if (_ballY < 0)
            {
                _ballY = 0;
                _ballMovingDown = false;
            }
            else if (_ballY >= ScreenManager.GetScreenHeight() - 1)
            {
                _ballY = ScreenManager.GetScreenHeight() - 1;
                _ballMovingDown = true;
            }
            ScreenManager.DrawPoint(_ballX, _ballY, "o");
        }

        public override void Update(double deltaTime)
        {
            UpdateBall();
        }

        public override void HandleKeyInput(ConsoleKeyInfo key)
        {
            
        }
    }
}