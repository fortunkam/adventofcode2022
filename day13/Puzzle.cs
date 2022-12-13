using System.Text;
using System.Text.RegularExpressions;

public static class Puzzle
{
    public static void PartOne(string inputFile)
    {

        var ed = ElementFactory.FromLine("[[1],[2,3,4]]");
        Console.WriteLine(ed.ToString());

        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        Element first = null;
        Element second = null;

        var validIndicies = new List<int>();
        var currentPair = 1;

        do
        {  
            var line = sr.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                if (AreEqual(first, second))
                {
                    validIndicies.Add(currentPair);
                }

                first = null;
                second = null;
                currentPair++;

            }
            else
            {
                var element = ElementFactory.FromLine(line);
                if (first == null)
                {
                    first = element;
                }
                else
                {
                    second = element;
                }
            }

        }
        while (!sr.EndOfStream);

        foreach(var i in validIndicies)
        {
            Console.WriteLine($"{i} is valid");
        }

        Console.WriteLine(validIndicies.Aggregate((r1, r2) => r1 * r2));
    }

    public static bool AreEqual(Element one, Element two)
    {
        if(one is IntElement && two is IntElement)
        {
            return (one as IntElement).Value <= (one as IntElement).Value;       
        }

        if (one is IntElement && two is ArrayElement)
        {
            var tmpOne = new ArrayElement();
            tmpOne.Elements.Add(one);
            return AreEqual(tmpOne, two);
        }

        if (one is ArrayElement && two is IntElement)
        {
            var tmpTwo = new ArrayElement();
            tmpTwo.Elements.Add(two);
            return AreEqual(one, tmpTwo);
        }

        if (one is ArrayElement && two is ArrayElement)
        {
            var result = true;
            var a1 = one as ArrayElement;
            var a2 = two as ArrayElement;

            if (a2.Elements.Count >= a1.Elements.Count) return false;

            for (int i = 0; i < a1.Elements.Count; i++)
            {
                if (i >= a2.Elements.Count) break;
                result = result && AreEqual(a1.Elements[i], a2.Elements[i]);
            }
            return result;

        }

         return false;
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
    public List<Element> Elements {get;set;} = new List<Element>();

    public override string ToString()
    {
        var sb = new StringBuilder("ARRAY\r\n");
        foreach(var e in Elements)
        {
            sb.Append($"\t{e.ToString()}\r\n");
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
       // Console.WriteLine($"Processing {line}");
        if(matchInt.IsMatch(line)) 
        {
           // Console.WriteLine($"Found Integer {line}");
            return new IntElement{ Value = int.Parse(line)};
        }

        var startArray = new ArrayElement();
        ArrayElement currentArray = null;
        var arrays = new Stack<ArrayElement>();
        var numbers = new List<IntElement>();

        var buffer = new List<char>();
        foreach(var i in line)
        {
            switch(i)
            {
                case ']':
                    var curr = arrays.Pop();
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

                        if (numbers.Count > 0)
                        {
                            currentArray.Elements.AddRange(numbers);
                            numbers.Clear();
                        }
                    }
                    else
                    {
                        var newArray = new ArrayElement();
                        currentArray.Elements.Add(newArray);

                        if (numbers.Count > 0)
                        {
                            currentArray.Elements.AddRange(numbers);
                            numbers.Clear();
                        }

                        currentArray = newArray;
                    }
                    arrays.Push(currentArray);
                break;
                default:
                    buffer.Insert(0,i);
                    break;
            }
        }

        return startArray;
    }


}