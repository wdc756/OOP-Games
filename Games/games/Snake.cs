using System;
using System.Collections.Generic;

namespace Games.Games

{
    public class Snake : Game
    {
        // setting up bounds for our borders
        int width = 40;
        int height = 20;

        // we will use two queues for the coordinates
        // that the snake's body is taking up
        Queue<int> snakeX = new();
        Queue<int> snakeY = new();

        // these are our current directions
        // + x is rightward
        // and + y is downward in this case
        int dirX = 1;
        int dirY = 0;

        // apple position
        int appleX;
        int appleY;
        // c# inbuilt random libary
        // need this to give the apple a random spawn
        Random rng = new Random();

        double timer = 0;
        // this is our time threshold for each frame
        double speed = 0.15;

        bool gameOver = false;
        double gameOverTimer = 0;

        public Snake()
        {
            // had to look this up
            // hides the blinking cursor so we just have the game uninterupted
            Console.CursorVisible = false;
            Console.Clear();
            DrawBorder();
            SpawnSnake();
            SpawnApple();
        }

        void DrawBorder()
        {
            // displays text in different colors
            // our border will be red
            Console.ForegroundColor = ConsoleColor.Red;
            // using the Screen manager we made
            // and the bounds we set earlier
            for (int x = 0; x <= width; x++)
            {
                ScreenManager.DrawPoint(x, 0, "#");
                ScreenManager.DrawPoint(x, height, "#");
            }
            for (int y = 0; y <= height; y++)
            {
                ScreenManager.DrawPoint(0, y, "#");
                ScreenManager.DrawPoint(width, y, "#");
            }
        }

        void SpawnSnake()
        {
            // starting x and y are the middle of the grid
            int startX = width / 2;
            int startY = height / 2;
            // just putting it next in the queue
            // storing our snake body logic
            snakeX.Enqueue(startX);
            snakeY.Enqueue(startY);

            Console.ForegroundColor = ConsoleColor.Green;
            // we are using green and the "O" char for the snake body
            ScreenManager.DrawPoint(startX, startY, "O");
        }

        void SpawnApple()
        {
            // choose a random location for the apple
            do
            {
                // keep doing it until we find a position
                // that isn't where the snake is currently at
                appleX = rng.Next(1, width - 1);
                appleY = rng.Next(1, height - 1);
            } while (IsOnSnake(appleX, appleY));

            // apple is yellow and uses the @ char
            Console.ForegroundColor = ConsoleColor.Yellow;
            ScreenManager.DrawPoint(appleX, appleY, "@");
        }

        // checks if a position is in the snake's body
        bool IsOnSnake(int x, int y)
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


        // this function doesn't do anything except
        // and wait for a frame's timer to be finished
        // and this allows us to wait before updating the
        // next snake movement or "tick"
        public override void Update(double deltaTime)
        {
            // gameOver is initially false
            // set to true if they hit a wall
            // or snake body
            if (gameOver)
            {
                gameOverTimer += deltaTime;
                // wait's two seconds after game over
                if (gameOverTimer >= 2.0)
                {
                    // and then transitions to state 2 (possibly pong)
                    _context.TransitionTo(new Pong());
                }
                return;
            }
            // adding the bits of time to reach a frame
            timer += deltaTime;
            if (timer < speed)
            {
                return;
            }
            // resets to zero after the frame time threshold is hit
            timer = 0;

            // the end of the queue (array of our queue)
            // displays the most recent thing put it
            // add the direction and this is where we should be next
            int newHeadX = snakeX.ToArray()[snakeX.Count - 1] + dirX;
            int newHeadY = snakeY.ToArray()[snakeY.Count - 1] + dirY;

            // end if we went out of bounds
            if (newHeadX <= 0 || newHeadX >= width || newHeadY <= 0 || newHeadY >= height)
            {
                // display GAME OVER message
                gameOver = true;
                Console.ForegroundColor = ConsoleColor.White;
                ScreenManager.DrawPoint(width / 2 - 4, height / 2, "GAME OVER");
                return;
            }
            
            // end if the snake's new head moves onto its old body
            if (IsOnSnake(newHeadX, newHeadY))
            {
                gameOver = true;
                Console.ForegroundColor = ConsoleColor.White;
                ScreenManager.DrawPoint(width / 2 - 4, height / 2, "GAME OVER");
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
                // spawn a new apple for the snake to find
                SpawnApple();
            }
            else
            {
                // erase tail by taking out the end of the front of teh queue
                int tailX = snakeX.Dequeue();
                int tailY = snakeY.Dequeue();
                // take away teh visual
                ScreenManager.ClearPoint(tailX, tailY);

                // add new head
                snakeX.Enqueue(newHeadX);
                snakeY.Enqueue(newHeadY);
            }

            // draw new head (happens either way, just doesn't erase if apple eaten)
            Console.ForegroundColor = ConsoleColor.Green;
            ScreenManager.DrawPoint(newHeadX, newHeadY, "O");
        }


        // these are all translations for a key press
        // to a direction that will be added to the current head position
        public override void HandleKeyInput(string key)
        {
            // we can only go up if we weren't going down
            // otherwise we would run straight into our body
            if (key == "UpArrow" && dirY != 1)
            {
                dirX = 0;
                dirY = -1;
            }
            // same for all the others
            // doesn't permit opposite movement bc that would 
            // immediately kill the snake
            if (key == "DownArrow" && dirY != -1)
            {
                dirX = 0;
                dirY = 1;
            }
            if (key == "LeftArrow" && dirX != 1)
            {
                dirX = -1;
                dirY = 0;
            }
            if (key == "RightArrow" && dirX != -1)
            {
                dirX = 1;
                dirY = 0;
            }
        }

        public override void HandleInput(string input)
        {
            // here is arbitrary implementation bc Will made an abstract class
        }
        

        
    }
}