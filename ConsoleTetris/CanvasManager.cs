using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTetris
{
    static class CanvasManager
    {
        public static void AddBlock(Coordinate coordinate)
        {
            Console.SetCursorPosition(3*coordinate.X + 11, coordinate.Y + 6);
            Console.Write("[ ]");
        }
        public static void AddBlock(int x, int y)
        {
            Console.SetCursorPosition(3 * x + 11, y + 6);
            Console.Write("[ ]");
        }

        public static void RemoveBlock(Coordinate coordinate)
        {
            Console.SetCursorPosition(3 * coordinate.X + 11, coordinate.Y + 6);
            Console.Write("   ");
        }
        public static void RemoveBlock(int x, int y)
        {
            Console.SetCursorPosition(3 * x + 11, y + 6);
            Console.Write("   ");
        }
    }
}
