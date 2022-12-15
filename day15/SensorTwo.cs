using System.Text.RegularExpressions;

public class SensorTwo : Element
{

    public SensorTwo(int x, int y, Beacon closestBeacon) : base(x, y, "S", ConsoleColor.Yellow)
    {
        ClosestBeacon = closestBeacon;
        CalculateRange();
    }

    private int _distance;

    public int MinimumX(int y)
    {
        return this.X - _distance + Math.Abs(this.Y - y);
    }

    public int MaximumX(int y)
    {
        return this.X + _distance - Math.Abs(this.Y - y);
    }

    private void CalculateRange()
    {
        var diffX = Math.Abs(this.X - ClosestBeacon.X);
        var diffY = Math.Abs(this.Y - ClosestBeacon.Y);

        _distance = diffX + diffY;

        //RangeMinX = this.X - _distance;
        //RangeMaxX = this.X + distance;




        //for (var y = this.Y - distance; y <= this.Y + distance; y++)
        //{
        //    if (y == _targetRow)
        //    {
        //        _noBeacons.Add(new Empty(this.X, y));
        //        for (int i = 1; i < itemCount; i++)
        //        {
        //            _noBeacons.Add(new Empty(this.X - i, y));
        //            _noBeacons.Add(new Empty(this.X + i, y));
        //        }
        //    }
        //    if (y >= this.Y)
        //    {
        //        itemCount--;
        //    }
        //    else
        //    {
        //        itemCount++;
        //    }
        //}

        //_noBeacons.RemoveAll(x => x.X == ClosestBeacon.X && x.Y == ClosestBeacon.Y);



    }

    public static SensorTwo FromLine(string line)
    {
        var match = Regex.Match(line, @"Sensor at x=(?<sx>-*\d*), y=(?<sy>-*\d*): closest beacon is at x=(?<bx>-*\d*), y=(?<by>-*\d*)");
        return new SensorTwo(int.Parse(match.Groups["sx"].Value), int.Parse(match.Groups["sy"].Value),
            new Beacon(int.Parse(match.Groups["bx"].Value), int.Parse(match.Groups["by"].Value)));
    }

    public Beacon ClosestBeacon { get; }

}

