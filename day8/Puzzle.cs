using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day8
{
    public static class Puzzle
    {

        private static List<List<int>> GetInput(string inputFile)
        {
            using var fs = new FileStream(inputFile, FileMode.Open);
            using var sr = new StreamReader(fs);

            var ret = new List<List<int>>();

            do
            {
                var line = sr.ReadLine();    
                ret.Add(new List<int>(line.ToCharArray().Select(r=>int.Parse(r.ToString()))));
            }
            while(!sr.EndOfStream);

            return ret;
        }

        public static void PartOne(string inputFile)
        {
            var inputData = GetInput(inputFile);

            Dictionary<string,int> visibleCount = new Dictionary<string, int>();

            for(var y=1; y< inputData.Count - 1; y++)
            {

                //Left Row
                for(var x = 1; x< inputData[y].Count -1; x++)
                {
                    // Right
                    var rightPoints = inputData[y].Skip(x + 1).ToArray();
                    if(!rightPoints.Any(r=>r >= inputData[y][x]))
                    {
                        var key = $"{x},{y}";
                        if (!visibleCount.ContainsKey(key))
                        {
                            visibleCount.Add(key, 0);
                        }
                        visibleCount[key]++;
                        Console.WriteLine($"Right: {x},{y}");
                    }

                    // Left
                    var leftPoints = inputData[y].Take(new Range(0,x)).ToArray();
                    if (!leftPoints.Any(r => r >= inputData[y][x]))
                    {
                        var key = $"{x},{y}";
                        if (!visibleCount.ContainsKey(key))
                        {
                            visibleCount.Add(key, 0);
                        }
                        visibleCount[key]++;
                        Console.WriteLine($"Left: {x},{y}");
                    }
                }
               
            }

            for (var x = 1; x < inputData[0].Count - 1; x++)
            {

                for (var y = 1; y < inputData[x].Count - 1; y++)
                {
                    //Top Points
                    var topPoints = Enumerable.Range(0, y).Select(r => inputData[r][x]).ToArray();
                    if (!topPoints.Any(r => r >= inputData[y][x]))
                    {
                        var key = $"{x},{y}";
                        if (!visibleCount.ContainsKey(key))
                        {
                            visibleCount.Add(key, 0);
                        }
                        visibleCount[key]++;
                        Console.WriteLine($"Top: {x},{y}");
                    }

                    //Bottom Points
                    var range = Enumerable.Range(y + 1, inputData.Count - (y + 1)).ToArray();
                    var bottomPoints = range.Select(r => inputData[r][x]).ToArray();
                    if (!bottomPoints.Any(r => r >= inputData[y][x]))
                    {
                        var key = $"{x},{y}";
                        if (!visibleCount.ContainsKey(key))
                        {
                            visibleCount.Add(key, 0);
                        }
                        visibleCount[key]++;
                        Console.WriteLine($"Bottom: {x},{y}");
                    }
                }
            }


            foreach (var val in visibleCount)
            {
                Console.WriteLine($"{val.Key} = {val.Value}");
            }

            var total = visibleCount.Count + (2 * (inputData.Count - 1)) + (2 * (inputData[0].Count - 1));
            Console.WriteLine($"total = {total}");
        }
        
        public static void PartTwo(string inputFile)
        {
            var inputData = GetInput(inputFile);

            Dictionary<string, int> visibleCount = new Dictionary<string, int>();

            for (var y = 1; y < inputData.Count - 1; y++)
            {

                //Left Row
                for (var x = 1; x < inputData[y].Count - 1; x++)
                {
                    var key = $"{x},{y}";

                    // Right
                    var rightPoints = inputData[y].Skip(x + 1).ToArray();
                    var rightTotal = 0;
                    foreach(var p in rightPoints)
                    {
                        rightTotal++;
                        if (p >= inputData[y][x]) break;
                    }

                    if (!visibleCount.ContainsKey(key))
                    {
                        visibleCount.Add(key, rightTotal);
                    }
                    else
                    {
                        visibleCount[key] *= rightTotal;
                    }

                    Console.WriteLine($"Right: {x},{y} = {rightTotal}");

                    // Left
                    var leftPoints = inputData[y].Take(new Range(0, x)).Reverse().ToArray();
                    var leftTotal = 0;

                    foreach (var p in leftPoints)
                    {
                        leftTotal++;
                        if (p >= inputData[y][x]) break;
                    }

                    if (!visibleCount.ContainsKey(key))
                    {
                        visibleCount.Add(key, leftTotal);
                    }
                    else
                    {
                        visibleCount[key] *= leftTotal;
                    }

                    Console.WriteLine($"Left: {x},{y} = {leftTotal}");
                }

            }

            for (var x = 1; x < inputData[0].Count - 1; x++)
            {

                for (var y = 1; y < inputData[x].Count - 1; y++)
                {
                    var key = $"{x},{y}";

                    //Top Points
                    var topPoints = Enumerable.Range(0, y).Select(r => inputData[r][x]).Reverse().ToArray();
                    var topTotal = 0;

                    foreach (var p in topPoints)
                    {
                        topTotal++;
                        if (p >= inputData[y][x]) break;
                    }

                    if (!visibleCount.ContainsKey(key))
                    {
                        visibleCount.Add(key, topTotal);
                    }
                    else
                    {
                        visibleCount[key] *= topTotal;
                    }

                    Console.WriteLine($"Top: {x},{y} = {topTotal}");

                    //Bottom Points
                    var range = Enumerable.Range(y + 1, inputData.Count - (y + 1)).ToArray();
                    var bottomPoints = range.Select(r => inputData[r][x]).ToArray();
                    var bottomTotal = 0;

                    foreach (var p in bottomPoints)
                    {
                        bottomTotal++;
                        if (p >= inputData[y][x]) break;
                    }

                    if (!visibleCount.ContainsKey(key))
                    {
                        visibleCount.Add(key, bottomTotal);
                    }
                    else
                    {
                        visibleCount[key] *= bottomTotal;
                    }

                    Console.WriteLine($"Bottom: {x},{y} = {bottomTotal}");
                }
            }

            foreach (var val in visibleCount)
            {
                Console.WriteLine($"{val.Key} = {val.Value}");
            }

            var maxScore = visibleCount.Max(r => r.Value);
            Console.WriteLine($"maximum score is {maxScore}");
        }
    }
}
