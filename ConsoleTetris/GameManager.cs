using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace ConsoleTetris
{
    static class GameManager
    {
        static Shape currentShape = null;
        static bool getInputIsActive = false;
        public static void RunTetrisGame()
        {
            //Oyunun Ana Çalıştırıcı/Döngü Methodu

            DrawCanvas(); //Oyunun ana canvasını çizdik

            Thread inputThread = new Thread(InputGetter);

            inputThread.Start();
            

            bool gameRunning = true;
            bool gameOver = false;
            bool isShapeStabilized = false;
            bool rowRemoved = false;

            while (gameRunning)
            {

                while (!gameOver)
                {
                    rowRemoved = CheckAndRemoveRows(currentShape);
                    
                    if(!rowRemoved) //Herhangi bir satır silinmişse oyun daha bitmemiştir. Bakmaya gerek yok.
                    {
                        if (currentShape != null)
                        {
                            /*Son stabile olan shapein block larının Y lerine bakılır
                             * Eğer herhangi birisi 1 veya 1 dan küçükse oyun bitmiştir.
                             */

                            gameOver = CheckGameOver(currentShape);
                            
                            if (gameOver)
                            {
                                Console.SetCursorPosition(21, 5);
                                Console.WriteLine("GAME OVER!");
                                Thread.Sleep(5000);
                                Environment.Exit(-1);
                                break;
                            }
                        }
                    }
                    
                    currentShape = InitializeRandomShape();

                    isShapeStabilized = false; //Yeni init. edilen shape stabil değildir.

                    getInputIsActive = true;
                    //input system activated

                    while (!isShapeStabilized)
                    {
                        Thread.Sleep(1000);
                        isShapeStabilized = currentShape.FallOnce();
                    }

                    getInputIsActive = false;
                    //input system deactivated
                }
            }
        }

        private static void DrawCanvas()
        {
            //BASIC CANVAS

            Console.SetCursorPosition(10, 5);
            Console.CursorVisible = false;

            Console.WriteLine("————————————————————————————————");

            for (int i = 0; i < 20; i++)
            {
                Console.SetCursorPosition(10, i + 6);
                Console.WriteLine("|                              |");
            }

            Console.SetCursorPosition(10, 26);
            Console.WriteLine("————————————————————————————————");
        }
        private static bool CheckGameOver(Shape lastStabilizedShape)
        {
            //Son shape in kordinatlarına bakıp oyunun bitip bitmediğini return eden method.
            //Oyun bittiyse true bitmediyse false

            if (lastStabilizedShape.shapeCoordinate.Y > 4) //Shape origini Y si 4 den büyükse bitmiş olma imkanı yok zaten.
            {
                //Maksat daha optimize bir kod yazmak

                return false;
            }

            List<int> blocksRows = lastStabilizedShape.GetBlocksRows();

            foreach(int row in blocksRows)
            {
                if (row <= 1)
                {
                    return true;
                }
            }

            return false;
        }
        private static Shape InitializeRandomShape()
        {
            Random diceD7 = new Random();
            int randomShapeNum = diceD7.Next(1, 8); // NOT: Max 8 olacak
            
            switch (randomShapeNum)
            {
                case 1:
                    {
                        //Initialize ShapeT

                        ShapeT shapeT = new ShapeT(5, 0);
                        return shapeT;
                    }
                case 2:
                    {
                        //Initialize ShapeLL
                        ShapeLL shapeLL = new ShapeLL(5, 1);
                        return shapeLL;


                    }
                case 3:
                    {
                        //Initialize ShapeLR
                        ShapeLR shapeLR = new ShapeLR(5, 1);
                        return shapeLR;
                    }
                case 4:
                    {
                        //Initialize ShapeSq
                        ShapeSq shapeSq = new ShapeSq(5, 0);
                        return shapeSq;
                    }
                case 5:
                    {
                        //Initialize ShapeLine
                        ShapeLine shapeLine = new ShapeLine(5, 0);
                        return shapeLine;
                    }
                case 6:
                    {
                        //Initialize ShapeStaireR
                        ShapeStaireR shapeStaireR = new ShapeStaireR(5, 1);
                        return shapeStaireR;
                    }
                case 7:
                    {
                        //Initialize ShapeStaireL
                        ShapeStaireL shapeStaireL = new ShapeStaireL(5, 1);
                        return shapeStaireL;
                    }
                default:
                    {
                        return null;
                    }

            }

            
        }

        private static bool CheckAndRemoveRows(Shape lastStabilizedShape)
        {
            //Son stabilize edilen şeklin blocklarının satırlarına bakıp gerekli satırları silen method
            //Herhangi bir satır silinirse true return edecek, hiç silinmezse false

            if (lastStabilizedShape == null)
            {
                return false;
            }

            //1. Shape in bloklarının bulunduğu satırlara bak. Ve ful doluysa silip silinen satırlar listesine kaydet
            List<int> shapesRows = lastStabilizedShape.GetBlocksRows(); //Shapein bütün rowları kaydedildi.

            List<int> rowsToBeRemoved = DetermineRowsToBeRemoved(shapesRows); //Silinecek rowlar belirlendi.

            if (rowsToBeRemoved.Count > 0) //Silinecek rowlar da herhangi bir elemean varsa silme methodunu aktifleştiririz.
            {
                //Aksi halde boşuna işlem yapacak azda olsa.

                RemoveRows(rowsToBeRemoved); //Silinecek rowlar silindi. Matrice'den ve Canvas'dan

                //2.Silinen en alttaki satırı sapta
                int lowestRemovedRow = GetLowestRemovedRow(rowsToBeRemoved);

                for(int row = lowestRemovedRow - 1; row >= 0; row--)
                {
                    if (rowsToBeRemoved.Contains(row)) //Silinenler arasında varsa bir üste geç
                    {
                        continue;
                    }

                    if (!MatriceManager.CheckRowToBeTransferred(row)) //Silinenler arasında da olmayıp tamamen boşsa bunun üstüde tamamen boştur
                    {
                        break;
                    }

                    int shiftAmount = CalculateShiftAmountOfTheRow(row, rowsToBeRemoved); //Kaç satır aşağı kaydırılıacağını saptadık.

                    ShiftTheRowDown(row, shiftAmount);
                }
            }
            
            


            if (rowsToBeRemoved.Count > 0) //Silinecek rowlar da herhangi bir elemean varsa en azından 1 tane row silinmiştir.
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static List<int> DetermineRowsToBeRemoved(List<int> shapesRows)
        {
            //Shape in bulunduğu rowları kontrol edip silinicek rowları return eden method(yani tamamen dolu olanları).
            
            List<int> rowsToBeRemoved = new List<int> { };
            
            foreach (int row in shapesRows)
            {
                if (MatriceManager.CheckRowToBeRemoved(row) && !rowsToBeRemoved.Contains(row)) //Eklenen bir daha eklenmesin, sıkıntı olabilir.
                {
                    rowsToBeRemoved.Add(row);
                }
            }

            return rowsToBeRemoved;
        }
        private static void RemoveRows(List<int> rowsToBeRemoved)
        {
            //Parametredeki row ları matrice ve canvasdan silen method.

            foreach(int row in rowsToBeRemoved)
            {
                for (int x = 0; x <= 9; x++)
                {
                    CanvasManager.RemoveBlock(x, row);
                    MatriceManager.RemoveCoordinate(x, row);
                } 
            }
        }
        private static int GetLowestRemovedRow(List<int> rowsToBeRemoved)
        {
            //Silinen en alttaki satırı return eder.

            int lowestRemovedRow = 0; 

            foreach(int row in rowsToBeRemoved)
            {
                if (row > lowestRemovedRow)
                {
                    lowestRemovedRow = row;
                }
            }

            return lowestRemovedRow;
        }
        private static int CalculateShiftAmountOfTheRow(int row, List<int> rowsToBeRemoved)
        {
            //Belirli bir satırın(row) silinen satırlar listesi üstünden ne kadar aşağı kaydırılıcağını hesaplayan method.

            int shiftAmount = 0;

            foreach (int removedRow in rowsToBeRemoved)
            {
                if (removedRow > row) //Eğer silinen row baktığımızın altındaysa 1 tane aşağı kaydırılmalı, shiftAmount'u 1 artırırız
                {
                    shiftAmount++;
                }
            }
            return shiftAmount;
        }
        private static void ShiftTheRowDown(int row, int shiftAmount)
        {
            //Belirli bir row'u belirli miktarda aşağı kaydıran method.

            for(int column = 0; column <= 9; column++) //Bir satırdaki tüm blokları iterate ediyoruz
            {
                if (MatriceManager.CheckCoordinate(column, row))
                {
                    MatriceManager.RemoveCoordinate(column, row);
                    CanvasManager.RemoveBlock(column, row);
                    MatriceManager.FillCoordinate(column, row + shiftAmount);
                    CanvasManager.AddBlock(column, row + shiftAmount);
                }
            }

        }
        private static void InputGetter()
        {
            while (true)
            {
                var input = Console.ReadKey(true).Key;

                if (getInputIsActive)
                {
                    switch (input)
                    {
                        case ConsoleKey.RightArrow:
                            {
                                currentShape.ShiftRight();

                                break;
                            }
                        case ConsoleKey.LeftArrow:
                            {
                                currentShape.ShiftLeft();

                                break;
                            }
                        case ConsoleKey.DownArrow:
                            {
                                currentShape.FallOnce();

                                break;
                            }
                        case ConsoleKey.Spacebar:
                            {
                                currentShape.Rotate();

                                break;
                            }
                        default:
                            {


                                break;
                            }

                    }

                }

            }

        }
    }
}
