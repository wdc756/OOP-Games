using System;
using System.Threading;
using Games.Games;

namespace Games
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Context context = new Context(new Menu());

            context.Reset();
            while (true)
            {
                // Check if key has been pressed
                if (Console.KeyAvailable)
                {
                    // Get key without printing
                    ConsoleKeyInfo key = Console.ReadKey(true);
                
                    // If key was escape, pause game
                    if (key.Key == ConsoleKey.Escape)
                    {
                        // If current game is Menu, quit program
                        if (context.IsMainMenu())
                        {
                            break;
                        }
                        
                        // If pause returns false go back to main menu
                        if (!context.Pause())
                        {
                            context.TransitionTo(new Menu());
                            context.Reset();
                            continue;
                        }
                    }
                    
                    // Else pass key
                    context.Input(key);
                }
                
                // Update game
                if (!context.Update())
                {
                    // If we're here, it means the game ended, so send to end screen
                    context.End();
                    
                    // After end screen transition to game selection menu
                    context.TransitionTo(new Menu());
                    context.Reset();
                }
            }
            
            ScreenManager.ClearScreen();
            Console.WriteLine("Thanks for playing!");
        }
    }
}