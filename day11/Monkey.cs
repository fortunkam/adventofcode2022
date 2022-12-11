using day11;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

public class Monkey
{
    public Monkey(int index, string[] lines)
    {
        if (lines.Length != 5) throw new ArgumentException("Lines contains wrong number of elements");
        Index = index;
        //Line 0: Starting Items
        var matchItems = startingItemsRegex.Match(lines[0]);
        var items = matchItems.Groups["items"].Value;
        var itemCol = items.Split(',').Select(r => long.Parse(r.Trim()));
        _items.AddRange(itemCol);

        //Line 1: Operation
        Operation = Parsers.Operation(lines[1]);

        //Line 2: Test
        Test = Parsers.Test(lines[2]);

        //Line 3: True Monkey
        var tM = trueMonkeyRegex.Match(lines[3]);
        TrueMonkeyIndex = int.Parse(tM.Groups["val"].Value);

        //Line 4: True Monkey
        var fM = falseMonkeyRegex.Match(lines[4]);
        FalseMonkeyIndex = int.Parse(fM.Groups["val"].Value);

    }

    private Regex startingItemsRegex = new Regex(@"Starting items: (?<items>.*)$", RegexOptions.Compiled);
   
    private Regex trueMonkeyRegex = new Regex(@"If true: throw to monkey (?<val>\d*)", RegexOptions.Compiled);
    private Regex falseMonkeyRegex = new Regex(@"If false: throw to monkey (?<val>\d*)", RegexOptions.Compiled);
    
    private List<long> _items = new List<long>();
    public IEnumerable<long> Items {
        get
        {
            return _items.ToArray();
        }
    }

    public Func<long, long> Operation {get; init;}

    public Func<long, bool> Test {get; init;}

    public int TrueMonkeyIndex {get;init;}

    public int FalseMonkeyIndex {get;init;}

    public int Index { get; init; }

    public override string ToString()
    {
        return $"holding {string.Join(',',_items)}";
    }

    public void AddItem(long item)
    {
        _items.Add(item);
    }

    public long PopItem()
    {
        var i = _items[0];
        _items.RemoveAt(0);
        return i;
    }
}