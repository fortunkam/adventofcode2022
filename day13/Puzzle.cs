using System.Text;
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

            var element = ElementFactory.FromLine(line);

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

    public override string ToString()
    {
        return Value.ToString();
    }
}

public class ArrayElement : Element
{
    public List<Element> Elements {get;set;}

    public override string ToString()
    {
        var sb = new StringBuilder("ARRAY");
        foreach(var e in Elements)
        {
            sb.Append($"\t{e.ToString()}");
        }
        return sb.ToString();
    }
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

        var startArray = new ArrayElement();
        ArrayElement currentArray = null;
        var arrays = new Queue<ArrayElement>();
        var numbers = new List<IntElement>();

        var buffer = new List<char>();
        foreach(var i in line)
        {
            switch(i)
            {
                case ']':
                    var curr = arrays.Dequeue();
                    if(buffer.Count > 0)
                    {
                        var number = int.Parse(new String(buffer.ToArray()));
                        numbers.Add(new IntElement { Value = number});
                        curr.Elements.AddRange(numbers);
                        buffer.Clear();
                    }
                    numbers.Clear();
                    break;
                case ',':
                    if(buffer.Count > 0)
                    {
                        var numberComma = int.Parse(new String(buffer.ToArray()));
                        numbers.Add(new IntElement { Value = numberComma});
                        buffer.Clear();
                    }
                    break;
                case '[':
                    if(currentArray == null)
                    {
                        currentArray = startArray;
                    }
                    else
                    {
                        var newArray = new ArrayElement();
                        currentArray.Elements.Add(newArray);
                        currentArray = newArray;
                    }
                    arrays.Enqueue(currentArray);
                break;
                default:
                    buffer.Insert(0,i);
                    break;
            }
        }

        return startArray;
    }


}