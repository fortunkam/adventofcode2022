using System.Text.RegularExpressions;

public static class Puzzle
{
    public static void PartOne(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        do
        {  
            var line = sr.ReadLine();
            if(string.IsNullOrWhiteSpace(line)) continue;

            ElementFactory.FromLine(line);

        }
        while (!sr.EndOfStream);
    }

    public static void PartTwo(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        do
        {  
        }
        while (!sr.EndOfStream);
    }
}


public abstract class Element
{

}

public class IntElement : Element
{
    public int Value {get;set;}
}

public class ArrayElement : Element
{
    public Element[] Elements {get;set;}
}

public static class ElementFactory
{
    private static Regex matchInt = new Regex(@"^\d*$", RegexOptions.Compiled);
    private static Regex matchArray = new Regex(@"(?<start>\[)(?<all>.*?)(?<end>\])", RegexOptions.Compiled);
    public static Element FromLine(string line)
    {
        Console.WriteLine($"Processing {line}");
        if(matchInt.IsMatch(line)) 
        {
            Console.WriteLine($"Found Integer {line}");
            return new IntElement{ Value = int.Parse(line)};
        }
        return new ArrayElement();
    }


}