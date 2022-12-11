using day11;
using static System.Net.Mime.MediaTypeNames;

var inputFile = args[0];

//Puzzle.PartOne(inputFile);
Puzzle.PartTwo(inputFile);

//var test = "  Operation: new = old / old";
//var lmb = Parsers.Operation(test);

//Console.WriteLine(lmb(10));

//var tests = new[]
//{
//    "Test: divisible by 23",
//    "Test: divisible by 19",
//    "Test: divisible by 13",
//    "Test: divisible by 17"
//};


//foreach (var item in tests)
//{
//    var t = Parsers.Test(item);

//    for(var i = 0; i < 100; i++)
//    {
//        Console.WriteLine($"test: \"{item}\" for value {i} is {t((ulong)i)}");
//    }
//}