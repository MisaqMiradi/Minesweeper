using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MineSweeper
{
    public class Field
    {
        private int[,] board;
        private bool[,] discovered;
        private HashSet<int> mines;
        private int row;
        private int col;
        private int totalMines;
        private bool started;

        public bool Started => started;

        public HashSet<int> Discovered
        {
            get
            {
                HashSet<int> discoveredIndices = new HashSet<int>();
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        if (discovered[i, j])
                        {
                            discoveredIndices.Add(i * col + j);
                        }
                    }
                }
                return discoveredIndices;
            }
        }

        public HashSet<int> Flagged { get; }

        public Field(int row, int col, int totalMines)
        {
            this.row = row;
            this.col = col;
            this.totalMines = totalMines;
            board = new int[row, col];
            discovered = new bool[row, col];
            mines = new HashSet<int>();
            Flagged = new HashSet<int>();
            started = false;
        }

        public void Initialize(int startX, int startY)
        {
            GenerateMines(startX, startY);
            CalculateNumbers();
            started = true;
        }

        private void GenerateMines(int startX, int startY)
        {
            Random random = new Random();
            int minesToPlace = totalMines;

            while (minesToPlace > 0)
            {
                int x = random.Next(row);
                int y = random.Next(col);

                if ((x != startX || y != startY) && !mines.Contains(x * col + y))
                {
                    mines.Add(x * col + y);
                    minesToPlace--;
                }
            }
        }

        private void CalculateNumbers()
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (!mines.Contains(i * col + j))
                    {
                        int count = 0;
                        foreach (int neighbor in GetNeighbors(i, j))
                        {
                            if (mines.Contains(neighbor))
                            {
                                count++;
                            }
                        }
                        board[i, j] = count;
                    }
                }
            }
        }

        public int CountMines(int x, int y)
        {
            int count = 0;
            foreach (int neighbor in GetNeighbors(x, y))
            {
                if (mines.Contains(neighbor))
                {
                    count++;
                }
            }
            return count;
        }

        public HashSet<int> GetSafeIsland(int x, int y)
        {
            HashSet<int> safeIsland = new HashSet<int> { x * col + y };
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(x * col + y);
            discovered[x, y] = true;

            while (queue.Count > 0)
            {
                int current = queue.Dequeue();
                int currentX = current / col;
                int currentY = current % col;

                if (board[currentX, currentY] == 0)
                {
                    foreach (int neighbor in GetNeighbors(currentX, currentY))
                    {
                        int neighborX = neighbor / col;
                        int neighborY = neighbor % col;

                        if (!discovered[neighborX, neighborY] && !mines.Contains(neighbor))
                        {
                            discovered[neighborX, neighborY] = true;
                            queue.Enqueue(neighbor);
                            safeIsland.Add(neighbor);
                        }
                    }
                }
            }

            return safeIsland;
        }

        public bool IsMine(int x, int y)
        {
            return mines.Contains(x * col + y);
        }

        public IEnumerable<int> GetNeighbors(int x, int y)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && i < row && j >= 0 && j < col && !(i == x && j == y))
                    {
                        yield return i * col + j;
                    }
                }
            }
        }

        public bool Win()
        {
            int discoveredCount = Discovered.Count;
            int safeCount = row * col - totalMines;

            return discoveredCount == safeCount;
        }
    }
}

