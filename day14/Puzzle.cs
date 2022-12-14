using System.Text.RegularExpressions;

public abstract class Element
{
    public Element(int x, int y, string display)
    {
        X= x; Y=y; 
        Display= display; 
    }
    public int X; public int Y;

    public string Display;

    public override string ToString()
    {
        return $"{Display}({X},{Y})";
    }
}

public class Rock : Element
{
    public Rock(int x, int y): base(x, y, "#") { }
}

public class Sand : Element
{
    public Sand(int x, int y) : base(x, y, "o") { }
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

        DrawPoints(rockPoints);

       
    }

    //public static Element[,] BuildMap(List<Element> rocks)
    //{
        
    //}

    public static void DrawPoints(List<Element> rocks)
    {
        var minX = rocks.Min(r => r.X);
        var maxX = rocks.Max(r => r.X);
        var minY = rocks.Min(r => r.Y);
        var maxY = rocks.Max(r => r.Y);

        if (minY > 0)
        {
            minY = 0;
        }

        if(minX > 500)
        {
            minX = 500;
        }

        if(maxX < 500) { maxX = 500; }

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                var rock = rocks.FirstOrDefault(r => r.X == x && r.Y == y);
                if (rock != null)
                {
                    Console.Write(rock.Display);
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }
    }
    
    public static void PartTwo(string inputFile)
    {
        
    }
}