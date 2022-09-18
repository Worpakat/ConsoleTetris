using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTetris
{
    class ShapeSq : Shape 
    {
        Block sideB1;
        Block sideB2;
        Block sideB3;

        Block[,] firstShapeMatrice = new Block[2, 2];

        /*_____BLOCK MATRICE TEMPLATE____
         * 
         *                                                            O,  B1          
         *      ROTATE YAPMAYACAĞIMIZ İÇİN İHTİYAÇ YOK AM BASİTÇE     B2, B3  ŞEKLİNDE
         *
         */

        public ShapeSq(int _x, int _y) : base(_x, _y)
        {
            sideB1 = new Block(_x + 1, _y);
            sideB2 = new Block(_x, _y + 1);
            sideB3 = new Block(_x + 1, _y + 1);

            currentShapeMatrice = firstShapeMatrice;
            currentShapeMatrice[0, 0] = originBlock;
            currentShapeMatrice[0, 1] = sideB1;
            currentShapeMatrice[1, 0] = sideB2;
            currentShapeMatrice[1, 1] = sideB3;
            
            blocksRotateQueue = new Block[3] { sideB1, sideB2, sideB3 };
        }

        public override bool FallOnce()
        {
            return base.FallOnce();
        }
        public override void ShiftRight()
        {
            base.ShiftRight();
        }
        public override void ShiftLeft()
        {
            base.ShiftLeft();
        }
        public override void Rotate()
        {
            //Kareyi döndürmenin bir mantığı yok.
        }
    }
}
