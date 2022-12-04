var inputFile = "./real.txt";

using var fs = new FileStream(inputFile, FileMode.Open);
using var sr = new StreamReader(fs);

var currentElfTotal = 0;
var elfTotals = new List<int>();

while(!sr.EndOfStream)
{
    var line = sr.ReadLine();
    if(string.IsNullOrWhiteSpace(line))
    {
        elfTotals.Add(currentElfTotal);
        currentElfTotal = 0;
    }
    else
    {
        currentElfTotal += int.Parse(line);
    }
}
if(currentElfTotal != 0){
    elfTotals.Add(currentElfTotal);
}

Console.WriteLine(elfTotals.Max());

var orderedElfTotals = elfTotals.OrderByDescending(r=>r).Take(3).Sum();

Console.WriteLine(orderedElfTotals);
