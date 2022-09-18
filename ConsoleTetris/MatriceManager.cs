using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTetris
{
    static class MatriceManager
    {
        static bool[,] canvasMatrice = new bool[20, 10];
        /*Canvasın matrice referansı. Burda stabil olan dolu block ları gösteriyoruz.
        Inputların çalışıp çalışmayacağını bu matris üstünden kontrol ediyoruz.*/

        public static int matriceColumnCount = canvasMatrice.GetLength(0); //Dış Array deki eleman sayısı 20
        public static int matriceRowCount = canvasMatrice.GetLength(1); //İç Array deki eleman sıyısı 10

        public static bool CheckCoordinate(Coordinate coordinate)
        {
            return canvasMatrice[coordinate.Y, coordinate.X];
        }
        public static bool CheckCoordinate(int x, int y)
        {
            return canvasMatrice[y, x];
        }
        public static bool CheckCoordinates(List<Coordinate> coordinates)
        {
            //Listedeki kordinatlardan bir tanesi bile doluysa true return eden method.
            //Her yer boşsa false return eder.  __Rotation Shifting vs. yapılıp yapılamadığını kontrol etmek için kullanıyoruz.
            foreach(Coordinate coordinate in coordinates)
            {
                if (CheckCoordinate(coordinate)) //Bir tanesi bile doluysa true return edicez. 
                {
                    return true;
                }
            }

            return false;
        }
        public static bool CheckRowToBeTransferred(int row)
        {
            //Belirli bir satırı kontrol edip herhangi bir blok varsa true, tamamen boşsa false return eden method.

            for (int x = 0; x <= 9; x++)
            {
                if(CheckCoordinate(x, row))
                {
                    return true;
                }
            }

            return false;
        }
        public static bool CheckRowToBeRemoved(int row)
        {
            //Belirli bir satır tamamen doluysa true, aksi takdirde false return eden method.

            for (int x = 0; x <= 9; x++)
            {
                if (!CheckCoordinate(x, row))
                {
                    return false;
                }
            }

            return true;
        }

        public static void FillCoordinate(Coordinate coordinate)
        {
            canvasMatrice[coordinate.Y, coordinate.X] = true;
        }
        public static void FillCoordinate(int x, int y)
        {
            canvasMatrice[y, x] = true;
        }
        public static void RemoveCoordinate(Coordinate coordinate)
        {
            canvasMatrice[coordinate.Y, coordinate.X] = false;
        }
        public static void RemoveCoordinate(int x, int y)
        {
            canvasMatrice[y, x] = false;
        }
    }
}
