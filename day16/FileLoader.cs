using System.Text.RegularExpressions;

public static class FileLoader
{
    public static List<Valve> FromFile(string inputFile)
    {
        var match = new Regex(@"Valve (?<name>[A-Z]{2}) has flow rate=(?<rate>\d*); tunnels lead to valves (?<link>([A-Z]{2}(,\s|$|\r))*)", RegexOptions.Compiled);
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);
        var valves = new List<Valve>();
        do
        {
            var line = sr.ReadLine();

        }
        while (!sr.EndOfStream);

        return valves;
    }
}