﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTetris
{
    class ShapeLR : Shape
    {
        Block sideB1;
        Block sideB2;
        Block sideB3;

        Block[,] firstShapeMatrice = new Block[3, 2];

        /*_____BLOCK MATRICE TEMPLATE____
         * 
         *     [ [  B1,   N  ]
         *       [  O ,   N  ] 
         *       [  B2 ,  B3 ] ]
         *
         */

        public ShapeLR(int _x, int _y) : base(_x, _y)
        {
            sideB1 = new Block(_x, _y - 1);
            sideB2 = new Block(_x, _y + 1);
            sideB3 = new Block(_x + 1, _y + 1);

            currentShapeMatrice = firstShapeMatrice;
            currentShapeMatrice[1, 0] = originBlock;
            currentShapeMatrice[0, 0] = sideB1;
            currentShapeMatrice[2, 0] = sideB2;
            currentShapeMatrice[2, 1] = sideB3;

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
