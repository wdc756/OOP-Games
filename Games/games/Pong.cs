using System;
using System.Threading;

namespace Games.Games
{
    public class Pong : Game
    {
        // ball position and previous position (for erasing)
        private int _ballX;
        private int _ballY;
        private int _prevBallX;
        private int _prevBallY;
        private bool _ballMovingRight;
        private bool _ballMovingDown;

        // player paddle position, previous position, and height
        private int _playerPaddleX;
        private int _playerPaddleY;
        private int _prevPlayerPaddleY;
        private int _playerPaddleHeight;

        // ai paddle position, previous position, and height
        private int _aiPaddleX;
        private int _aiPaddleY;
        private int _prevAiPaddleY;
        private int _aiPaddleHeight;
        private bool _aiMovingDown;
        private int _aiMoveCounter; // throttles ai speed, only moves every 3 frames

        private bool _gameOver;
        private string _gameWinner;



        public Pong()
        {
            // set update timer to 0.08s
            _updateTime = 80;
        }



        public override void Reset()
        {
            _ballX = ScreenManager.GetScreenWidth() / 2;
            _ballY = ScreenManager.GetScreenHeight() / 2;
            _prevBallX = _ballX;
            _prevBallY = _ballY;
            _ballMovingRight = false;
            _ballMovingDown = false;
            _playerPaddleX = 1;
            _playerPaddleY = ScreenManager.GetScreenHeight() / 2;
            _prevPlayerPaddleY = _playerPaddleY;
            _playerPaddleHeight = 8;
            _aiPaddleX = ScreenManager.GetScreenWidth() - 2;
            _aiPaddleY = ScreenManager.GetScreenHeight() / 2;
            _prevAiPaddleY = _aiPaddleY;
            _aiPaddleHeight = 3;
            _aiMovingDown = false;
            _aiMoveCounter = 0;
            _gameOver = false;
            _gameWinner = "";

            ScreenManager.ClearScreen();

            // draw paddles once on reset
            for (int i = 0; i < _playerPaddleHeight; i++)
                ScreenManager.SetPoint(_playerPaddleX, _playerPaddleY + i, '|');
            for (int i = 0; i < _aiPaddleHeight; i++)
                ScreenManager.SetPoint(_aiPaddleX, _aiPaddleY + i, '|');
        }



        public override void Input(ConsoleKeyInfo key)
        {
            // save previous position before moving
            _prevPlayerPaddleY = _playerPaddleY;

            if (key.Key == ConsoleKey.UpArrow && _playerPaddleY > 1)
                _playerPaddleY -= 2;
            else if (key.Key == ConsoleKey.DownArrow && _playerPaddleY < ScreenManager.GetScreenHeight() - _playerPaddleHeight - 1)
                _playerPaddleY += 2;
        }



        private void UpdateBall()
        {
            // save previous position so we can erase it next render
            _prevBallX = _ballX;
            _prevBallY = _ballY;

            // move ball diagonally
            if (_ballMovingRight) _ballX++;
            else _ballX--;
            if (_ballMovingDown) _ballY--;
            else _ballY++;

            // handle screen bounds
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

            // handle paddle collisions
            if (!_ballMovingRight && _ballX == _playerPaddleX &&
                _ballY >= _playerPaddleY && _ballY < _playerPaddleY + _playerPaddleHeight)
            {
                _ballMovingRight = true;
                _ballX = _playerPaddleX + 1;
            }
            else if (_ballMovingRight && _ballX == _aiPaddleX &&
                     _ballY >= _aiPaddleY && _ballY < _aiPaddleY + _aiPaddleHeight)
            {
                _ballMovingRight = false;
                _ballX = _aiPaddleX - 1;
            }
        }

        private void UpdateAI()
        {
            // only move every 3 frames to keep the ai slow and beatable
            _aiMoveCounter++;
            if (_aiMoveCounter % 3 != 0) return;

            // save previous position so we can erase it next render
            _prevAiPaddleY = _aiPaddleY;

            if (_aiMovingDown) _aiPaddleY++;
            else _aiPaddleY--;

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



        public override void Render()
        {
            // erase and redraw ball
            ScreenManager.ClearPoint(_prevBallX, _prevBallY);
            ScreenManager.SetPoint(_ballX, _ballY, 'o');

            // erase and redraw player paddle
            for (int i = 0; i < _playerPaddleHeight; i++)
                ScreenManager.ClearPoint(_playerPaddleX, _prevPlayerPaddleY + i);
            for (int i = 0; i < _playerPaddleHeight; i++)
                ScreenManager.SetPoint(_playerPaddleX, _playerPaddleY + i, '|');

            // erase and redraw ai paddle
            for (int i = 0; i < _aiPaddleHeight; i++)
                ScreenManager.ClearPoint(_aiPaddleX, _prevAiPaddleY + i);
            for (int i = 0; i < _aiPaddleHeight; i++)
                ScreenManager.SetPoint(_aiPaddleX, _aiPaddleY + i, '|');
        }



        public override bool Pause()
        {
            return false; // no pause menu, false = quit game
        }

        public override void End()
        {
            ScreenManager.ClearScreen();
            Console.WriteLine("Game Over");
            Console.WriteLine($"{_gameWinner} Won!");
            Thread.Sleep(2000);

            // only move to next state if player won
            if (_gameWinner == "You")
                _context.TransitionTo(new Snake());
            else
                _context.TransitionTo(new Pong());
        }
    }
}