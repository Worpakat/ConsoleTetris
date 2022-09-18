using System;
using System.Threading;
namespace ConsoleTetris
{
    class Program
    {
        static void Main(string[] args)
        {
            GameManager.RunTetrisGame();


            /////////////**/////////////

            //INPUT SYSTEM WİTH KEY ARROWS (TASLAK:bunun mantığında yapıcaz)

            //int xB = 0;
            //int yB = 0;

            //CanvasManager.AddBlock(xB, yB);


            //while (true)
            //{
            //    var input = Console.ReadKey(true).Key;
            //    switch (input)
            //    {
            //        case ConsoleKey.UpArrow :
            //            {
            //                if (yB <= 0) //Canvasın sınırındaysa break yapacak
            //                {
            //                    break;
            //                }

            //                CanvasManager.RemoveBlock(xB, yB);
            //                yB--;
            //                CanvasManager.AddBlock(xB, yB);

            //                break;
            //            }
            //        case ConsoleKey.DownArrow:
            //            {
            //                if (yB >= 19)
            //                {
            //                    break;
            //                }

            //                CanvasManager.RemoveBlock(xB, yB);
            //                yB++;
            //                CanvasManager.AddBlock(xB, yB);

            //                break;
            //            }
            //        case ConsoleKey.RightArrow:
            //            {
            //                if (xB >= 9)
            //                {
            //                    break;
            //                }

            //                CanvasManager.RemoveBlock(xB, yB);
            //                xB++;
            //                CanvasManager.AddBlock(xB, yB);

            //                break;
            //            }
            //        case ConsoleKey.LeftArrow:
            //            {
            //                if (xB <= 0)
            //                {
            //                    break;
            //                }

            //                CanvasManager.RemoveBlock(xB, yB);
            //                xB--;
            //                CanvasManager.AddBlock(xB, yB);

            //                break;
            //            }


            //    }
            //}




            //ShapeLL shapeLL1 = new ShapeLL(5, 1);

            //while (true)
            //{
            //    var input = Console.ReadKey(true).Key;
            //    switch (input)
            //    {
            //        case ConsoleKey.RightArrow:
            //            {
            //                shapeLL1.ShiftRight();

            //                break;
            //            }
            //        case ConsoleKey.LeftArrow:
            //            {
            //                shapeLL1.ShiftLeft();

            //                break;
            //            }
            //        case ConsoleKey.DownArrow:
            //            {
            //                shapeLL1.FallOnce();

            //                break;
            //            }
            //        case ConsoleKey.Spacebar:
            //            {
            //                shapeLL1.Rotate();

            //                break;
            //            }
            //        default:
            //            {


            //                break;
            //            }

            //    }

            //    shapeLL1.FallOnce();
            //    Thread.Sleep(1000);
            //}




        }
    }
}
