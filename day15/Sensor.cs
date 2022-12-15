using System.Text.RegularExpressions;

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

        RangeMinX = this.X - distance;
        RangeMaxX = this.X + distance;

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
            if (y >= this.Y)
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

    public int RangeMinX { get; private set; }

    public int RangeMaxX { get; private set; }
}
