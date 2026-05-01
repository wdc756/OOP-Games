using System;
using System.Threading;
using Games.Games;

namespace Games
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;

            // flush any buffered keypresses before starting
            while (Console.KeyAvailable) Console.ReadKey(true);

            // start with pong, states transition themselves on win
            Context context = new Context(new Pong());

            while (true)
            {
                // check if a key has been pressed
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    // escape quits
                    if (key.Key == ConsoleKey.Escape) break;

                    context.Input(key);
                }

                // update game logic
                if (!context.Update())
                {
                    context.End();
                    // flush buffered keys so next game starts clean
                    while (Console.KeyAvailable) Console.ReadKey(true);
                    context.Reset();
                    continue;
                }

                // render current frame
                context.Render();

                Thread.Sleep(context.UpdateTime());
            }

            // clean up on exit
            ScreenManager.ClearScreen();
            Console.CursorVisible = true;
            Console.WriteLine("Thanks for playing!");
        }
    }
}