
namespace Core
{
    public class GameOfLife
    {
        #region Fields

        private static readonly Random rnd = new(DateTime.Now.Millisecond);

        #endregion

        #region Methods

        public static void SeedGrid(ref bool[,] cells, int initialPopulation)
        {
            int width = cells.GetLength(0);
            int height = cells.GetLength(1);

            for (int i = 0; i < initialPopulation; i++)
            {
                int x = rnd.Next(0, width - 1);
                int y = rnd.Next(0, height - 1);

                cells[x, y] = true;
            }
        }

        public static int Generation(ref bool[,] cells, out bool hasChanged)
        {
            /*
            http://en.wikipedia.org/wiki/Conway's_Game_of_Life
            Based on current generation, calculate a new one.
            Any live cell with fewer than two live neighbours dies, as if caused by under-population. 
            Any live cell with two or three live neighbours lives on to the next generation. 
            Any live cell with more than three live neighbours dies, as if by overcrowding. 
            Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction. 
            */

            int population = 0;
            int width = cells.GetLength(0);
            int height = cells.GetLength(1);
            hasChanged = false;

            bool[,] nextGen = new bool[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int neighborCount = 0;

                    // Top left
                    if (x > 0 && y > 0)
                    {
                        if (cells[x - 1, y - 1] == true)
                            neighborCount++;
                    }

                    // Top middle
                    if (y > 0)
                    {
                        if (cells[x, y - 1] == true)
                            neighborCount++;
                    }

                    // Top right
                    if (x < width - 1 && y > 0)
                    {
                        if (cells[x + 1, y - 1] == true)
                            neighborCount++;
                    }

                    // Center left
                    if (x > 0)
                    {
                        if (cells[x - 1, y] == true)
                            neighborCount++;
                    }

                    // Center right
                    if (x < width - 1)
                    {
                        if (cells[x + 1, y] == true)
                            neighborCount++;
                    }

                    // Bottom left
                    if (x > 0 && y < height - 1)
                    {
                        if (cells[x - 1, y + 1] == true)
                            neighborCount++;
                    }

                    // Bottom middle
                    if (y < height - 1)
                    {
                        if (cells[x, y + 1] == true)
                            neighborCount++;
                    }

                    // Bottom right
                    if (x < width - 1 && y < height - 1)
                    {
                        if (cells[x + 1, y + 1] == true)
                            neighborCount++;
                    }

                    if (cells[x, y] == true)
                    {
                        // Current gen is alive.

                        // Any live cell with fewer than two live neighbours dies, as if caused by under-population. 
                        if (neighborCount < 2)
                        {
                            nextGen[x, y] = false;
                            hasChanged = true;
                        }

                        // Any live cell with two or three live neighbours lives on to the next generation.
                        if (neighborCount == 2 || neighborCount == 3)
                            nextGen[x, y] = true;

                        // Any live cell with more than three live neighbours dies, as if by overcrowding. 
                        if (neighborCount > 3)
                        {
                            nextGen[x, y] = false;
                            hasChanged = true;
                        }
                    }
                    else
                    {
                        // Current gen is dead.

                        // Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
                        if (neighborCount == 3)
                        {
                            nextGen[x, y] = true;
                            hasChanged = true;
                        }
                    }

                    if (nextGen[x, y] == true)
                        population++;
                }
            }

            cells = nextGen;

            return population;
        }

        public static bool IsIdentical(bool[,] cells1, bool[,] cells2)
        {
            for (int i = 0; i < cells1.GetLength(0); i++)
            {
                for (int j = 0; j < cells1.GetLength(1); j++)
                {
                    if (cells1[i, j] != cells2[i, j])
                        return false;
                }
            }

            return true;
        }

        #endregion
    }
}
