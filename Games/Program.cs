using System;
using System.Threading;
using Games.Games;

namespace Games
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Tell use about escape key
            Console.WriteLine("Press escape to skip through games");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            
            // Counter to keep track of # of games played, and context (state) to interact with
            int endCounter = 0;
            Context context = new Context(new Pong());
            
            context.Reset();
            // Loop until last state ends
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
                        // If we've played 3 games stop loop
                        endCounter++;
                        if (endCounter == 3) break;
                        
                        // If pause returns false transition to next state (done internally)
                        if (!context.Pause())
                        {
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
                    
                    // If we've played 3 games stop loop
                    endCounter++;
                    if (endCounter == 3) break;
                    
                    // After end screen assume state changed
                    context.Reset();
                    continue;
                }
                
                // Render each new frame
                context.Render();
                
                // Sleep using set timer to avoid constant stack calls
                Thread.Sleep(context.UpdateTime());
            }
            
            // Reset console
            ScreenManager.ClearScreen();
            Console.CursorVisible = true;
            Console.WriteLine("Thanks for playing!");
        }
    }
}