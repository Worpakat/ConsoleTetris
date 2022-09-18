﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTetris
{
    class ShapeStaireR : Shape
    {
        Block sideB1;
        Block sideB2;
        Block sideB3;

        Block[,] firstShapeMatrice = new Block[2, 3];

        /*_____BLOCK MATRICE TEMPLATE____
         * 
         *     [ [N  , B2 , B1 ]
         *       [B3 , O  , N ] ] 
         *      
         */

        public ShapeStaireR(int _x, int _y) : base(_x, _y)
        {
            sideB1 = new Block(_x + 1, _y - 1);
            sideB2 = new Block(_x, _y - 1);
            sideB3 = new Block(_x - 1, _y);

            currentShapeMatrice = firstShapeMatrice;
            currentShapeMatrice[1, 1] = originBlock;
            currentShapeMatrice[0, 2] = sideB1;
            currentShapeMatrice[0, 1] = sideB2;
            currentShapeMatrice[1, 0] = sideB3;

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
