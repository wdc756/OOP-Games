using System;

namespace Games
{
    public static class ScreenManager
    {
        // Use these to get screen dims
        public static int GetScreenWidth()
        {
            return Console.WindowWidth;
        }
        public static int GetScreenHeight()
        {
            return Console.WindowHeight;
        }

        
        
        // Use this to clear everything on the screen
        public static void ClearScreen()
        {
            Console.Clear();
        }

        // Use this to set a given point to empty char
        public static void ClearPoint(int x, int y)
        {
            if (x < 0 || x >= GetScreenWidth() || y < 0 || y >= GetScreenHeight())
                throw new IndexOutOfRangeException($"Can't clear point {x},{y} - out of range {GetScreenWidth()},{GetScreenHeight()}");
            
            Console.SetCursorPosition(x, y);
            Console.Write(" ");
        }
        
        // Use this to set a given point to the string provided
        public static void DrawPoint(int x, int y, string s)
        {
            if (x < 0 || x >= GetScreenWidth() || y < 0 || y >= GetScreenHeight())
                throw new IndexOutOfRangeException($"Can't draw point {x},{y} - out of range {GetScreenWidth()},{GetScreenHeight()}");
            
            Console.SetCursorPosition(x, y);
            Console.Write(s);
        }
    }
}