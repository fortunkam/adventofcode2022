public static class Puzzle
{
    public static void PartOne(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        var monkeyIndex = 0;
        var monkeys = new List<Monkey>();
        do
        {
            var line = sr.ReadLine();
            if(line.StartsWith("Monkey"))
            {
                var lines = new List<string>();
                lines.Add(sr.ReadLine());
                lines.Add(sr.ReadLine());
                lines.Add(sr.ReadLine());
                lines.Add(sr.ReadLine());
                lines.Add(sr.ReadLine());

                monkeys.Add(new Monkey(monkeyIndex, lines.ToArray()));
                monkeyIndex++;
            }
        }
        while (!sr.EndOfStream);

        var inspectCount = Enumerable.Range(0,monkeys.Count)
            .Select(r=> new {k=r,v=0}).ToDictionary(x=>x.k,x=>x.v);

        for (int z = 0; z < 20; z++)
        {
            foreach (var m in monkeys)
            {
                while (m.Items.Count() > 0)
                {
                    long i = m.PopItem();
                    long currentWorryLevel = i;
                    currentWorryLevel = m.Operation(currentWorryLevel);
                    currentWorryLevel = (int)Math.Floor(currentWorryLevel / 3.0);
                    //Console.WriteLine($"worry level for item {i} is currently {currentWorryLevel}");
                    if (m.Test(currentWorryLevel))
                    {
                        monkeys[m.TrueMonkeyIndex].AddItem(currentWorryLevel);
                    }
                    else
                    {
                        monkeys[m.FalseMonkeyIndex].AddItem(currentWorryLevel);
                    }
                    inspectCount[m.Index]++;
                }
            }
        }

        foreach(var m in monkeys)
        {
            Console.WriteLine(m.ToString());
        }

        foreach(var i in inspectCount)
        {
            Console.WriteLine($"monkey {i.Key} inspected {i.Value}");
        }

        var result = inspectCount.Select(r => r.Value).OrderByDescending(r=>r).Take(2).ToArray();
        Console.WriteLine((long)result[0] * (long)result[1]);
    }

    public static void PartTwo(string inputFile)
    {
        
    }
}