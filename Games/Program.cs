using System;
using System.Threading;
using Games.Games;

namespace Games
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Change the game here to change what runs first
            Context context = new Context(new Pong());

            // Clear screen and start game
            ScreenManager.ClearScreen();
            while (true)
            {
                // Check if key has been pressed
                if (Console.KeyAvailable)
                {
                    // Get key without printing
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                
                    // If key was escape, exit
                    if (keyInfo.Key == ConsoleKey.Escape)
                        break;
                    
                    // Else pass key
                    context.KeyInput(keyInfo);
                }

                // Else just update
                else
                {
                    context.Update(0.05);
                }
                
                // Sleep to avoid stack overflow
                Thread.Sleep(50); // (ms) - must match with Update
            }
            
            ScreenManager.ClearScreen();
            Console.WriteLine("Thanks for playing!");
        }
    }
}