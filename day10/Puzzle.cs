using System.Text.RegularExpressions;

public enum CommandType
{
    Noop,
    Addx
}
public class Command
{ 
    public CommandType Type { get; set; }
    public int CycleCount { get; set; }
    public int Value { get; set; }
}

public static class CommandFactory
{
    static Regex addMatch = new Regex(@"addx (?<move>-*\d+)", RegexOptions.Compiled);
    
    public static Command Create(string line)
    {
        if (line.StartsWith("noop"))
        {
            return new Command() { Type = CommandType.Noop, CycleCount = 1 };
        }
        else
        {
            var match = addMatch.Match(line);
            return new Command() {  Type = CommandType.Addx, CycleCount = 2, Value = int.Parse(match.Groups["move"].Value) };
        }
    }
}



public static class Puzzle
{
    public static void PartOne(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);
        
        var commands = new List<Command>();

        var signalStrength = new Dictionary<int, int>()
        {
            { 20,0  },
            { 60,0  },
            { 100,0  },
            { 140,0  },
            { 180,0  },
            { 220,0  },
        };
        

        do
        {
            var line = sr.ReadLine();
            commands.Add(CommandFactory.Create(line));
        }
        while(!sr.EndOfStream);

        var totalCyclesRequired = commands.Sum(r => r.CycleCount);
        var xValue = 1;

        var currentCommandIndex = 0;
        var currentCommandCount = 0;

        for (int i = 1; i <= totalCyclesRequired; i++)
        {
           

            Console.WriteLine($"At Cycle {i} x is {xValue}");

            if (signalStrength.ContainsKey(i))
            {
                signalStrength[i] = xValue * i;
            }

            if (currentCommandCount == (commands[currentCommandIndex].CycleCount - 1))
            {
                currentCommandCount = 0;
                xValue += commands[currentCommandIndex].Value;
                currentCommandIndex++;
            }
            else
            {
                currentCommandCount++;
            }
        }

        var totalValue = signalStrength.Sum(r => r.Value);
        Console.WriteLine($"Total Value = {totalValue}");
    }

    public static void PartTwo(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        var commands = new List<Command>();

        do
        {
            var line = sr.ReadLine();
            commands.Add(CommandFactory.Create(line));
        }
        while (!sr.EndOfStream);

        var totalCyclesRequired = commands.Sum(r => r.CycleCount);
        var xValue = 1;

        var currentCommandIndex = 0;
        var currentCommandCount = 0;

        var crt = new string[,]
        {
            { ".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", "." },
            { ".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", "." },
            { ".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", "." },
            { ".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", "." },
            { ".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", "." },
            { ".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", ".",".", "." }
        };

        var crtIndexX = 0;
        var crtIndexY = 0;

        for (int i = 1; i <= totalCyclesRequired; i++)
        {
            var possiblePixelIndexes = new int[] { (i - 1) - (40 * crtIndexY), i - (40 * crtIndexY), (i + 1) - (40 * crtIndexY) };


            if (possiblePixelIndexes.Any(r => r == (xValue + 1)))
            {
                var yPos = crtIndexY;
                var xPos = crtIndexX;
                   
                crt[yPos, xPos] = "#";
            }

            if (currentCommandCount == (commands[currentCommandIndex].CycleCount - 1))
            {
                currentCommandCount = 0;
                xValue += commands[currentCommandIndex].Value;
                currentCommandIndex++;
            }
            else
            {
                currentCommandCount++;
            }

            if (crtIndexX == 39)
            {
                crtIndexX = 0;
                crtIndexY++;
            }
            else
            {
                crtIndexX++;
            }
        }

        var index = 0;
        foreach(var i in crt)
        {
            Console.Write(i);

            if (index != 0 && (index + 1) % 40 == 0)
            {
                Console.WriteLine();
            }
            index++;
        }
    }
}