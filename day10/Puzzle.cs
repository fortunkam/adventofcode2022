public static class Puzzle
{
    public static void PartOne(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);
        do
        {
            var line = sr.ReadLine();
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
        }
        while(!sr.EndOfStream);
    }
}