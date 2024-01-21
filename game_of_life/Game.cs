using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace game_of_life
{
    public static class Game
    {
        private static int Width = 50;
        private static int Height = 35;

        private static char[,] Map = new char[Width, Height];

        private static int Generation = 0;

        private static char Dead = ' ';
        private static char Alive = '■';

        private const int StepInterval = 500;

        private static void InitializeMap()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Map[x, y] = Dead;
                }
            }
        }

        private static void DisplayMap()
        {
            string MapString = "";

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    char curr = Map[x, y];

                    MapString += curr;

                    if (x == Width - 1) MapString += Environment.NewLine;
                }
            }

            Console.Write(MapString);
        }

        private static int GetNumberOfAliveNeighbors(int x, int y)
        {
            int retval = 0;

            int currX = (x - 1) >= 0 ? (x - 1) : x;
            int currY = (y - 1) >= 0 ? (y - 1) : y;

            int savedY = currY;

            while (currX <= (x + 1) && (x + 1) < Width)
            {
                while (currY <= (y + 1) && (y + 1) < Height)
                {
                    if (currX == x && currY == y)
                    {
                        currY++;
                        continue;
                    }

                    if (Map[currX, currY] == Alive) retval++;

                    currY++;
                }

                currY = savedY;
                currX++;
            }

            return retval;
        }

        private static void Revive(int x, int y)
        {
            Map[x, y] = Alive;
        }

        private static void Kill(int x, int y)
        {
            Map[x, y] = Dead;
        } 

        private static void PerformStep()
        {
            List<Vector2> toKill = new List<Vector2>();
            List<Vector2> toRevive = new List<Vector2>();

            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    char current = Map[x, y];

                    Vector2 currentVector = new Vector2(x, y);

                    int nAliveNeighbors = GetNumberOfAliveNeighbors(x, y);

                    if (current == Alive)
                    {
                        if (nAliveNeighbors < 2) toKill.Add(currentVector);
                        else if (nAliveNeighbors > 3) toKill.Add(currentVector);
                    }
                    else 
                    {
                        if (nAliveNeighbors == 3) toRevive.Add(currentVector);
                    }
                }
            }

            toKill.ForEach(vect => Kill((int)vect.X, (int)vect.Y));

            toRevive.ForEach(vect => Revive((int)vect.X, (int)vect.Y));
        }

        private static void RefreshMap()
        {
            Console.Clear();
            DisplayMap();
        }

        public static void StartGame()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            InitializeMap();

            Map[1, 2] = Alive;
            Map[2, 2] = Alive;
            Map[3, 2] = Alive;

            DisplayMap();

            while (true)
            {
                Thread.Sleep(StepInterval);

                PerformStep();
                RefreshMap();

                Generation++;
                Console.WriteLine("Generation: " + Generation);
            }
        }
    }
}
