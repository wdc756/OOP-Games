using System;
using System.Threading;
using Games.Games;

namespace Games
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Context context = new Context(new Pong());

            ScreenManager.ClearScreen();
            while (true)
            {
                Thread.Sleep(50);
                context.Update(0.25);
            }
        }
    }
}