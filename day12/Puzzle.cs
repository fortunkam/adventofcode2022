using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing;
using System.ComponentModel.DataAnnotations;

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
                    start = map[x, y];
                }
                else if (points[y][x] == 'E')
                {
                    map[x, y] = new Cell(x, y, 'z');
                    end = map[x, y];
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
                if (y > 0 && map[x, y - 1].Value - currentChar <= 1)
                {
                    map[x, y].AddNeighbour(map[x, y - 1]);
                }

                if (y + 1 < ySize && map[x, y + 1].Value - currentChar <= 1)
                {
                    map[x, y].AddNeighbour(map[x, y + 1]);
                }
                
                if (x > 0 && map[x - 1, y].Value - currentChar <= 1)
                {
                    map[x, y].AddNeighbour(map[x - 1,y]);
                }

                if (x + 1 < xSize && map[x + 1, y].Value - currentChar <= 1)
                {
                    map[x, y].AddNeighbour(map[x + 1, y]);
                }
            }
        }

        //var result = GetShortestRoute(start, new List<Point>());

        //foreach(var r in result)
        //{
        //    Console.WriteLine(r);
        //}

        var result = GetRoute(start, new List<Cell>()
        {
            start
        });

        Console.WriteLine(result.Count - 1);
    }

    public static List<Cell> GetRoute(Cell current, List<Cell> route)
    {
        if (current.X == end.X && current.Y == end.Y)
        {
            return route;
        }

        var neighbours = current.Neighbours;
        var shortestRoute = new List<Cell>();
        foreach (var neighbour in neighbours)
        {
            if (route.Contains(neighbour))
            {
                continue;
            }

            var newRoute = new List<Cell>(route)
            {
                neighbour
            };
            var newShortestRoute = GetRoute(neighbour, newRoute);
           // Console.WriteLine(newShortestRoute.Count);
            if(shortestRoute.Count == 0)
            {
                shortestRoute = newShortestRoute;
            }
            else if (newShortestRoute.Count != 0 && newShortestRoute.Count < shortestRoute.Count)
            {
                shortestRoute = newShortestRoute;
            }
        }

        return shortestRoute;
    }

    //private static Dictionary<string, List<Point>> _history = new Dictionary<string, List<Point>>();

    //private static List<Point> GetShortestRoute(Point current, List<Point> history)
    //{

    //   // Console.WriteLine($"Scanning {current} contains value {points[current.Y][current.X]}");

    //    var cacheKey = $"{current.X},{current.Y}";
    //    if (history.Any(x => x.X == current.X && x.Y == current.Y))
    //    {
    //        return null;
    //    }

    //    if (points[current.Y][current.X] == 'E')
    //    {
    //        return history;
    //    }

    //    char targetChar = 'a';
    //    if (points[current.Y][current.X] != 'S')
    //    {
    //        if (points[current.Y][current.X] == 'z')
    //        {
    //            targetChar = 'E';
    //        }
    //        else
    //        {
    //            targetChar = (char)((int)(points[current.Y][current.X]) + 1);
    //        }
    //    }

    //    var results = new List<List<Point>>();

    //    var newHistory = new List<Point>(history)
    //    {
    //        current
    //    };

    //    if (current.Y > 0 
    //        && (points[current.Y - 1][current.X] == targetChar || points[current.Y - 1][current.X] == points[current.Y][current.X])
    //        && !history.Any(r=>r.X == current.X && r.Y == current.Y -1))
    //    {

    //        var r = GetShortestRoute(new Point(current.X, current.Y - 1), newHistory);
    //        if(r.Count > 0)
    //        {
    //            results.Add(r);
    //        }
            
    //    }

    //    if (current.Y < points.Count - 1 
    //        && (points[current.Y + 1][current.X] == targetChar || points[current.Y + 1][current.X] == points[current.Y][current.X])
    //        && !history.Any(r => r.X == current.X && r.Y == current.Y + 1))
    //    {
    //        var r = GetShortestRoute(new Point(current.X, current.Y + 1), newHistory);
    //        if (r?.Count > 0)
    //        {
    //            results.Add(r);
    //        }
    //    }

    //    if (current.X > 0 
    //        && (points[current.Y][current.X- 1] == targetChar || points[current.Y][current.X - 1] == points[current.Y][current.X])
    //        && !history.Any(r => r.X == current.X - 1 && r.Y == current.Y))
    //    {
    //        var r = GetShortestRoute(new Point(current.X - 1, current.Y), newHistory);
    //        if (r?.Count > 0)
    //        {
    //            results.Add(r);
    //        }
    //    }

    //    if (current.X < points[0].Count - 1 
    //        && (points[current.Y][current.X+ 1] == targetChar || points[current.Y][current.X + 1] == points[current.Y][current.X])
    //        && !history.Any(r => r.X == current.X + 1 && r.Y == current.Y))
    //    {
    //        var r = GetShortestRoute(new Point(current.X+ 1, current.Y), newHistory);
    //        if (r?.Count > 0)
    //        {
    //            results.Add(r);
    //        }
    //    }

    //    var resultEnd = results.Count == 0 ? new List<Point>()
    //        : results.Where(r => r.Count > 0).OrderBy(r => r.Count).FirstOrDefault() ?? new List<Point>();

    //   // _history.Add(cacheKey, resultEnd);


    //    return resultEnd;
    //}
    
    public static void PartTwo(string inputFile) 
    {
            
        
    }

}
