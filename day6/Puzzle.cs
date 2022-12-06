public static class Puzzle
{
    public static void PartOne(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);
        do
        {
            var line = sr.ReadLine();
            var chars = line.ToCharArray();
            for(var i=0; i< chars.Length - 4;i++)
            {
                var isValidGroup = chars.Skip(i).Take(4).GroupBy(r=>r).All(r=> r.Count() == 1);
                if(isValidGroup)
                {
                    Console.WriteLine($"Found valid Group at Index {i+4}");
                    break;
                }

            }
        }
        while(!sr.EndOfStream);
    }

    public static void PartTwo(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);
        do
        {
            var line = sr.ReadLine();
            var chars = line.ToCharArray();
            for(var i=0; i< chars.Length - 14;i++)
            {
                var isValidGroup = chars.Skip(i).Take(14).GroupBy(r=>r).All(r=> r.Count() == 1);
                if(isValidGroup)
                {
                    Console.WriteLine($"Found valid Group at Index {i+14}");
                    break;
                }

            }
        }
        while(!sr.EndOfStream);
    }
}