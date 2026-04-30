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
            Reset();
        }

        private void Reset()
        {
            // Set all vars
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
            
            // Create initial object displays
            UpdateBall();
            DisplayPaddle(_playerPaddleX, _playerPaddleY, _playerPaddleHeight, "|");
            DisplayPaddle(_aiPaddleX, _aiPaddleY, _aiPaddleHeight, "|");
        }


        
        private void DisplayPaddle(int x, int y, int h, string paddle)
        {
            for (int i = 0; i < h; i++)
                ScreenManager.DrawPoint(x, y + i, paddle);
        }

        
        
        private void UpdateBall()
        {
            // Clear last location's marker
            ScreenManager.ClearPoint(_ballX, _ballY);
            
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
            }
            else if (_ballMovingRight && _ballX == _aiPaddleX &&
                     _ballY < _aiPaddleY + _aiPaddleHeight && _ballY > _aiPaddleY)
            {
                _ballMovingRight = false;
            }
            
            // Draw updated ball marker
            ScreenManager.DrawPoint(_ballX, _ballY, "o");
        }

        private void UpdateAI()
        {
            // Clear last paddle marker
            DisplayPaddle(_aiPaddleX, _aiPaddleY, _aiPaddleHeight, " ");
            
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
            
            // Display new paddle marker
            DisplayPaddle(_aiPaddleX, _aiPaddleY, _aiPaddleHeight, "|");
        }

        private void UpdatePlayer(ConsoleKeyInfo key)
        {
            // Clear last paddle marker
            DisplayPaddle(_playerPaddleX, _playerPaddleY, _playerPaddleHeight, " ");
            
            // Handle inputs
            if (key.Key == ConsoleKey.UpArrow && _playerPaddleY > 1)
            {
                _playerPaddleY--;
            }
            else if (key.Key == ConsoleKey.DownArrow && _playerPaddleY < ScreenManager.GetScreenHeight() - _playerPaddleHeight - 1)
            {
                _playerPaddleY++;
            }
            
            // Display new paddle marker
            DisplayPaddle(_playerPaddleX, _playerPaddleY, _playerPaddleHeight, "|");
        }
        
        private void DisplayGameOver()
        {
            ScreenManager.ClearScreen();
            Console.WriteLine("Game Over");
            Console.WriteLine($"{_gameWinner} Won!");
            
            // Wait for user to input something to clear the screen
            Console.ReadKey(true);
        }

        public override void Update(double deltaTime)
        {
            if (_gameOver)
            {
                DisplayGameOver();
                return;
            }
            UpdateAI();
            UpdateBall();
        }

        public override void HandleKeyInput(ConsoleKeyInfo key)
        {
            Update(0.0);
            UpdatePlayer(key);
        }
    }
}