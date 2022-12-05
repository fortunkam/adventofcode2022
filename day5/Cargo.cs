using System.Text.RegularExpressions;
public static class Cargo
{
    public static void PartOne(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        var stacks = new List<List<char>>();

        bool processingStacks = true;
        var moves = new Regex(@"move (?<num>\d*) from (?<from>\d*) to (?<to>\d*)", RegexOptions.Compiled);
        do
        {
            
            if(processingStacks)
            {
                var lines = new List<string>();
                var stackLine = sr.ReadLine();
                do
                {
                    //Console.WriteLine($"add line {stackLine}");
                    lines.Add(stackLine);
                    stackLine = sr.ReadLine();
                }
                while(!string.IsNullOrWhiteSpace(stackLine));

                // Last line contains the number of columns

                var last = lines[lines.Count - 1];
                var count = int.Parse(last.Trim().Split(' ').Last());
                for(var j=0; j < count; j++)
                {
                    stacks.Add(new List<char>());
                }

                //Console.WriteLine($"Last line {last} shows a count of {count}");

                var regexStackItem = new Regex(@"(\s{3}|\[(?<m>[A-Z])\])[\s|$]?", RegexOptions.Compiled);
                
                
                for (var i = 0; i < (lines.Count - 1); i++)
                {
                    var curLine = lines[i];
                    var rmatch = regexStackItem.Matches(curLine);
                    for(var j=0; j < count; j++)
                    {
                        var cmatch = rmatch[j];
                        if(!string.IsNullOrWhiteSpace(cmatch.Value))
                        {
                            //Console.WriteLine($"Inserting {cmatch.Groups["m"].Value.ToCharArray().First()} into column {j}");
                            stacks[j].Insert(0,cmatch.Groups["m"].Value.ToCharArray().First());
                        }
                    }

                }

                processingStacks = false;
            }
            else
            {
                var line = sr.ReadLine();
                var match = moves.Match(line);
                if (match.Success)
                {
                    var num = int.Parse(match.Groups["num"].Value);
                    var from = int.Parse(match.Groups["from"].Value) - 1;
                    var to = int.Parse(match.Groups["to"].Value) - 1;

                    for (var i = 0; i < num; i++)
                    {
                        var item = stacks[from].Last();
                        stacks[from].RemoveAt(stacks[from].Count - 1);
                        stacks[to].Add(item);
                    }
                }
            }            
        }
        while(!sr.EndOfStream);
        
        var result = "";
        foreach(var item in stacks)
        {
            result += item.Last().ToString();
        }

        Console.WriteLine(result);

    }

    public static void PartTwo(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        var stacks = new List<List<char>>();

        bool processingStacks = true;
        var moves = new Regex(@"move (?<num>\d*) from (?<from>\d*) to (?<to>\d*)", RegexOptions.Compiled);
        do
        {
            
            if(processingStacks)
            {
                var lines = new List<string>();
                var stackLine = sr.ReadLine();
                do
                {
                    //Console.WriteLine($"add line {stackLine}");
                    lines.Add(stackLine);
                    stackLine = sr.ReadLine();
                }
                while(!string.IsNullOrWhiteSpace(stackLine));

                // Last line contains the number of columns

                var last = lines[lines.Count - 1];
                var count = int.Parse(last.Trim().Split(' ').Last());
                for(var j=0; j < count; j++)
                {
                    stacks.Add(new List<char>());
                }

               // Console.WriteLine($"Last line {last} shows a count of {count}");

                var regexStackItem = new Regex(@"(\s{3}|\[(?<m>[A-Z])\])[\s|$]?", RegexOptions.Compiled);
                
                
                for (var i = 0; i < (lines.Count - 1); i++)
                {
                    var curLine = lines[i];
                    var rmatch = regexStackItem.Matches(curLine);
                    for(var j=0; j < count; j++)
                    {
                        var cmatch = rmatch[j];
                        if(!string.IsNullOrWhiteSpace(cmatch.Value))
                        {
                            //Console.WriteLine($"Inserting {cmatch.Groups["m"].Value.ToCharArray().First()} into column {j}");
                            stacks[j].Insert(0,cmatch.Groups["m"].Value.ToCharArray().First());
                        }
                    }

                }

                processingStacks = false;
            }
            else
            {
                var line = sr.ReadLine();
                var match = moves.Match(line);
                if (match.Success)
                {
                    var num = int.Parse(match.Groups["num"].Value);
                    var from = int.Parse(match.Groups["from"].Value) - 1;
                    var to = int.Parse(match.Groups["to"].Value) - 1;


                    //Grab the last n values 
                    var valsToInsert = stacks[from].Skip(stacks[from].Count - num).Take(num).ToArray();
                    DebugStack(nameof(valsToInsert), valsToInsert);

                    stacks[from].RemoveRange(stacks[from].Count - num, num);

                    stacks[to].AddRange(valsToInsert);
                }
            }            
        }
        while(!sr.EndOfStream);
        
        var result = "";
        foreach(var item in stacks)
        {
            result += item.Last().ToString();
        }

        Console.WriteLine(result);

    }

    private static void DebugStack(string stackidentifier, IEnumerable<char> vals)
    {
        var joined = string.Join(",", vals);
        Console.WriteLine($"{stackidentifier}: {joined}");
    }
}