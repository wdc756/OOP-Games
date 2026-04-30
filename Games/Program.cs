using System;
// this allows us to pause between frames
using System.Threading;
// this allows us to have the stopwatch and time
using System.Diagnostics;
using Games.Games;


namespace Games
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // initialize the context to start w snake state (state 0)
            // the snake will transition to the other states
            Context context = new Context(new Snake());

            Stopwatch stopwatch = Stopwatch.StartNew();
            // tracks the most recent frame
            double lastTime = 0;

            // game runs forever until we close the terminal
            while (true)
            {
                // current time tracker (regardless of frames)
                double now = stopwatch.Elapsed.TotalSeconds;
                // tracks the time between the last frame and now
                double deltaTime = now - lastTime;
                // updating current frame time to current time
                lastTime = now;

                // handles key presses
                if (Console.KeyAvailable)
                {
                    string key = Console.ReadKey(true).Key.ToString();
                    // sends the string of what was pressed to the current running game
                    context.KeyInput(key);
                }

                context.Update(deltaTime);
                // slows down this loop at the end
                // so we go through it essentially a frame at a time
                Thread.Sleep(10);
            }
        }
    }
}