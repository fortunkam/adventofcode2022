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
    private readonly Beacon _closestBeacon;

    public Sensor(int x, int y, Beacon closestBeacon) : base(x, y, "S", ConsoleColor.Yellow)
    {
        _closestBeacon = closestBeacon;
        CalculateNoBeacons();
    }

    private void CalculateNoBeacons()
    {
        //Distance to beacon
        _noBeacons.Clear();

        var diffX = Math.Abs(this.X - _closestBeacon.X);
        var diffY = Math.Abs(this.Y - _closestBeacon.Y);

        var distance = diffX + diffY;

        var itemCount = 1;
        var rightItemCount = 1;
        for (var y = this.Y - distance; y <= this.Y + distance; y++)
        {
            _noBeacons.Add(new Empty(this.X, y));
            for (int i = 1; i < itemCount; i++)
            {
                _noBeacons.Add(new Empty(this.X - i, y));
                _noBeacons.Add(new Empty(this.X + i, y));
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

        _noBeacons.RemoveAll(x => x.X == _closestBeacon.X && x.Y == _closestBeacon.Y);



    }

    public static Sensor FromLine(string line)
    {
        var match = Regex.Match(line, @"Sensor at x=(?<sx>-*\d*), y=(?<sy>-*\d*): closest beacon is at x=(?<bx>-*\d*), y=(?<by>-*\d*)");
        return new Sensor(int.Parse(match.Groups["sx"].Value), int.Parse(match.Groups["sy"].Value),
            new Beacon(int.Parse(match.Groups["bx"].Value), int.Parse(match.Groups["by"].Value)));
    }

    private List<Empty> _noBeacons = new List<Empty>();

    public Empty[] NoBeacons
    {
        get
        {
            return _noBeacons.ToArray();
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

    public static List<Sensor> GetSensors(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        var sensorList = new List<Sensor>();
        do
        {
            var line = sr.ReadLine();
            sensorList.Add(Sensor.FromLine(line));
            
        }
        while (!sr.EndOfStream);

        return sensorList;
    }
    
    public static void PartOne(string inputFile)
    {
            // var sensors = Puzzle.GetSensors(inputFile);

            var testSensor = Sensor.FromLine("Sensor at x=8, y=7: closest beacon is at x=2, y=10");
            Console.WriteLine(testSensor);

    }

    public static void PartTwo(string inputFile)
    {
        var sensors = Puzzle.GetSensors(inputFile);
    }
}