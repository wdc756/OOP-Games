using System;
using System.Collections.Generic;

namespace Games.Games
{
    public class Snake : Game
    {
        // Game bounds
        private int width;
        private int height;

        // Snake body coords
        private Queue<int> snakeX;
        private Queue<int> snakeY;

        // Current directions
        // + x is rightward
        // and + y is downward in this case
        private int dirX;
        private int dirY;

        // Apple position
        private int appleX;
        private int appleY;
        // Used to generate random spawn points for apples
        Random rng = new Random();

        // Holds game state
        private bool gameOver;


        // scores for a win condition
        private int _score;
        private bool _gameWon;
        private int WIN_SCORE = 10;

        
        
        public Snake()
        {
            // Set update tick speed to 0.075s
            _updateTime = 75;
        }
        
        
        
        // checks if a position is in the snake's body
        private bool IsOnSnake(int x, int y)
        {
            // converts the queue to an array
            // so we can loop through and check values
            int[] xs = snakeX.ToArray();
            int[] ys = snakeY.ToArray();
            // if that position is the the queue
            // return true that it's "onSnake"
            for (int i = 0; i < xs.Length; i++)
            {
                if (xs[i] == x && ys[i] == y)
                    return true;
            }
            return false;
        }



        private void SpawnSnake()
        {
            // Spawn the first part of the snake
            // starting x and y are the middle of the grid
            int startX = width / 2;
            int startY = height / 2;
            // just putting it next in the queue
            // storing our snake body logic
            snakeX.Enqueue(startX);
            snakeY.Enqueue(startY);
        }
        
        private void SpawnApple()
        {
            // choose a random location for the apple
            do
            {
                // keep doing it until we find a position
                // that isn't where the snake is currently at
                appleX = rng.Next(1, width - 1);
                appleY = rng.Next(1, height - 1);
            } while (IsOnSnake(appleX, appleY));
        }
        
        public override void Reset()
        {
            // Set initial values
            width = 40;
            height = 20;
            snakeX = new Queue<int>();
            snakeY = new Queue<int>();
            dirX = 1;
            dirY = 0;
            appleX = 0;
            appleY = 0;
            rng = new Random();
            gameOver = false;

            // reset score
            _score = 0;
            _gameWon = false;
            
            // Setup initial snake body and apple
            SpawnSnake();
            SpawnApple();
        }



        private void HandleSnakeInput(ConsoleKeyInfo key)
        {
            // we can only go up if we weren't going down
            // otherwise we would run straight into our body
            if (key.Key == ConsoleKey.UpArrow && dirY != 1)
            {
                dirX = 0;
                dirY = -1;
            }
            // same for all the others
            // doesn't permit opposite movement bc that would 
            // immediately kill the snake
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
            // the end of the queue (array of our queue)
            // displays the most recent thing put it
            // add the direction and this is where we should be next
            int newHeadX = snakeX.ToArray()[snakeX.Count - 1] + dirX;
            int newHeadY = snakeY.ToArray()[snakeY.Count - 1] + dirY;

            // end if we went out of bounds
            if (newHeadX <= 0 || newHeadX >= width || newHeadY <= 0 || newHeadY >= height)
            {
                gameOver = true;
                return;
            }
            
            // end if the snake's new head moves onto its old body
            if (IsOnSnake(newHeadX, newHeadY))
            {
                gameOver = true;
                return;
            }
            
            // check if apple eaten (both coords of new head match)
            if (newHeadX == appleX && newHeadY == appleY)
            {
                // if so put the new position in the queue
                // without erasing the tail
                // since we grew one
                snakeX.Enqueue(newHeadX);
                snakeY.Enqueue(newHeadY);

                // move the score up since they at an apple
                _score++;
                // end game if they are greater than threshold of 10
                if (_score >= WIN_SCORE) { _gameWon = true; return; }
                
                // spawn a new apple for the snake to find
                SpawnApple();
            }
            else
            {
                // erase tail by taking out the end of the front of the queue
                _ = snakeX.Dequeue();
                _ = snakeY.Dequeue();

                // add new head
                snakeX.Enqueue(newHeadX);
                snakeY.Enqueue(newHeadY);

            }
        }

        public override bool Update()
        {
            // now we also have a win condition with the bool and
            // threshold of 10 apples
            if (gameOver || _gameWon)
            {
                return false;
            }
            UpdateSnakeApple();
            return true; // True means game is still running
        }



        private void RenderBorder()
        {
            // using the Screen manager we made
            // and the bounds we set earlier
            for (int x = 0; x <= width; x++)
            {
                ScreenManager.SetPoint(x, 0, '#', ConsoleColor.Red);
                ScreenManager.SetPoint(x, height, '#', ConsoleColor.Red);
            }
            for (int y = 0; y <= height; y++)
            {
                ScreenManager.SetPoint(0, y, '#', ConsoleColor.Red);
                ScreenManager.SetPoint(width, y, '#', ConsoleColor.Red);
            }
        }

        private void RenderSnake()
        {
            // converts the queue to an array
            // so we can loop through and check values
            int[] xs = snakeX.ToArray();
            int[] ys = snakeY.ToArray();
            
            // For each point, draw snake body
            for (int i = 0; i < xs.Length; i++)
            {
                ScreenManager.SetPoint(xs[i], ys[i], 'O', ConsoleColor.Green);
            }
        }

        private void RenderApple()
        {
            ScreenManager.SetPoint(appleX, appleY, '@', ConsoleColor.Yellow);
        }

        public override void Render()
        {
            ScreenManager.ClearScreen();
            RenderBorder();
            RenderSnake();
            RenderApple();
        }
        
        
        
        public override bool Pause()
        {
            return false; // No pause menu implemented - false = quit game
        }



        public override void End()
        {
            Console.ForegroundColor = ConsoleColor.White;
            if (_gameWon)
            {
                ScreenManager.Print(width / 2 - 4, height / 2, "YOU WIN! ");
                Console.ReadKey();
                _context.TransitionTo(new BlackJack());
            }
            else
            {
                ScreenManager.Print(width / 2 - 4, height / 2, "GAME OVER");
                Console.ReadKey();
            }
        }
    }
}