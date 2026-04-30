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
                    // If current state is menu, it handles end differently so just continue
                    if (context.IsMainMenu())
                    {
                        context.End();
                        continue;
                    }
                    
                    // If we're here, it means the game ended, so send to end screen
                    context.End();
                    
                    // After end screen transition to game selection menu
                    context.TransitionTo(new Menu());
                    context.Reset();
                }
                
                // Render each new frame
                context.Render();
                
                // Sleep using set timer to avoid constant stack calls
                Thread.Sleep(context.UpdateTime());
            }
            
            ScreenManager.ClearScreen();
            Console.WriteLine("Thanks for playing!");
        }
    }
}