using System.Drawing;
using System.Text.RegularExpressions;

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

    public static List<SensorTwo> GetSensorsTwo(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        var sensorList = new List<SensorTwo>();
        do
        {
            var line = sr.ReadLine();
            sensorList.Add(SensorTwo.FromLine(line));

        }
        while (!sr.EndOfStream);

        return sensorList;
    }

    public static void PartOne(string inputFile)
    {
        var targetRow = 2000000;
        var sensors = Puzzle.GetSensors(inputFile, targetRow);

        var minX = sensors.Min(r => r.RangeMinX);
        var maxX = sensors.Max(r => r.RangeMaxX);





        var emptyYPoints = new Dictionary<int, int>(Enumerable.Range(minX, maxX - minX + 1).Select(r => new KeyValuePair<int, int>(r, 0)));
        foreach (var sensor in sensors)
        {
            var noHits = sensor.NoBeacons.Where(r => r.Y == targetRow).Select(r => r.X);
            foreach (var n in noHits)
            {
                emptyYPoints[n]++;
            }
        }

        foreach (var sensor in sensors)
        {
            if (sensor.ClosestBeacon.Y == targetRow)
            {
                emptyYPoints[sensor.ClosestBeacon.X] = 0;
            }

            if (sensor.Y == targetRow)
            {
                emptyYPoints[sensor.X] = 0;
            }
        }

        //foreach(var i in emptyYPoints.OrderBy(r=>r.Key))
        //{
        //    Console.WriteLine($"{i.Key} => {i.Value}");
        //}

        //var testSensor = Sensor.FromLine("Sensor at x=8, y=7: closest beacon is at x=2, y=10",targetRow);
       Console.WriteLine(emptyYPoints.Where(r=>r.Value > 0).Count());

    }

    public static void PartTwo(string inputFile)
    {
        var targetRow = 4000000;

        var sensors = Puzzle.GetSensorsTwo(inputFile);
        var xpoints = new List<int>();

        var matchedY = 0;

        for (var y = 0; y < targetRow; y++)
        {
            xpoints.Clear();
            var list = new List<int>();


            var noBeacons = sensors
                .Select(r => new int[] {
                    Math.Max(r.MinimumX(y), 0),
                    Math.Min(r.MaximumX(y), targetRow) } )
                .Where(r=>r[0] <= r[1])
                .OrderBy(r => r[0])
                .ToArray();

            var listStuff = new List<int[]>(noBeacons);
            bool match = true;
            while(match && listStuff.Count > 1)
            {
                match = false;
                if (Overlap(listStuff[0], listStuff[1]))
                {                    
                    listStuff[0][1] = Math.Max(listStuff[0][1], listStuff[1][1]);
                    listStuff.RemoveAt(1);
                    match = true;
                }
            }

            if(!match || listStuff[0][0] != 0 || listStuff[0][1] != targetRow)
            {
                var xResult = listStuff[0][1] + 1;
                var yResult = y;

                var result = ((Int64)xResult * (Int64)4000000) + (Int64)yResult;

                Console.WriteLine(result);
                return;
            }

        }

    }

    private static bool Overlap(int[] first, int[] second)
    {
        return (first[0] <= second[0] && first[1] >= second[0]);
    }
}