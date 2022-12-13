using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

public static class Puzzle
{
    public static void PartOne(string inputFile)
    {

        var ed1 = ElementFactory.FromLine("[[2,10,[1,[],5],[[1],[3,0,9,7,2],[6]],[[],[7,0,2]]],[[[0,10]]]]");
        var ed2 = ElementFactory.FromLine("[[3,[[],[5,0,2],2],7,[[0,2,9],2,[4]]],[3,[[4,5]],[[10,2,5,1],[1],9,[3,3,7,7,10],9]],[]]");
        Console.WriteLine(ed1.Draw());
        Console.WriteLine(ed2.Draw());

        Console.WriteLine(AreEqual(ed1, ed2));

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
                if (AreEqual(first, second) == -1)
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

        foreach (var i in validIndicies)
        {
            Console.WriteLine($"{i} is valid");
        }

        Console.WriteLine(validIndicies.Sum(r => r));
    }

    public static int AreEqual(Element one, Element two)
    {
        if(one is IntElement && two is IntElement)
        {
            Console.WriteLine($"Compare {(one as IntElement).Value} vs {(two as IntElement).Value}");
            if((one as IntElement).Value == (two as IntElement).Value) return 0;
            if ((one as IntElement).Value > (two as IntElement).Value) return 1;
            return -1;
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
            var a1 = one as ArrayElement;
            var a2 = two as ArrayElement;


            var maxLength = Math.Max(a1.Elements.Count, a2.Elements.Count);
            for (int i = 0; i < maxLength; i++)
            {
                if (i >= a1.Elements.Count)
                {
                    return -1;
                }
                if (i >= a2.Elements.Count)
                {
                    return 1;
                }
                
                var result = AreEqual(a1.Elements[i], a2.Elements[i]);
                if(result != 0)
                {
                    return result;
                }
            }

        }

         return 0;
    }

    public static void PartTwo(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);


        var elements = new List<Element>();
        
        var twoMarker = new ArrayElement();
        var twoSub = new ArrayElement();
        twoSub.Elements.Add(new IntElement() {  Value = 2 });
        twoMarker.Elements.Add(twoSub);
        elements.Add(twoMarker);
        
        var sixMarker = new ArrayElement();
        var sixSub = new ArrayElement();
        sixSub.Elements.Add(new IntElement() { Value = 6 });
        sixMarker.Elements.Add(sixSub);
        elements.Add(sixMarker);


        var currentPair = 1;

        do
        {
            var line = sr.ReadLine();
            if (!string.IsNullOrWhiteSpace(line))
            {
                var element = ElementFactory.FromLine(line);
                elements.Add(element);
            }
        }
        while (!sr.EndOfStream);

        elements.Sort();

        var twoIndex = elements.IndexOf(twoMarker) + 1;
        var sixIndex = elements.IndexOf(sixMarker) + 1;

        Console.WriteLine(twoIndex * sixIndex);

        

    }
}




public abstract class Element : IComparable<Element>
{
    public string Line { get; set; }

    public int CompareTo(Element? other)
    {
        return Puzzle.AreEqual(this, other);
    }

    public abstract string Draw(int indent = 0);

    public override string ToString()
    {
        return Line;
    }
}

public class IntElement : Element
{
    public int Value {get;set;}

    public override string Draw(int indent = 0)
    {
        return $"{string.Join("", Enumerable.Range(0, indent).Select(_ => "  ").ToArray())}{Value}";
    }
}

public class ArrayElement : Element
{
    public List<Element> Elements {get;set;} = new List<Element>();

    public override string Draw(int indent = 0)
    {
        var sb = new StringBuilder($"{string.Join("",Enumerable.Range(0,indent).Select(_=>"  ").ToArray())}ARRAY{{\r\n");
        foreach(var e in Elements)
        {
            sb.Append($"{string.Join("", Enumerable.Range(0, indent).Select(_ => "  ").ToArray())}{e.Draw(indent+1)}\r\n");
        }
        sb.Append($"{string.Join("", Enumerable.Range(0, indent).Select(_ => "  ").ToArray())}}}");
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
                        var number = int.Parse(new string(buffer.ToArray()));
                        numbers.Add(new IntElement { Value = number, Line = number.ToString()});
                        curr.Elements.AddRange(numbers);
                        curr.Line = $"[{numbers.Select(r => r.Line).Aggregate((a, b) => a + "," + b)}]";
                        buffer.Clear();
                    }
                    numbers.Clear();
                    currentArray = arrays.Count > 0 ? arrays.Peek() : null;
                    break;
                case ',':
                    if(buffer.Count > 0)
                    {
                        var numberComma = int.Parse(new string(buffer.ToArray()));
                        numbers.Add(new IntElement { Value = numberComma, Line = numberComma.ToString() });
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
                        

                        if (numbers.Count > 0)
                        {
                            currentArray.Elements.AddRange(numbers);
                            numbers.Clear();
                        }
                        currentArray.Elements.Add(newArray);

                        currentArray = newArray;
                    }
                    arrays.Push(currentArray);
                break;
                default:
                    buffer.Add(i);
                    break;
            }
        }

        return startArray;
    }


}