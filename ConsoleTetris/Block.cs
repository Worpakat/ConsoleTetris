using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTetris
{
    class Block
    {
        public Coordinate coordinate;

        public Coordinate previousCoordinate;

        

        public Block(int _x, int _y)
        {
            coordinate = new Coordinate(_x, _y);
            previousCoordinate = coordinate;

            CanvasManager.AddBlock(coordinate);
            //MatriceManager.FillCoordinate(coordinate);

        }

        public void ResetMatricePlace(int x, int y)
        {
            //Blockun Matrice deki yerini değiştiren method

            //MatriceManager.RemoveCoordinate(xAxis, yAxis);
            //SetCoordinate(x, y);
            //MatriceManager.FillCoordinate(xAxis, yAxis);
        }

        public void ResetCanvasPlace(int x, int y)
        {
            //Blockun Canvas daki yerini değiştiren method

            //MatriceManager.RemoveCoordinate(coordinate);

            CanvasManager.RemoveBlock(coordinate);
            SetCoordinate(x, y);
            CanvasManager.AddBlock(coordinate);

            //MatriceManager.FillCoordinate(coordinate);
        }
        

        private void SetCoordinate(int x, int y)
        {
            //Block'un coordinatlarını değiştiren method

            //Değiştirmeden önce previous a kaydettik.
            previousCoordinate.X = coordinate.X;
            previousCoordinate.Y = coordinate.Y;

            coordinate.X = x;
            coordinate.Y = y;
        }

       
    }
}
