public static class Pairs
{
    public static void PartOne(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        var regex = new System.Text.RegularExpressions.Regex(@"(?<firststart>[0-9]*)-(?<firstend>[0-9]*),(?<secondstart>[0-9]*)-(?<secondend>[0-9]*)", System.Text.RegularExpressions.RegexOptions.Compiled);

        var count = 0;
        do
        {
            var line = sr.ReadLine();
            var match = regex.Match(line);
            var f1 = int.Parse(match.Groups["firststart"].Value);
            var f2 = int.Parse(match.Groups["firstend"].Value);
            var s1 = int.Parse(match.Groups["secondstart"].Value);
            var s2 = int.Parse(match.Groups["secondend"].Value);

            var range1 = Enumerable.Range(f1,f2-f1 + 1).ToArray();
            var range2 = Enumerable.Range(s1,s2-s1 + 1).ToArray();

            //Console.WriteLine($"group 1 ({f1},{f2}), group 2 ({s1},{s2})");

            var intersect = range1.Intersect(range2).Count();
            if(intersect == range1.Length || intersect == range2.Length)
            {
                //Console.WriteLine("Complete Enclosure");
                count++;
            }

            
        }
        while(!sr.EndOfStream);

        Console.WriteLine(count);

    }

    public static void PartTwo(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        var regex = new System.Text.RegularExpressions.Regex(@"(?<firststart>[0-9]*)-(?<firstend>[0-9]*),(?<secondstart>[0-9]*)-(?<secondend>[0-9]*)", System.Text.RegularExpressions.RegexOptions.Compiled);

        var count = 0;
        do
        {
            var line = sr.ReadLine();
            var match = regex.Match(line);
            var f1 = int.Parse(match.Groups["firststart"].Value);
            var f2 = int.Parse(match.Groups["firstend"].Value);
            var s1 = int.Parse(match.Groups["secondstart"].Value);
            var s2 = int.Parse(match.Groups["secondend"].Value);

            var range1 = Enumerable.Range(f1,f2-f1 + 1).ToArray();
            var range2 = Enumerable.Range(s1,s2-s1 + 1).ToArray();

            //Console.WriteLine($"group 1 ({f1},{f2}), group 2 ({s1},{s2})");

            var intersect = range1.Intersect(range2).Count();
            if(intersect > 0)
            {
                //Console.WriteLine("Complete Enclosure");
                count++;
            }

            
        }
        while(!sr.EndOfStream);

        Console.WriteLine(count);

    }
}