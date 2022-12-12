using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing;
using System.ComponentModel.DataAnnotations;

public static class Puzzle
{
    public static List<List<char>> points = new List<List<char>>();

    public static Point start = new Point();
    public static Point end = new Point();
    
    public static void PartOne(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        do
        {
            var line = sr.ReadLine();
            var pointLine = line.ToCharArray().ToList();
            points.Add(pointLine);
            if (pointLine.Contains('S'))
            {
                start = new Point(pointLine.IndexOf('S'), points.Count - 1);
            }
            if(pointLine.Contains('E'))
            {
                end = new Point(pointLine.IndexOf('E'), points.Count - 1);
            }
        }
        while (!sr.EndOfStream);

        var result = GetShortestRoute(start, new List<Point>());

        Console.WriteLine(result.Count);
    }

    private static Dictionary<Point, List<Point>> _history = new Dictionary<Point, List<Point>>();

    private static List<Point> GetShortestRoute(Point current, List<Point> history)
    {
        if (points[current.Y][current.X] == 'E')
        {
            return history;
        }

        char targetChar = 'a';
        if (points[current.Y][current.X] != 'S')
        {
            targetChar = (char)((int)(points[current.Y][current.X]) + 1);
        }

        var results = new List<List<Point>>();

        if (current.Y > 0 && points[current.Y - 1][current.X] == targetChar)
        {
            var newHistory = new List<Point>(history)
            {
                current
            };
            var p = new Point(current.X, current.Y - 1);
            if (_history.ContainsKey(p))
            {
                results.Add(_history[p]);
            }
            else
            {
                var result = GetShortestRoute(p, newHistory);
                results.Add(result);
                _history.Add(p, result);
            }
        }

        if (current.Y < points.Count - 1 && points[current.Y + 1][current.X] == targetChar)
        {
            var newHistory = new List<Point>(history)
            {
                current
            };
            var p = new Point(current.X, current.Y + 1);
            if (_history.ContainsKey(p))
            {
                results.Add(_history[p]);
            }
            else
            {
                var result = GetShortestRoute(p, newHistory);
                results.Add(result);
                _history.Add(p, result);
            }
        }

        if (current.X > 0 && points[current.Y][current.X - 1] == targetChar)
        {
            var newHistory = new List<Point>(history)
            {
                current
            };
            var p = new Point(current.X - 1, current.Y);
            if (_history.ContainsKey(p))
            {
                results.Add(_history[p]);
            }
            else
            {
                var result = GetShortestRoute(p, newHistory);
                results.Add(result);
                _history.Add(p, result);
            }
        }

        if (current.X < points[0].Count - 1 && points[current.Y][current.X + 1] == targetChar)
        {
            var newHistory = new List<Point>(history)
            {
                current
            };
            var p = new Point(current.X + 1, current.Y);
            if (_history.ContainsKey(p))
            {
                results.Add(_history[p]);
            }
            else
            {
                var result = GetShortestRoute(p, newHistory);
                results.Add(result);
                _history.Add(p, result);
            }
        }

        return results.OrderBy(r => r.Count).First();
    }
    
    public static void PartTwo(string inputFile) 
    {
            
        
    }

}
