using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTetris
{
    class Coordinate
    {
        private int x;
        private int y;

        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public Coordinate(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public bool EqualTo(Coordinate otherCoordinate)
        {
            if (otherCoordinate.x == this.x && otherCoordinate.y == this.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
