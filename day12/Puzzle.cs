using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

public class Cell
{
    public Cell(int x, int y, char value)
    {
        X = x;
        Y = y;
        Value = value;
    }

    public int X { get; }
    public int Y { get; }
    public char Value { get; }

    private List<Cell> _cells = new List<Cell>();

    public void AddNeighbour(Cell neighbour)
    {
        _cells.Add(neighbour);
    }

    public Cell[] Neighbours
    {
        get { return _cells.ToArray(); }
    }

    public bool Visited { get; set; }

    public Cell Previous { get; set; }
}


public static class Puzzle
{
    public static List<List<char>> points = new List<List<char>>();

    public static Cell start;
    public static Cell end;

    public static Cell[,] map;


    public static void PartOne(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        do
        {
            var line = sr.ReadLine();
            var pointLine = line.ToCharArray().ToList();
            points.Add(pointLine);
        }
        while (!sr.EndOfStream);


        int xSize = points[0].Count;
        int ySize = points.Count;

        map = new Cell[xSize,ySize];
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                if (points[y][x] == 'S')
                {
                    map[x, y] = new Cell(x, y, 'a');
                    end = map[x, y];
                }
                else if (points[y][x] == 'E')
                {
                    map[x, y] = new Cell(x, y, 'z');
                    start = map[x, y];
                }
                else
                {
                    map[x, y] = new Cell(x, y, points[y][x]);
                }
            }
        }

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                var currentChar = map[x, y].Value;
                if (y > 0 && currentChar - map[x, y - 1].Value <= 1)
                {
                    map[x, y].AddNeighbour(map[x, y - 1]);
                }

                if (y + 1 < ySize && currentChar - map[x, y + 1].Value <= 1 )
                {
                    map[x, y].AddNeighbour(map[x, y + 1]);
                }
                
                if (x > 0 && currentChar - map[x - 1, y].Value <= 1)
                {
                    map[x, y].AddNeighbour(map[x - 1,y]);
                }

                if (x + 1 < xSize && currentChar - map[x + 1, y].Value <= 1)
                {
                    map[x, y].AddNeighbour(map[x + 1, y]);
                }
            }
        }

        Queue<Cell> queue = new Queue<Cell>();
        start.Visited = true;
        queue.Enqueue(start);

        while(queue.Count > 0)
        {
            var current = queue.Dequeue();
            GetRoute(current, queue);
        }

        Console.WriteLine(CalculateDistance());
    }

    public static int CalculateDistance()
    {
        var current = end;
        var count = 0;
        
        while(current != start)
        {
            if (!current.Visited || current.Previous == null)
                return int.MaxValue;
            current = current.Previous;
            count++;
        }

        return count;
    }

    public static int CalculateDistanceToEnd(Cell testEnd)
    {
        var current = testEnd;
        var count = 0;

        while (current != start)
        {
            if (!current.Visited || current.Previous == null)
                return int.MaxValue;
            current = current.Previous;
            count++;
        }

        return count;
    }

    public static void GetRoute(Cell current, Queue<Cell> queue)
    {
        foreach (var neighbour in current.Neighbours)
        {
            if (!neighbour.Visited)
            {
                neighbour.Visited = true;
                neighbour.Previous = current;
                queue.Enqueue(neighbour);
            }
        }
    }

    public static void PartTwo(string inputFile) 
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        do
        {
            var line = sr.ReadLine();
            var pointLine = line.ToCharArray().ToList();
            points.Add(pointLine);
        }
        while (!sr.EndOfStream);


        int xSize = points[0].Count;
        int ySize = points.Count;

        var alternateEndPoints = new List<Cell>();
        
        map = new Cell[xSize, ySize];
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                if (points[y][x] == 'S')
                {
                    map[x, y] = new Cell(x, y, 'a');
                    end = map[x, y];
                }
                else if (points[y][x] == 'E')
                {
                    map[x, y] = new Cell(x, y, 'z');
                    start = map[x, y];
                }
                else
                {
                    map[x, y] = new Cell(x, y, points[y][x]);
                    if (points[y][x] == 'a')
                    {
                        alternateEndPoints.Add(map[x, y]);
                    }
                }
            }
        }

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                var currentChar = map[x, y].Value;
                if (y > 0 && currentChar - map[x, y - 1].Value <= 1)
                {
                    map[x, y].AddNeighbour(map[x, y - 1]);
                }

                if (y + 1 < ySize && currentChar - map[x, y + 1].Value <= 1)
                {
                    map[x, y].AddNeighbour(map[x, y + 1]);
                }

                if (x > 0 && currentChar - map[x - 1, y].Value <= 1)
                {
                    map[x, y].AddNeighbour(map[x - 1, y]);
                }

                if (x + 1 < xSize && currentChar - map[x + 1, y].Value <= 1)
                {
                    map[x, y].AddNeighbour(map[x + 1, y]);
                }
            }
        }

        Queue<Cell> queue = new Queue<Cell>();
        start.Visited = true;
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            GetRoute(current, queue);
        }
        

        Console.WriteLine(alternateEndPoints.Min(r => CalculateDistanceToEnd(r)));
        

        

    }

}
