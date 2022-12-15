using System.Drawing;
using System.Text.RegularExpressions;

public abstract class Element
{
    public Element(int x, int y, string drawn, ConsoleColor color)
    {
        X = x;
        Y = y;
        Drawn = drawn;
        Color = color;
    }

    public int X { get; }
    public int Y { get; }
    public string Drawn { get; }
    public ConsoleColor Color { get; }

    public override string ToString()
    {
        return $"{this.GetType().Name}: {X}, {Y}";
    }
}

public class Sensor : Element
{

    public Sensor(int x, int y, Beacon closestBeacon, int targetRow) : base(x, y, "S", ConsoleColor.Yellow)
    {
        ClosestBeacon = closestBeacon;
        this._targetRow = targetRow;
        CalculateNoBeacons();
    }

    private void CalculateNoBeacons()
    {
        //Distance to beacon
        _noBeacons.Clear();

        var diffX = Math.Abs(this.X - ClosestBeacon.X);
        var diffY = Math.Abs(this.Y - ClosestBeacon.Y);

        var distance = diffX + diffY;

        var itemCount = 1;

        for (var y = this.Y - distance; y <= this.Y + distance; y++)
        {
            if (y == _targetRow)
            {
                _noBeacons.Add(new Empty(this.X, y));
                for (int i = 1; i < itemCount; i++)
                {
                    _noBeacons.Add(new Empty(this.X - i, y));
                    _noBeacons.Add(new Empty(this.X + i, y));
                }
            }
            if (y > this.Y)
            {
                itemCount--;
            }
            else
            {
                itemCount++;
            }
        }

        _noBeacons.RemoveAll(x => x.X == ClosestBeacon.X && x.Y == ClosestBeacon.Y);



    }

    public static Sensor FromLine(string line, int targetRow)
    {
        var match = Regex.Match(line, @"Sensor at x=(?<sx>-*\d*), y=(?<sy>-*\d*): closest beacon is at x=(?<bx>-*\d*), y=(?<by>-*\d*)");
        return new Sensor(int.Parse(match.Groups["sx"].Value), int.Parse(match.Groups["sy"].Value),
            new Beacon(int.Parse(match.Groups["bx"].Value), int.Parse(match.Groups["by"].Value)), targetRow);
    }

    public Beacon ClosestBeacon { get; }

    private List<Empty> _noBeacons = new List<Empty>();
    private readonly int _targetRow;

    public Empty[] NoBeacons
    {
        get
        {
            return _noBeacons.ToArray();
        }
    }

    public int RangeMinX
    {
        get
        {
            return _noBeacons.Count > 0 ? _noBeacons.Min(r => r.X) : int.MaxValue;
        }
    }

    public int RangeMaxX
    {
        get
        {
            return _noBeacons.Count > 0 ? _noBeacons.Max(r => r.X): int.MinValue;
        }
    }
}

public class Empty : Element
{
    public Empty(int x, int y) : base(x, y, "#", ConsoleColor.DarkGray)
    {
    }
}

public class Beacon : Element
{
    public Beacon(int x, int y) : base(x, y, "B", ConsoleColor.Red)
    {
    }
}

public static class Puzzle
{

    public static List<Sensor> GetSensors(string inputFile, int targetRow)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        var sensorList = new List<Sensor>();
        do
        {
            var line = sr.ReadLine();
            sensorList.Add(Sensor.FromLine(line, targetRow));
            
        }
        while (!sr.EndOfStream);

        return sensorList;
    }
    
    public static void PartOne(string inputFile)
    {
        var targetRow = 10;
        var sensors = Puzzle.GetSensors(inputFile, targetRow);

        var minX = sensors.Min(r => r.RangeMinX);
        var maxX = sensors.Max(r => r.RangeMaxX);

        

        var emptyYPoints = new Dictionary<int, int>(Enumerable.Range(minX, maxX - minX + 1).Select(r => new KeyValuePair<int, int>(r, 0)));
        foreach(var sensor in sensors)
        {
            var noHits = sensor.NoBeacons.Where(r => r.Y == targetRow).Select(r => r.X);
            foreach(var n in noHits)
            {
                emptyYPoints[n]++;
            }
        }







        //var testSensor = Sensor.FromLine("Sensor at x=8, y=7: closest beacon is at x=2, y=10");
        Console.WriteLine(emptyYPoints.Where(r=>r.Value != 0).Count());

    }

    public static void PartTwo(string inputFile)
    {
        var sensors = Puzzle.GetSensors(inputFile, 0);
    }
}