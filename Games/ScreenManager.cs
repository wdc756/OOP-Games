using System;

namespace Games
{
    public static class ScreenManager
    {
        // use these to get screen dims
        public static int GetScreenWidth()
        {
            return Console.WindowWidth;
        }
        public static int GetScreenHeight()
        {
            return Console.WindowHeight;
        }



        // clears the whole screen once (only call this on game start/reset, not every frame)
        public static void ClearScreen()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        // use this to clear a single point (erase a character)
        public static void ClearPoint(int x, int y)
        {
            if (x < 0 || x >= GetScreenWidth() || y < 0 || y >= GetScreenHeight())
                return;

            Console.SetCursorPosition(x, y);
            Console.Write(" ");
            Console.CursorVisible = false;
        }

        // use this to set a given point to the char provided
        public static void SetPoint(int x, int y, char c, ConsoleColor color = ConsoleColor.White)
        {
            if (x < 0 || x >= GetScreenWidth() || y < 0 || y >= GetScreenHeight())
                return;

            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(c);
            Console.CursorVisible = false;
        }

        // use this to print a string starting at a given coordinate
        public static void Print(int x, int y, string s, ConsoleColor color = ConsoleColor.White)
        {
            if (x < 0 || x >= GetScreenWidth() || y < 0 || y >= GetScreenHeight())
                return;
            if (x + s.Length > GetScreenWidth())
                return;

            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(s);
            Console.CursorVisible = false;
        }
    }
}