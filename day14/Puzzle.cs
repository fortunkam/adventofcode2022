using System.Drawing;
using System.Text.RegularExpressions;

public abstract class Element
{
    public Element(int x, int y, string display, ConsoleColor color)
    {
        X= x; Y=y; 
        Display= display;
        Color = color;
    }
    public int X; public int Y;

    public string Display;
    public ConsoleColor Color;

    public override string ToString()
    {
        return $"{Display}({X},{Y})";
    }
}

public class Rock : Element
{
    public Rock(int x, int y): base(x, y, "#", ConsoleColor.Magenta) { }
}

public class Sand : Element
{
    public Sand(int x, int y) : base(x, y, "o", ConsoleColor.Yellow) { }
}




public static class Puzzle
{
    public static void PartOne(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        var lineMatch = new Regex(@"(?<x>\d*),(?<y>\d*)(\s-\>|$|\r)", RegexOptions.Compiled);

        var rockPoints = new List<Element>();
        do
        {
            var line = sr.ReadLine();

            var matches = lineMatch.Matches(line);
            int? previousX = null;
            int? previousY = null;
            
            foreach(Match match in matches)
            {
                var x = int.Parse(match.Groups["x"].Value);
                var y = int.Parse(match.Groups["y"].Value);
                if (previousX.HasValue)
                {
                    var minX = Math.Min(previousX.Value, x);
                    var maxX = Math.Max(previousX.Value, x);
                    if (minX < maxX)
                    {
                        rockPoints.AddRange(Enumerable.Range(minX, maxX - minX + 1).Select(r => new Rock(r, y)));
                    }
                    var minY = Math.Min(previousY.Value, y);
                    var maxY = Math.Max(previousY.Value, y);
                    if (minY < maxX)
                    {
                        rockPoints.AddRange(Enumerable.Range(minY, maxY - minY + 1).Select(r => new Rock(x, r)));
                    }
                }
                
                previousX = x;
                previousY = y;
            }
            
            
        }
        while (!sr.EndOfStream);

        int xOffset;
        int yOffset;
        var map = BuildMap(rockPoints, out xOffset, out yOffset);
        DrawPoints(map);

        var startPointX = 500;
        var turnCount = 0;
        while(true)
        {

            var result = EvaluateColumn(map, startPointX - xOffset, 0 - yOffset, out bool hitBottom);
            if (hitBottom)
            {
                Console.WriteLine("hit bottom");
                break;
            }
            turnCount++;
            DrawPoints(map);
           // if (turnCount == 24) break;

        }

        

    }

    public static bool EvaluateColumn(Element[,] map, int startX, int startY, out bool hitBottom)
    {
        hitBottom = false;
        for (int y = startY; y < map.GetLength(1); y++)
        {
            if (y >= map.GetLength(1) - 1)
            {
                //At bottom, signal end
                hitBottom = true;
                break;

            }
            var currentX = startX;
            var element = map[currentX, y + 1];
            if (map[currentX, y + 1] != null)
            {
                if (element is Rock)
                {
                    map[currentX, y] = new Sand(currentX, y);
                    hitBottom = false;
                    return true;
                }

                if (element is Sand)
                {
                    var left = map[currentX - 1, y + 1];
                    var right = map[currentX + 1, y + 1];
                    
                    if(right != null && left == null)
                    {
                        map[currentX - 1, y + 1] = new Sand(currentX - 1, y + 1);
                        hitBottom = false;
                        return true;
                    }

                    if (left != null && right == null)
                    {
                        map[currentX + 1, y + 1] = new Sand(currentX + 1, y + 1);
                        hitBottom = false;
                        return true;
                    }

                    if (left != null && right != null)
                    {
                        map[currentX, y] = new Sand(currentX, y);
                        hitBottom = false;
                        return true;
                    }

                    if(right == null && left == null)
                    {   
                        return EvaluateColumn(map, currentX - 1, y, out hitBottom);
                    }
                }

            }
        }
        
        return false;
    }

    public static Element[,] BuildMap(List<Element> rocks, out int xOffset, out int yOffset)
    {
        var minX = rocks.Min(r => r.X);
        var maxX = rocks.Max(r => r.X);
        var countX = maxX - minX + 1;
        var minY = rocks.Min(r => r.Y);
        if (minY > 0)
        {
            minY = 0;
        }
        var maxY = rocks.Max(r => r.Y);
        var countY = maxY - minY + 1;

        yOffset = minY;
        xOffset = minX;

        var map = new Element[countX, countY];

        foreach(var rock in rocks)
        {
            map[rock.X - minX, rock.Y - minY] = rock;
        }

        return map;

    }

    public static void DrawPoints(Element[,] elements)
    {
        for (int y = 0; y < elements.GetLength(1); y++)
        {
            for (int x = 0; x < elements.GetLength(0); x++)
            {
                var e = elements[x,y];
                if (e != null)
                {
                    Console.ForegroundColor = e.Color;
                    Console.Write(e.Display);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }

        Console.ResetColor();
    }
    
    public static void PartTwo(string inputFile)
    {
        
    }
}