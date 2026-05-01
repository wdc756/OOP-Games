using System;

namespace Games.Games
{
    public class Pong : Game
    {
        private int _ballX;
        private int _ballY;
        private bool _ballMovingRight;
        private bool _ballMovingDown;
        private int _playerPaddleX;
        private int _playerPaddleY;
        private int _playerPaddleHeight;
        private int _aiPaddleX;
        private int _aiPaddleY;
        private int _aiPaddleHeight;
        private bool _aiMovingDown;
        private bool _gameOver;
        private string _gameWinner;

        
        
        public Pong()
        {
            // Set update timer to 0.05s
            _updateTime = 50;
        }

        
        
        public override void Reset()
        {
            _ballX = ScreenManager.GetScreenWidth() - 3;
            _ballY = ScreenManager.GetScreenHeight() / 2;
            _ballMovingRight = false;
            _ballMovingDown = false;
            _playerPaddleX = 1;
            _playerPaddleY = ScreenManager.GetScreenHeight() / 2;
            _playerPaddleHeight = 5;
            _aiPaddleX = ScreenManager.GetScreenWidth() - 2;
            _aiPaddleY = ScreenManager.GetScreenHeight() / 2;
            _aiPaddleHeight = 7;
            _aiMovingDown = false;
            _gameOver = false;
            _gameWinner = "";
        }
        
        
        
        private void HandlePlayerInput(ConsoleKeyInfo key)
        {
            // Handle inputs
            if (key.Key == ConsoleKey.UpArrow && _playerPaddleY > 1)
            {
                _playerPaddleY--;
            }
            else if (key.Key == ConsoleKey.DownArrow && _playerPaddleY < ScreenManager.GetScreenHeight() - _playerPaddleHeight - 1)
            {
                _playerPaddleY++;
            }
        }

        public override void Input(ConsoleKeyInfo key)
        {
            HandlePlayerInput(key);
        }

        
        
        private void UpdateBall()
        {
            // Move diagonally
            if (_ballMovingRight) _ballX++;
            else _ballX--;
            if (_ballMovingDown) _ballY--;
            else _ballY++;
            
            // Handle bounds
            if (_ballX < 1)
            {
                _gameOver = true;
                _gameWinner = "AI";
                _ballX = 1;
            }
            else if (_ballX >= ScreenManager.GetScreenWidth() - 1)
            {
                _gameOver = true;
                _gameWinner = "You";
                _ballX = ScreenManager.GetScreenWidth() - 1;
            }
            if (_ballY < 1)
            {
                _ballY = 1;
                _ballMovingDown = false;
            }
            else if (_ballY >= ScreenManager.GetScreenHeight() - 1)
            {
                _ballY = ScreenManager.GetScreenHeight() - 1;
                _ballMovingDown = true;
            }
            
            // Handle paddles
            if (!_ballMovingRight && _ballX == _playerPaddleX && 
                _ballY < _playerPaddleY + _playerPaddleHeight && _ballY > _playerPaddleY)
            {
                _ballMovingRight = true;
                _ballX = _playerPaddleX + 1;
            }
            else if (_ballMovingRight && _ballX == _aiPaddleX &&
                     _ballY < _aiPaddleY + _aiPaddleHeight && _ballY > _aiPaddleY)
            {
                _ballMovingRight = false;
                _ballX = _aiPaddleX - 1;
            }
        }

        private void UpdateAI()
        {
            // Update position
            if (_aiMovingDown) _aiPaddleY++;
            else _aiPaddleY--;
            
            // Handle bounds
            if (_aiPaddleY < 1)
            {
                _aiMovingDown = true;
                _aiPaddleY = 1;
            }
            else if (_aiPaddleY + _aiPaddleHeight >= ScreenManager.GetScreenHeight() - 1)
            {
                _aiMovingDown = false;
                _aiPaddleY = ScreenManager.GetScreenHeight() - _aiPaddleHeight - 1;
            }
        }

        public override bool Update()
        {
            if (_gameOver) return false;
            UpdateBall();
            UpdateAI();
            return true;
        }
        
        
        
        private void RenderBall()
        {
            ScreenManager.SetPoint(_ballX, _ballY, 'o');
        }

        private void RenderPaddle(int x, int y, int h, char c)
        {
            for (int i = 0; i < h; i++)
                ScreenManager.SetPoint(x, y + i, c);
        }

        private void RenderPaddles()
        {
            RenderPaddle(_playerPaddleX, _playerPaddleY, _playerPaddleHeight, '|');
            RenderPaddle(_aiPaddleX, _aiPaddleY, _aiPaddleHeight, '|');
        }

        public override void Render()
        {
            ScreenManager.ClearScreen();
            RenderBall();
            RenderPaddles();
        }


        public override bool Pause()
        {
            return false; // No pause menu implemented - false = quit game
        }

        
        
        public override void End()
        {
            ScreenManager.ClearScreen();
            Console.WriteLine("Game Over");
            Console.WriteLine($"{_gameWinner} Won!");
            
            // Wait for user to input something to clear the screen
            Console.ReadKey(true);
        }
    }
}