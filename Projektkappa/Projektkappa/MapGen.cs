using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projektkappa
{
    class MapGen
    {
        private int[,] map;
        int temp;

        private int[,] endRoom = new int[,] {
            {8,4,4,4,4,4,10 },
            {3,2,2,2,2,2,3 },
            {3,1,1,1,1,1,3 },
            {3,1,1,1,1,1,3 },
            {3,1,8,4,10,1,3 },
            {3,1,28,30,29,1,3},
            {3,1,1,1,1,1,3 },
            {3,1,1,1,1,1,3 },
            {3,1,1,1,1,1,3 },
            {3,1,1,1,1,1,3 },
            {3,1,1,1,1,1,3 },
            {3,1,1,1,1,1,3 },
            {3,1,1,1,1,1,3 },
            {3,1,1,1,1,1,3 },
            {3,1,1,1,1,1,3 },
            {7,4,4,4,4,4,9 },
            {2,2,2,2,2,2,2 },};

        private int[,] rowRoom = new int[,]
            {
                {8,4,4,4,4,4,4,4,4,4,4,4,4,4,10 },
                {3,2,2,2,2,2,2,2,2,2,2,2,2,2,3 },
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3 },
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3 },
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3 },
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3 },
                {7,4,4,4,4,4,4,4,4,4,4,4,4,4,9 },
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2 },
            };

        private int[,] colRoom = new int[,]
            {
                {8,4,4,4,4,4,10 },
                {3,2,2,2,2,2,3 },
                {3,1,1,1,1,1,3 },
                {3,1,1,1,1,1,3 },
                {3,1,1,1,1,1,3 },
                {3,1,1,1,1,1,3 },
                {3,1,1,1,1,1,3 },
                {3,1,1,1,1,1,3 },
                {3,1,1,1,1,1,3 },
                {3,1,1,1,1,1,3 },
                {3,1,1,1,1,1,3 },
                {3,1,1,1,1,1,3 },
                {3,1,1,1,1,1,3 },
                {3,1,1,1,1,1,3 },
                {3,1,1,1,1,1,3 },
                {7,4,4,4,4,4,9 },
                {2,2,2,2,2,2,2 },
            };

        private int[,] horPass = new int[,]
        {
            { 7,4,9 },
            { 2,2,2 },
            { 1,1,1 },
            { 8,4,10 },
            { 3,2,3 },
        };


        private int[,] verPass = new int[,]
        {
            { 10,1,8 },
            { 3,1,3 },
            { 3,1,3 },
            { 9,1,7 },
            { 2,1,2 },
        };

        int[,] horPassPositions;
        int[,] verPassPositions;
        List<int> Rooms = new List<int>();

        Random rand;

        int endSizeX, endSizeY, colSizeX, colSizeY, rowSizeX, rowSizeY, horSizeX, horSizeY, verSizeX, verSizeY, startX, startY, positionX, positionY, i = 0, j = 0;
        int kierunek; // 1 - dol, 2 - prawo, 3 - gora, 4 - lewo
        int countrow, countcol; // zeby nie bylo za duzo pokoi w jednej linii
        int liczbaPokoi = 8;
        public int playerPositionX;
        public int playerPositionY;
        int count = 0;
        bool beenCorrected = false;
        // Generowanie mapy 2D
        public int[,] Gen()
        {
            map = new int[((liczbaPokoi + 1) * 3) * 20, ((liczbaPokoi + 1) * 3) * 20];
            horPassPositions = new int[liczbaPokoi+1, 3];
            verPassPositions = new int[liczbaPokoi+1, 3];

            for (int x = 0; x < ((liczbaPokoi + 1) * 3) * 20; x++)
                for (int y = 0; y < ((liczbaPokoi + 1) * 3) * 20; y++)
                    map[y, x] = 0;

            endSizeX = endRoom.GetLength(1);
            endSizeY = endRoom.GetLength(0);

            colSizeX = colRoom.GetLength(1);
            colSizeY = colRoom.GetLength(0);

            rowSizeX = rowRoom.GetLength(1);
            rowSizeY = rowRoom.GetLength(0);

            horSizeX = horPass.GetLength(1);
            horSizeY = horPass.GetLength(0);

            verSizeX = verPass.GetLength(1);
            verSizeY = verPass.GetLength(0);

            rand = new Random();

            for (int i = 0; i < liczbaPokoi; i++)
                Rooms.Add(rand.Next(1,3));

            Rooms.Add(3);

            startX = 0;
            startY = 0;
            // USTAWIENIE POZYCJI PIERWSZEGO POKOJU NA SRODEK MAPY
            positionX = (((liczbaPokoi + 1) * 3) * 20) /2;
            positionY = (((liczbaPokoi + 1) * 3) * 20) /2;
            countcol = 0;
            countrow = 0;
            // USTAWIENIE POZYCJI POCZATKOWEJ GRACZA
            playerPositionX = (positionX + 2) * 32;
            playerPositionY = (positionY + 3) * 32;

            for (int x = 0; x < Rooms.Count; x++)
            {
                // POKOJ 1
                if (Rooms[x] == 1)
                {
                    for (startX = 0; startX < rowSizeX; startX++)
                        for (startY = 0; startY < rowSizeY; startY++)
                        {
                            temp = rowRoom[startY, startX];
                            map[startY + positionY, startX + positionX] = temp;
                        }
                    kierunek = rand.Next(1,5);
                    // DOWN
                    if ((((kierunek == 1 && countcol != 2) || countrow == 2)))
                    {
                        if (Rooms[x + 1] == 2)
                        {
                            positionY = solveMapDown(map, positionX, positionY, colSizeX, colSizeY);
                            verPassPositions[i, 0] = positionX + 1;
                            verPassPositions[i, 1] = positionY - 3;
                            verPassPositions[i, 2] = kierunek;
                        }
                        else if (count != Rooms.Count - 2)
                        {
                            positionY = solveMapDown(map, positionX, positionY, rowSizeX, rowSizeY);
                            verPassPositions[i, 0] = positionX + 1;
                            verPassPositions[i, 1] = positionY - 3;
                            verPassPositions[i, 2] = kierunek;
                        }
                        i++;
                        countcol++;
                        if (countrow >= 2)
                            countrow = 0;
                    }
                    // RIGHT
                    else if ((((kierunek == 2 && countrow != 2) || countcol == 2)))
                    {
                        if (Rooms[x + 1] == 2)
                        {
                            positionX = solveMapRight(map, positionX, positionY, colSizeX, colSizeY);
                            horPassPositions[j, 0] = positionX - 2;
                            horPassPositions[j, 1] = positionY + 1;
                            horPassPositions[j, 2] = kierunek;
                        }
                        else if (count != Rooms.Count - 2)
                        {
                            positionX = solveMapRight(map, positionX, positionY, rowSizeX, rowSizeY);
                            horPassPositions[j, 0] = positionX - 2;
                            horPassPositions[j, 1] = positionY + 1;
                            horPassPositions[j, 2] = kierunek;
                        }
                        
                        j++;
                        countrow++;
                        if (countcol >= 2)
                            countcol = 0;
                    }
                    // UP
                    else if ((((kierunek == 3 && countcol != 2) || countrow == 2)))
                    {
                        if (Rooms[x + 1] == 2)
                        {
                            positionY = solveMapUp(map, positionX, positionY, colSizeX, colSizeY);
                            verPassPositions[i, 0] = positionX + 1;
                            verPassPositions[i, 1] = positionY + colSizeY - 2;
                            verPassPositions[i, 2] = kierunek;
                        }
                        else if (count != Rooms.Count - 2)
                        {
                            positionY = solveMapUp(map, positionX, positionY, rowSizeX, rowSizeY);
                            verPassPositions[i, 0] = positionX + 1;
                            verPassPositions[i, 1] = positionY + rowSizeY - 2;
                            verPassPositions[i, 2] = kierunek;
                        }

                        i++;
                        countcol++;
                        if (countrow >= 2)
                            countrow = 0;
                    }
                    // LEFT
                    else if ((((kierunek == 4 && countrow != 2) || countcol == 2)))
                    {
                        if (Rooms[x + 1] == 2)
                        {
                            positionX = solveMapLeft(map, positionX, positionY, colSizeX, colSizeY);
                            horPassPositions[j, 0] = positionX + colSizeX - 1;
                            horPassPositions[j, 1] = positionY + 1;
                            horPassPositions[j, 2] = kierunek;
                        }
                        else if (count != Rooms.Count - 2)
                        {
                            positionX = solveMapLeft(map, positionX, positionY, rowSizeX, rowSizeY);
                            horPassPositions[j, 0] = positionX + rowSizeX - 1;
                            horPassPositions[j, 1] = positionY + 1;
                            horPassPositions[j, 2] = kierunek;
                        }
                        j++;
                        countrow++;
                        if (countcol >= 2)
                            countcol = 0;
                    }
                    count++;
                }
                // POKOJ 2
                if(Rooms[x] == 2)
                {
                    for (startX = 0; startX < colSizeX; startX++)
                        for (startY = 0; startY < colSizeY; startY++)
                        {
                            temp = colRoom[startY, startX];
                            map[startY + positionY, startX + positionX] = temp;
                        }
                    kierunek = rand.Next(1,5);
                    // DOWN
                    if ((((kierunek == 1 && countcol != 2) || countrow == 2)))
                    {
                        if (Rooms[x + 1] == 1)
                        {
                            positionY = solveMapDown(map, positionX, positionY, rowSizeX, rowSizeY);
                            verPassPositions[i, 0] = positionX + 1;
                            verPassPositions[i, 1] = positionY - 3;
                            verPassPositions[i, 2] = kierunek;
                        }
                        else if (count != Rooms.Count - 2)
                        {
                            positionY = solveMapDown(map, positionX, positionY, colSizeX, colSizeY);
                            verPassPositions[i, 0] = positionX + 1;
                            verPassPositions[i, 1] = positionY - 3;
                            verPassPositions[i, 2] = kierunek;
                        }
                        
                        i++;
                        countcol++;
                        if (countrow >= 2)
                            countrow = 0;
                    }
                    // RIGHT
                    else if ((((kierunek == 2 && countrow != 2) || countcol == 2)))
                    {
                        if (Rooms[x + 1] == 1)
                        {
                            positionX = solveMapRight(map, positionX, positionY, rowSizeX, rowSizeY);
                            horPassPositions[j, 0] = positionX - 2;
                            horPassPositions[j, 1] = positionY + 1;
                            horPassPositions[j, 2] = kierunek;
                        }
                        else if (count != Rooms.Count - 2)
                        {
                            positionX = solveMapRight(map, positionX, positionY, colSizeX, colSizeY);
                            horPassPositions[j, 0] = positionX - 2;
                            horPassPositions[j, 1] = positionY + 1;
                            horPassPositions[j, 2] = kierunek;
                        }

                        j++;
                        countrow++;
                        if (countcol >= 2)
                            countcol = 0;
                    }
                    // UP
                    else if ((((kierunek == 3 && countcol != 2) || countrow == 2)))
                    {
                        if (Rooms[x + 1] == 1)
                        {
                            positionY = solveMapUp(map, positionX, positionY, rowSizeX, rowSizeY);
                            verPassPositions[i, 0] = positionX + 1;
                            verPassPositions[i, 1] = positionY + rowSizeY - 2;
                            verPassPositions[i, 2] = kierunek;
                        }
                        else if (count != Rooms.Count - 2)
                        {
                            positionY = solveMapUp(map, positionX, positionY, colSizeX, colSizeY);
                            verPassPositions[i, 0] = positionX + 1;
                            verPassPositions[i, 1] = positionY + colSizeY - 2;
                            verPassPositions[i, 2] = kierunek;
                        }

                        i++;
                        countcol++;
                        if (countrow >= 2)
                            countrow = 0;
                    }
                    // LEFT
                    else if ((((kierunek == 4 && countrow != 2) || countcol == 2)))
                    {
                        if (Rooms[x + 1] == 1)
                        {
                            positionX = solveMapLeft(map, positionX, positionY, rowSizeX, rowSizeY);
                            horPassPositions[j, 0] = positionX + rowSizeX - 1;
                            horPassPositions[j, 1] = positionY + 1;
                            horPassPositions[j, 2] = kierunek;
                        }
                        else if (count != Rooms.Count - 2)
                        {
                            positionX = solveMapLeft(map, positionX, positionY, colSizeX, colSizeY);
                            horPassPositions[j, 0] = positionX + colSizeX - 1;
                            horPassPositions[j, 1] = positionY + 1;
                            horPassPositions[j, 2] = kierunek;
                        }
                        j++;
                        countrow++;
                        if (countcol >= 2)
                            countcol = 0;
                    }
                    count++;
                }

                // POKOJ 3
                if (Rooms[x] == 3)
                {
                    // DOWN
                    if (kierunek == 1)
                    {
                        positionY = solveMapDown(map, positionX, positionY, endSizeX, endSizeY);
                        verPassPositions[i, 0] = positionX + 1;
                        verPassPositions[i, 1] = positionY - 3;
                    }
                    // RIGHT
                    else if (kierunek == 2)
                    {
                        positionX = solveMapRight(map, positionX, positionY, endSizeX, endSizeY);
                        horPassPositions[j, 0] = positionX - 2;
                        horPassPositions[j, 1] = positionY + 1;
                    }
                    // UP
                    else if (kierunek == 3)
                    {
                        positionY = solveMapUp(map, positionX, positionY, endSizeX, endSizeY);
                        verPassPositions[i, 0] = positionX + 1;
                        verPassPositions[i, 1] = positionY + endSizeY - 2;
                    }
                    // LEFT
                    else if (kierunek == 4)
                    {
                        positionX = solveMapLeft(map, positionX, positionY, endSizeX, endSizeY);
                        horPassPositions[j, 0] = positionX + endSizeX - 1;
                        horPassPositions[j, 1] = positionY + 1;
                    }
                    // DODAWANIE POKOJU DO MAPY
                    if (Rooms[x] == 3)
                    {
                        for (startX = 0; startX < endSizeX; startX++)
                            for (startY = 0; startY < endSizeY; startY++)
                            {
                                temp = endRoom[startY, startX];
                                map[startY + positionY, startX + positionX] = temp;
                            }
                    }
                }
            }

            // POPRAWIANIE POZYCJI PRZEJSC
            for (int z = 0; z < liczbaPokoi + 1; z++)
            {
                if (horPassPositions[z, 1] != 0 || horPassPositions[z, 0] != 0 && horPassPositions[z, 3] == 2)
                    horPassPositions = solvePassageRight(map, horPassPositions, horSizeX, horSizeY, z);
                if (horPassPositions[z, 1] != 0 || horPassPositions[z, 0] != 0 && horPassPositions[z, 3] == 4)
                    horPassPositions = solvePassageLeft(map, horPassPositions, horSizeX, horSizeY, z);
            }
            for (int z = 0; z < liczbaPokoi + 1; z++)
            {
                if (verPassPositions[z, 1] != 0 || verPassPositions[z, 0] != 0 && verPassPositions[z, 3] == 1)
                    verPassPositions = solvePassageDown(map, verPassPositions, verSizeX, verSizeY, z);
                if (verPassPositions[z, 1] != 0 || verPassPositions[z, 0] != 0 && verPassPositions[z, 3] == 3)
                    verPassPositions = solvePassageUp(map, verPassPositions, verSizeX, verSizeY, z);
            }
            // GENEROWANIE PRZEJSC MIEDZY POKOJAMI
            for (int x = 0; x < liczbaPokoi + 1; x++)
            {
                if(horPassPositions[x, 1] != 0 || horPassPositions[x, 0] != 0)
                for (startX = 0; startX < horSizeX; startX++)
                    for (startY = 0; startY < horSizeY; startY++)
                    {
                        temp = horPass[startY, startX];
                        map[startY + horPassPositions[x, 1], startX + horPassPositions[x, 0]] = temp;
                    }
            }
            for (int x = 0; x < liczbaPokoi + 1; x++)
            {
                if(verPassPositions[x, 1] != 0 || verPassPositions[x, 0] != 0)
                for (startX = 0; startX < verSizeX; startX++)
                    for (startY = 0; startY < verSizeY; startY++)
                    {
                        temp = verPass[startY, startX];
                        map[startY + verPassPositions[x, 1], startX + verPassPositions[x, 0]] = temp;
                    }
            }
            return map;
        } 
        // POKOJE
        // USTAWIANIE POZYCJI W PRAWO
        public int solveMapRight(int[,] roomPositions, int positionX, int positionY, int RoomSizeX, int RoomSizeY)
        {
            for (int x = positionX - 1; x < positionX + RoomSizeX + 1; x++)
                for (int y = positionY; y < positionY + RoomSizeY; y++)
                {
                    if (roomPositions[y, x] != 0)
                    return solveMapRight(roomPositions, positionX + 1, positionY, RoomSizeX, RoomSizeY);
                }
            return positionX;
        }
        // USTAWIANIE POZYCJI W LEWO
        public int solveMapLeft(int[,] roomPositions, int positionX, int positionY, int RoomSizeX, int RoomSizeY)
        {
            for (int x = positionX - 1; x < positionX + RoomSizeX + 1; x++)
                for (int y = positionY; y < positionY + RoomSizeY; y++)
                {
                    if (roomPositions[y, x] != 0)
                        return solveMapLeft(roomPositions, positionX - 1, positionY, RoomSizeX, RoomSizeY);
                }
            return positionX;
        }
        // USTAWIANIE POZYCJI W GORE
        public int solveMapUp(int[,] roomPositions, int positionX, int positionY, int RoomSizeX, int RoomSizeY)
        {
            for (int x = positionX; x < positionX + RoomSizeX; x++)
                for (int y = positionY - 1; y < positionY + RoomSizeY + 1; y++)
                {
                    if (roomPositions[y, x] != 0)
                        return solveMapUp(roomPositions, positionX, positionY - 1, RoomSizeX, RoomSizeY);
                }
            return positionY;
        }
        // USTAWIANIE POZYCJI W DOL
        public int solveMapDown(int[,] roomPositions, int positionX, int positionY, int RoomSizeX, int RoomSizeY)
        {
            for (int x = positionX; x < positionX + RoomSizeX; x++)
                for (int y = positionY - 1; y < positionY + RoomSizeY + 1; y++)
                {
                    if (roomPositions[y, x] != 0)
                        return solveMapDown(roomPositions, positionX, positionY + 1, RoomSizeX, RoomSizeY);
                }
            return positionY;
        }
        // PRZEJSCIA MIEDZY POKOJAMI
        // USTAWIANIE PRZEJSCIA - KIERUNEK LEWO
        public int[,] solvePassageLeft(int[,] roomPositions, int[,] horPassPositions, int horSizeX, int horSizeY, int z)
        {
            for (int x = 0; x < horSizeX; x++)
                for (int y = 0; y < horSizeY; y++)
                {
                    if (roomPositions[y + horPassPositions[z, 1], x + horPassPositions[z, 0] + 2] == 0)
                    {
                        beenCorrected = true;
                        horPassPositions[z, 1] += 1;
                        return solvePassageLeft(roomPositions, horPassPositions, horSizeX, horSizeY, z);
                    }
                }
            if (beenCorrected == true)
            {
                horPassPositions[z, 1] += 1;
                beenCorrected = false;
            }
            return horPassPositions;
        }
        // USTAWIANIE PRZEJSCIA - KIERUNEK PRAWO
        public int[,] solvePassageRight(int[,] roomPositions, int[,] horPassPositions, int horSizeX, int horSizeY, int z)
        {
            for (int x = 0; x < horSizeX; x++)
                for (int y = 0; y < horSizeY; y++)
                {
                    if (roomPositions[y + horPassPositions[z, 1], x + horPassPositions[z, 0] - 2] == 0)
                    {
                        beenCorrected = true;
                        horPassPositions[z, 1] += 1;
                        return solvePassageRight(roomPositions, horPassPositions, horSizeX, horSizeY, z);
                    }
                }
            if (beenCorrected == true)
            {
                horPassPositions[z, 1] += 1;
                beenCorrected = false;
            }
            return horPassPositions;
        }
        // USTAWIANIE PRZEJSCIA - KIERUNEK GORA
        public int[,] solvePassageUp(int[,] roomPositions, int[,] verPassPositions, int verSizeX, int verSizeY, int z)
        {
            for (int x = 0; x < horSizeX; x++)
                for (int y = 0; y < horSizeY; y++)
                {
                    beenCorrected = true;
                    if (roomPositions[y + verPassPositions[z, 1] - 5, x + verPassPositions[z, 0]] == 0)
                    {
                        verPassPositions[z, 0] += 1;
                        return solvePassageUp(roomPositions, verPassPositions, horSizeX, horSizeY, z);
                    }
                }
            if (beenCorrected == true)
            {
                verPassPositions[z, 0] += 1;
                beenCorrected = false;
            }
            return verPassPositions;
        }
        // USTWIANIE PRZEJSCIA - KIERUNEK DOL
        public int[,] solvePassageDown(int[,] roomPositions, int[,] verPassPositions, int verSizeX, int verSizeY, int z)
        {
            for (int x = 0; x < horSizeX; x++)
                for (int y = 0; y < horSizeY; y++)
                {
                    if (roomPositions[y + verPassPositions[z, 1] + 5, x + verPassPositions[z, 0]] == 0)
                    {
                        beenCorrected = true;
                        verPassPositions[z, 0] += 1;
                        return solvePassageDown(roomPositions, verPassPositions, horSizeX, horSizeY, z);
                    }
                }
            if (beenCorrected == true)
            {
                verPassPositions[z, 0] += 1;
                beenCorrected = false;
            }
            return verPassPositions;
        }

    }
}
