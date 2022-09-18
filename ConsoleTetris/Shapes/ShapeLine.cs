using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTetris
{
    class ShapeLine : Shape
    {
        Block sideB1;
        Block sideB2;
        Block sideB3;

        Block[,] firstShapeMatrice = new Block[1, 4];

        /*_____BLOCK MATRICE TEMPLATE____
         * 
         *     [ [B3 , O , B2 , B1] ]
         *
         */

        public ShapeLine(int _x, int _y) : base(_x, _y)
        {
            sideB1 = new Block(_x + 2, _y);
            sideB2 = new Block(_x + 1, _y);
            sideB3 = new Block(_x - 1, _y);

            currentShapeMatrice = firstShapeMatrice;
            currentShapeMatrice[0, 1] = originBlock;
            currentShapeMatrice[0, 3] = sideB1;
            currentShapeMatrice[0, 2] = sideB2;
            currentShapeMatrice[0, 0] = sideB3;

            blocksRotateQueue = new Block[3] { sideB1, sideB2, sideB3 };

            DrawRotateMatrices();
        }

        public override bool FallOnce()
        {
            return base.FallOnce();
        }
        public override void ShiftLeft()
        {
            base.ShiftLeft();
        }
        public override void ShiftRight()
        {
            base.ShiftRight();
        }
        public override void Rotate()
        {
            base.Rotate();
        }

    }
}
