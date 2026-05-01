using System;
using System.Collections.Generic;

namespace Games.Games
{
    public class Snake : Game
    {
        // game bounds
        private int width;
        private int height;

        // snake body coords
        private Queue<int> snakeX;
        private Queue<int> snakeY;

        // current directions
        // + x is rightward
        // + y is downward
        private int dirX;
        private int dirY;

        // apple position
        private int appleX;
        private int appleY;
        private int _prevAppleX;
        private int _prevAppleY;

        // used to generate random spawn points for apples
        Random rng = new Random();

        // holds game state
        private bool gameOver;

        // win condition
        private int _score;
        private bool _gameWon;
        private int WIN_SCORE = 10;

        // tracks the last tail position so we can erase it each frame
        private int _lastTailX;
        private int _lastTailY;
        private bool _atePrevFrame;



        public Snake()
        {
            // set update tick speed to 0.15s
            _updateTime = 150;
        }



        // checks if a position is in the snake's body
        private bool IsOnSnake(int x, int y)
        {
            int[] xs = snakeX.ToArray();
            int[] ys = snakeY.ToArray();
            for (int i = 0; i < xs.Length; i++)
            {
                if (xs[i] == x && ys[i] == y)
                    return true;
            }
            return false;
        }



        private void SpawnSnake()
        {
            // spawn the snake in the middle of the grid
            int startX = width / 2;
            int startY = height / 2;
            snakeX.Enqueue(startX);
            snakeY.Enqueue(startY);
        }

        private void SpawnApple()
        {
            // pick a random spot that isn't on the snake
            do
            {
                appleX = rng.Next(1, width - 1);
                appleY = rng.Next(1, height - 1);
            } while (IsOnSnake(appleX, appleY));
        }

        public override void Reset()
        {
            // reset all vars
            width = 50;
            height = 30;
            snakeX = new Queue<int>();
            snakeY = new Queue<int>();
            dirX = 1;
            dirY = 0;
            appleX = 0;
            appleY = 0;
            _prevAppleX = -1;
            _prevAppleY = -1;
            rng = new Random();
            gameOver = false;
            _score = 0;
            _gameWon = false;
            _lastTailX = -1;
            _lastTailY = -1;
            _atePrevFrame = false;

            SpawnSnake();
            SpawnApple();

            // draw the screen once on reset, border stays up all game
            ScreenManager.ClearScreen();
            RenderBorder();
        }



        private void HandleSnakeInput(ConsoleKeyInfo key)
        {
            // can't reverse direction into yourself
            if (key.Key == ConsoleKey.UpArrow && dirY != 1)
            {
                dirX = 0;
                dirY = -1;
            }
            if (key.Key == ConsoleKey.DownArrow && dirY != -1)
            {
                dirX = 0;
                dirY = 1;
            }
            if (key.Key == ConsoleKey.LeftArrow && dirX != 1)
            {
                dirX = -1;
                dirY = 0;
            }
            if (key.Key == ConsoleKey.RightArrow && dirX != -1)
            {
                dirX = 1;
                dirY = 0;
            }
        }

        public override void Input(ConsoleKeyInfo key)
        {
            HandleSnakeInput(key);
        }



        private void UpdateSnakeApple()
        {
            // figure out where the head is going next based on current direction
            int newHeadX = snakeX.ToArray()[snakeX.Count - 1] + dirX;
            int newHeadY = snakeY.ToArray()[snakeY.Count - 1] + dirY;

            // end if out of bounds
            if (newHeadX <= 0 || newHeadX >= width || newHeadY <= 0 || newHeadY >= height)
            {
                gameOver = true;
                return;
            }

            // end if snake runs into itself
            if (IsOnSnake(newHeadX, newHeadY))
            {
                gameOver = true;
                return;
            }

            if (newHeadX == appleX && newHeadY == appleY)
            {
                // grow the snake by not removing the tail
                snakeX.Enqueue(newHeadX);
                snakeY.Enqueue(newHeadY);

                _score++;
                if (_score >= WIN_SCORE) { _gameWon = true; return; }

                // save old apple position so we can erase it
                _prevAppleX = appleX;
                _prevAppleY = appleY;
                _atePrevFrame = true;

                SpawnApple();
            }
            else
            {
                // save tail before removing it so we can erase it
                _lastTailX = snakeX.Peek();
                _lastTailY = snakeY.Peek();
                _atePrevFrame = false;

                snakeX.Dequeue();
                snakeY.Dequeue();

                snakeX.Enqueue(newHeadX);
                snakeY.Enqueue(newHeadY);
            }
        }

        public override bool Update()
        {
            if (gameOver || _gameWon) return false;
            UpdateSnakeApple();
            return true;
        }



        private void RenderBorder()
        {
            // draw the top and bottom walls
            for (int x = 0; x <= width; x++)
            {
                ScreenManager.SetPoint(x, 0, '#', ConsoleColor.Red);
                ScreenManager.SetPoint(x, height, '#', ConsoleColor.Red);
            }
            // draw the left and right walls
            for (int y = 0; y <= height; y++)
            {
                ScreenManager.SetPoint(0, y, '#', ConsoleColor.Red);
                ScreenManager.SetPoint(width, y, '#', ConsoleColor.Red);
            }
        }

        public override void Render()
        {
            // erase old tail (only when snake moved, not when it ate)
            if (_lastTailX >= 0 && !_atePrevFrame)
                ScreenManager.ClearPoint(_lastTailX, _lastTailY);

            // erase old apple position if snake just ate it
            if (_atePrevFrame && _prevAppleX >= 0)
                ScreenManager.ClearPoint(_prevAppleX, _prevAppleY);

            // draw new head only
            int[] xs = snakeX.ToArray();
            int[] ys = snakeY.ToArray();
            ScreenManager.SetPoint(xs[xs.Length - 1], ys[ys.Length - 1], 'O', ConsoleColor.Green);

            // draw apple
            ScreenManager.SetPoint(appleX, appleY, '@', ConsoleColor.Yellow);

            // show score
            ScreenManager.Print(1, height + 1, $"Score: {_score}/{WIN_SCORE}  ");
        }


        // some pause functionality Will added I think
        public override bool Pause()
        {
            return false;
        }

        public override void End()
        {
            Console.ForegroundColor = ConsoleColor.White;
            if (_gameWon)
            {
                // show win message, wait a bit, then move to the next game
                ScreenManager.Print(width / 2 - 4, height / 2, "YOU WIN! ");
                System.Threading.Thread.Sleep(2000);
                _context.TransitionTo(new BlackJack());
            }
            else
            {
                // show game over, wait a bit, then restart snake
                ScreenManager.Print(width / 2 - 4, height / 2, "GAME OVER");
                System.Threading.Thread.Sleep(2000);
                _context.TransitionTo(new Snake());
            }
        }
    }
}