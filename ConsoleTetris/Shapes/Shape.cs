using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTetris
{
    abstract class Shape
    {
        public Coordinate shapeCoordinate;

        protected Block originBlock;

        protected Block[,] currentShapeMatrice;

        protected  Block[] blocksRotateQueue;

        protected Queue<Block[,]> rotatingMatricesQueue=new Queue<Block[,]> { };

        protected Shape(int _x, int _y)
        {
            originBlock = new Block(_x, _y);
            shapeCoordinate = originBlock.coordinate;
        } 

        public List<int> GetBlocksRows()
        {
            /*Row ları almanın amacı şu: Shape stabilize olduğunda bu rowlar kontrol edilecek silinicekse bu rowlardan silinicek.
             */
            List<int> rows = new List<int> { };

            rows.Add(originBlock.coordinate.Y);

            foreach(Block block in blocksRotateQueue)
            {
                rows.Add(block.coordinate.Y);
            }

            return rows;

        }

        public virtual void Rotate()
        {

            List<Coordinate> targetRotationCoordinates = GetTargetRotationCoordinates();

            if (GetShapeIndexOutOfRangeFromNegativeYAxis(targetRotationCoordinates)) //Eğer dışarda kordinat varsa rotate iptal
            {
                return;
            }
             
            Coordinate outerCoordinate = null; //En dışta kalan

            foreach (Coordinate coordinate in targetRotationCoordinates)
            {
                if (coordinate.X < 0)
                {
                    if (outerCoordinate != null)
                    {
                        if (coordinate.X < outerCoordinate.X)
                        {
                            outerCoordinate = coordinate;
                        }
                    }
                    else
                    {
                        outerCoordinate = coordinate;
                    }
                }
                else if (coordinate.X > 9)
                {
                    if (outerCoordinate != null)
                    {
                        if (coordinate.X > outerCoordinate.X)
                        {
                            outerCoordinate = coordinate;
                        }
                    }
                    else
                    {
                        outerCoordinate = coordinate;
                    }

                }
            }

            /*outerCoordinate null ise canvas dışına bir kayma yok demektir.
             * Bu durumda kordinatları kaydırmadan oralar boş mu diye check ederiz.
             * Yok kordinatlar dışardaysa onları kaydrıp check ederiz. Ve eğer dolu bir yere denk gelmezse kaydırma (ShiftRigt/Left) yapılır.
             * En sonda da eğer rotasyon yapılabili yorrsa Rotat işlemi yapılır.
             */

            //__COORDİNATE SHİFTİNG

            int shiftingAmount = 0;
            bool shiftingSide = true; //right:true | left:false

            if (outerCoordinate != null) 
            {
                //Outer coordinate e göre kordinatlar kaydırılır(origin dahil).
                //Ve o kordinatlar boşmu diye check edilir eğer hepsi boşsa shape kaç kere olcaksa gerçekten kaydırılır.

                Coordinate originCoordinateCopy = new Coordinate(originBlock.coordinate.X, originBlock.coordinate.Y); //Direk originCoordinate alırsam
                //Onun üstünde oynama yapılacak ve ortalık karşıp çorba olacak. Kopyası üstündn yaparsa sorun olmaz. Listedeki diğerlerde kopya zaten.
                //GetTargetRotationCoordinates methodundan bakabilirsin.

                targetRotationCoordinates.Add(originCoordinateCopy); //Bunu da check etmeliyiz, bu da kaydırılacak


                int outerX = outerCoordinate.X;

                if (outerX > 9)
                {
                    shiftingAmount = 9 - outerX;
                    shiftingSide = false;
                }
                else if (outerX < 0)
                {
                    shiftingAmount = outerX;
                    shiftingSide = true;
                }

                foreach (Coordinate coordinate in targetRotationCoordinates) //shiftingAmount kadar kaydırdık tüm kordinatları
                {
                    if (shiftingSide)
                    {
                        coordinate.X -= shiftingAmount;
                    }
                    else
                    {
                        coordinate.X += shiftingAmount;
                    }
                }
            }

            if (MatriceManager.CheckCoordinates(targetRotationCoordinates))  //Bu değer true ise bir yerde block var çakışıyor demektir shifting yapınca
            {
                return; //Rotating işlemi iptal
            }

            if (shiftingAmount != 0) //Shifting yapılması gerekiyor
            {
                if (shiftingSide == true) //
                {
                    for(int shifting = 0; shifting < Math.Abs(shiftingAmount); shifting++)
                    {
                        ShiftRight();
                    }
                }
                else
                {
                    for (int shifting = 0; shifting < Math.Abs(shiftingAmount); shifting++)
                    {
                        ShiftLeft();
                    }
                }
            }

            /*__BURAYA KADAR ROTATE YAPILABİLİR Mİ KISMI, BURDAN SONRAKİ KISIM ROTATE YAPAN KISIM___*/

            RotateMatrice();

            RotateBlocksAtCanvas();

        }

        public virtual void ShiftRight()
        {
            //Şekli bir birim sağa kaydıran method

            if (!CheckCanShiftRight()) //Eğer sağa kayamayacaksa methodu sonlandır.
            {
                return;
            }

            //Method buraya kadar gelebildiyse Shape sağa gidebilir demektir.

            int outerArrayCount = currentShapeMatrice.GetLength(0); //Dış Array deki eleman sayısı
            int innerArrayCount = currentShapeMatrice.GetLength(1); //İç Array deki eleman sıyısı

            for (int outerIndex = outerArrayCount - 1; outerIndex >= 0; outerIndex--)
            {
                for (int innerIndex = innerArrayCount - 1; innerIndex >= 0; innerIndex--)
                {
                    Block currentBlock = currentShapeMatrice[outerIndex, innerIndex];

                    if (currentBlock != null)
                    {
                        currentBlock.ResetCanvasPlace(currentBlock.coordinate.X + 1, currentBlock.coordinate.Y);

                    }
                }
            }

        }

        public virtual void ShiftLeft()
        {
            //Şekli bir birim sağa kaydıran method

            if (!CheckCanShiftLeft()) //Eğer sağa kayamayacaksa methodu sonlandır.
            {
                return;
            }

            //Method buraya kadar gelebildiyse Shape sağa gidebilir demektir.

            int outerArrayCount = currentShapeMatrice.GetLength(0); //Dış Array deki eleman sayısı
            int innerArrayCount = currentShapeMatrice.GetLength(1); //İç Array deki eleman sıyısı

            for (int outerIndex = outerArrayCount - 1; outerIndex >= 0; outerIndex--)
            {
                for (int innerIndex = 0; innerIndex < innerArrayCount; innerIndex++)
                {
                    Block currentBlock = currentShapeMatrice[outerIndex, innerIndex];

                    if (currentBlock != null)
                    {
                        currentBlock.ResetCanvasPlace(currentBlock.coordinate.X - 1, currentBlock.coordinate.Y);

                    }
                }
            }
        }

        public virtual bool FallOnce()
        {
            //Şekli bir birim aşağı kaydıran method
            //Shape in stabilize olup olmadığını return ediyor: Stabilized=True Unstabilezed=False
            

            if (!CheckCanFall()) //Eğer aşağı inemeycekse methodu sonlandır.
            {
                //Aşağı inemiyorsa Check ederken stabilize edilmişitir. True return ederiz.
                return true;
            }

            //Method buraya kadar gelebildiyse Shape aşağı gidebilir demektir.
           
            int outerArrayCount = currentShapeMatrice.GetLength(0); //Dış Array deki eleman sayısı
            int innerArrayCount = currentShapeMatrice.GetLength(1); //İç Array deki eleman sıyısı

            for (int outerIndex = outerArrayCount - 1; outerIndex >= 0; outerIndex--)
            {
                for (int innerIndex = 0; innerIndex < innerArrayCount; innerIndex++)
                {
                    Block currentBlock = currentShapeMatrice[outerIndex, innerIndex];

                    if (currentBlock != null)
                    {
                        currentBlock.ResetCanvasPlace(currentBlock.coordinate.X, currentBlock.coordinate.Y + 1);

                    }
                }
            }
            
            //Aşğaı kaydırılmışsa daha stabilize olmamıştır false return edilir.
            return false;
        }

        private bool CheckCanFall()
        {
            int outerArrayCount = currentShapeMatrice.GetLength(0); //Dış Array deki eleman sayısı
            int innerArrayCount = currentShapeMatrice.GetLength(1); //İç Array deki eleman sıyısı

            for (int innerIndex = 0; innerIndex < innerArrayCount; innerIndex++)
            {

                for (int outerIndex = outerArrayCount-1; outerIndex >= 0; outerIndex--)
                {
                    Block currentBlock = currentShapeMatrice[outerIndex, innerIndex];

                    if (currentBlock != null)
                    {
                        //Her satırda block denk gelince altının boş olup olmadığına bakıyoruz veya canvasın hala içinde olup olmayacağına

                        if (currentBlock.coordinate.Y == 19 || MatriceManager.CheckCoordinate(currentBlock.coordinate.X,currentBlock.coordinate.Y + 1))
                        {
                            //Eğer herhangi bir tanesinin altı bile doluysa return ederiz çünkü aşağı hareket ettiremeyiz.
                            
                            StabilizeShapeAtMatrice();

                            return false;
                        }

                        break; //Herhangi bir sütunda alttan üste bakıyoruz. Ve ilk denk gelene bakıyoruz. Onun üstündekine bakmak saçma. Algoritmayı bozar.
                    }
                }
            }

            return true;
        }

        private bool CheckCanShiftRight()
        {
            int outerArrayCount = currentShapeMatrice.GetLength(0); //Dış Array deki eleman sayısı
            int innerArrayCount = currentShapeMatrice.GetLength(1); //İç Array deki eleman sıyısı

            for (int outerIndex = outerArrayCount - 1; outerIndex >= 0; outerIndex--)
            {

                for (int innerIndex = innerArrayCount - 1; innerIndex >= 0; innerIndex--)
                {
                    Block currentBlock = currentShapeMatrice[outerIndex, innerIndex];

                    if (currentBlock != null)
                    {
                        //Her satırda block denk gelince altının boş olup olmadığına bakıyoruz veya canvasın hala içinde olup olmayacağına

                        if (currentBlock.coordinate.X == 9 || MatriceManager.CheckCoordinate(currentBlock.coordinate.X + 1, currentBlock.coordinate.Y))
                        {
                            //Eğer herhangi bir tanesinin altı bile doluysa return ederiz çünkü aşağı hareket ettiremeyiz.

                            return false;
                        }

                        break; //Herhangi bir sütunda alttan üste bakıyoruz. Ve ilk denk gelene bakıyoruz. Onun üstündekine bakmak saçma. Algoritmayı bozar.
                    }
                }
            }

            return true;
        }

        private bool CheckCanShiftLeft()
        {
            int outerArrayCount = currentShapeMatrice.GetLength(0); //Dış Array deki eleman sayısı
            int innerArrayCount = currentShapeMatrice.GetLength(1); //İç Array deki eleman sıyısı

            for (int outerIndex = outerArrayCount - 1; outerIndex >= 0; outerIndex--)
            {

                for (int innerIndex = 0; innerIndex < innerArrayCount; innerIndex++)
                {
                    Block currentBlock = currentShapeMatrice[outerIndex, innerIndex];

                    if (currentBlock != null)
                    {
                        //Her satırda block denk gelince altının boş olup olmadığına bakıyoruz veya canvasın hala içinde olup olmayacağına

                        if (currentBlock.coordinate.X == 0 || MatriceManager.CheckCoordinate(currentBlock.coordinate.X - 1, currentBlock.coordinate.Y))
                        {
                            //Eğer herhangi bir tanesinin altı bile doluysa return ederiz çünkü aşağı hareket ettiremeyiz.

                            return false;
                        }

                        break; //Herhangi bir sütunda alttan üste bakıyoruz. Ve ilk denk gelene bakıyoruz. Onun üstündekine bakmak saçma. Algoritmayı bozar.
                    }
                }
            }

            return true;
        }

        private void StabilizeShapeAtMatrice()
        {
            //Shape hareketi durup sabitlenince matrisde blockların kordinatlarına karşılık gelen yerleri true yapıyor ki sonrakileri engelleyebilsin. 

            int outerArrayCount = currentShapeMatrice.GetLength(0); //Dış Array deki eleman sayısı
            int innerArrayCount = currentShapeMatrice.GetLength(1); //İç Array deki eleman sıyısı

            for (int outerIndex = outerArrayCount - 1; outerIndex >= 0; outerIndex--)
            {
                for (int innerIndex = 0; innerIndex < innerArrayCount; innerIndex++)
                {
                    Block currentBlock = currentShapeMatrice[outerIndex, innerIndex];

                    if (currentBlock != null)
                    {
                        MatriceManager.FillCoordinate(currentBlock.coordinate);

                    }
                }
            }
        }

        private void RotateMatrice()
        {

            rotatingMatricesQueue.Enqueue(currentShapeMatrice); //Önce current ı kuyruğa aldık

            currentShapeMatrice = rotatingMatricesQueue.Dequeue(); //Sonra kuyruğun başından yeni matrice i aldık ve current yaptık
             
            //Ve matrice i de rotate etmiş olduk.

        }

        private void RotateBlocksAtCanvas()
        {
            //Rotate işleminde blockların canvasdaki yerini değiştiren method.

            Coordinate originCoordinate = originBlock.coordinate;

            foreach (Block rotatingBlock in blocksRotateQueue)
            {
                Coordinate rotatingCoordinate = rotatingBlock.coordinate;

                int targetX = originCoordinate.X - (rotatingCoordinate.Y - originCoordinate.Y);
                int targetY = originCoordinate.Y + (rotatingCoordinate.X - originCoordinate.X);

                rotatingBlock.ResetCanvasPlace(targetX, targetY);
            }
        }

        protected void DrawRotateMatrices()
        {
            //Shape in diğer rotating matrice lerini çizip dönüş sırasına göre rotatingQueue ya koyan method. Bunu shape'i init ederken invoke edicez.

            Block[,] willRotate = currentShapeMatrice; //Döndürülecek olanı buna koyup bunu üstünden değiştiricez.

             

            for(int i = 0; i < 3; i++)
            {
                Block[,] rotatedMatrice = RotateClokwiseMatrice(willRotate);

                rotatingMatricesQueue.Enqueue(rotatedMatrice);

                willRotate = rotatedMatrice; //Bir dahaki rotate buna göre yapılacak
            }

            // rotatingMatricesQueue.Enqueue(currentShapeMatrice); //En sonda current olmalı.

        }

        private Block[,] RotateClokwiseMatrice(Block[,] shapeMatrice)
        {
            //Bir block matrisini alıp saat yönünde 90 derece çevirip çevirdiğini return eden method. ___DrawRotateMatrices() INNER METHOD____

            int rotatedRowCount = shapeMatrice.GetLength(1); //Eskinin column sayısı yeninin row miktarı oluyor
            int rotatedColumnCount = shapeMatrice.GetLength(0); //Eskinin row sayısı yeninin column miktarı oluyor.
            
            Block[,] rotatedMatrice = new Block[rotatedRowCount, rotatedColumnCount];

            for(int row = 0; row < rotatedColumnCount; row++)
            {
                for (int column = 0; column < rotatedRowCount; column++)
                {
                    int rotatedRow = column; //Yeni row = eski column
                    int rotatedColumn = (rotatedColumnCount - row) - 1; // Yeni column = (Yeni column sayısı - eski row) - 1

                    rotatedMatrice[rotatedRow, rotatedColumn] = shapeMatrice[row, column]; 
                    
                    /*Eski matrice deki block ları yenide karşılık gelen yerine aktararak rotate işlemini tamamladık.
                     */
                }
            }

            return rotatedMatrice;
        }

        private bool GetShapeIndexOutOfRangeFromNegativeYAxis(List<Coordinate> targetRotationCoordinates)
        {
            /*Shape ilk initiate edildiğinde direk rotate edilmeye çalışılınırsa coordinate ın ysi 0 dan küçük hale gelebiliyor
                 * Bizde önceden böyle bişi olup olmadığını check ediyoruz. Eğer varsa true return ediyoruz
                 */
            foreach (Coordinate coordinate in targetRotationCoordinates)
            {
                if (coordinate.Y < 0)
                {
                    return true;
                }

            }

            return false;
        }
        private List<Coordinate> GetTargetRotationCoordinates()
        {
            /*Rotation kordinatlarını hesaplayıp bir listede return eden method.
             *Bunu Rotasyonu yapıp yapamayacağını hesaplar/ayarlarken kullanıyoruz. 
             *Canvasda oralar boşmu veya kaydırma yapılacakmı faln diye.
             */

            List<Coordinate> targetRotationCoordinates = new List<Coordinate> { };

            Coordinate originCoordinate = originBlock.coordinate;

            foreach (Block rotatingBlock in blocksRotateQueue)
            {
                Coordinate rotatingCoordinate = rotatingBlock.coordinate;

                int targetX = originCoordinate.X - (rotatingCoordinate.Y - originCoordinate.Y);
                int targetY = originCoordinate.Y + (rotatingCoordinate.X - originCoordinate.X);

                Coordinate targetCoordinate = new Coordinate(targetX, targetY);

                targetRotationCoordinates.Add(targetCoordinate);
            }

            return targetRotationCoordinates;
        }
    }
}
