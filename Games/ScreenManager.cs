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
            // Clears the whole terminal window
            Console.Clear();
            
            // Removes the blinking cursor
            Console.CursorVisible = false;
            
            // Sets default text color
            Console.ForegroundColor = ConsoleColor.White;
        }

        // Use this to set a given point to empty char
        public static void ClearPoint(int x, int y)
        {
            if (x < 0 || x >= GetScreenWidth() || y < 0 || y >= GetScreenHeight())
                throw new IndexOutOfRangeException($"Can't clear point {x},{y} - out of range {GetScreenWidth()},{GetScreenHeight()}");
            
            Console.SetCursorPosition(x, y);
            Console.Write(" ");
            Console.CursorVisible = false;
        }
        
        // Use this to set a given point to the char provided
        public static void SetPoint(int x, int y, char c, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            
            if (x < 0 || x >= GetScreenWidth() || y < 0 || y >= GetScreenHeight())
                throw new IndexOutOfRangeException($"Can't draw point {x},{y} - out of range {GetScreenWidth()},{GetScreenHeight()}");
            
            Console.SetCursorPosition(x, y);
            Console.Write(c);
            Console.CursorVisible = false;
        }
        
        // Use this to print a string starting at a given coordinate
        public static void Print(int x, int y, string s, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            
            if (x < 0 || x >= GetScreenWidth() || y < 0 || y >= GetScreenHeight())
                throw new IndexOutOfRangeException($"Can't print at {x},{y} - out of range {GetScreenWidth()},{GetScreenHeight()}");
            if (x + s.Length > GetScreenWidth())
                throw new ArgumentException($"Cannot print string with length {s.Length} at {x} - out of range {GetScreenWidth()}");
            
            Console.SetCursorPosition(x, y);
            Console.Write(s);
            Console.CursorVisible = false;
        }
    }
}