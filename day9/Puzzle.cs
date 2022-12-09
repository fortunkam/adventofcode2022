using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day9
{
    public static class Puzzle
    {
        private static bool AreAdjacent(int aX, int aY, int bX, int bY)
        {
            if (Math.Abs(aX - bX) <= 1 && aY == bY) return true;
            if (Math.Abs(aY - bY) <= 1 && aX == bX) return true;
            if (Math.Abs(aY - bY) <= 1 && Math.Abs(aX - bX) <= 1) return true;
            return false;
        }

        private static bool AreAdjacent(Segment segmentOne, Segment segmentTwo)
        {
            if (Math.Abs(segmentOne.X - segmentTwo.X) <= 1 && segmentOne.Y == segmentTwo.Y) return true;
            if (Math.Abs(segmentOne.Y - segmentTwo.Y) <= 1 && segmentOne.X == segmentTwo.X) return true;
            if (Math.Abs(segmentOne.Y - segmentTwo.Y) <= 1 && Math.Abs(segmentOne.X - segmentTwo.X) <= 1) return true;
            return false;
        }
        
        private static void DrawSegments(List<Segment> segments)
        {
            var grid = new string[6,7];

            for(var i = 0; i < segments.Count ; i++)
            {
                var segment = segments[i];
                if (string.IsNullOrEmpty(grid[segment.Y, segment.X]))
                {
                    grid[segment.X, segment.Y] = i.ToString();
                }
            }

            Console.WriteLine("--------------------------------------");

            for (int y = grid.GetUpperBound(0) - 1; y >= 0; y--)
            {
                for (int x = 0; x < grid.GetUpperBound(1); x++)
                {
                    if (string.IsNullOrWhiteSpace(grid[x, y]))
                    {
                        Console.Write(".");
                    }
                    else
                    {
                        Console.Write(grid[x, y]);
                    }
                }
                Console.WriteLine();
            }
        }
        
        public static void PartOne(string inputFile)
        {
            using var fs = new FileStream(inputFile, FileMode.Open);
            using var sr = new StreamReader(fs);

            

            var commands = new List<Tuple<string,int>>();
            var commandMatch = new Regex(@"(?<dir>[R|U|L|D]) (?<count>\d*)", RegexOptions.Compiled);

            do
            {
                var line = sr.ReadLine();
                var m = commandMatch.Match(line);
                commands.Add(new Tuple<string, int>(m.Groups["dir"].Value, 
                    int.Parse(m.Groups["count"].Value)));              
            }
            while (!sr.EndOfStream);

            var headX = 0;
            var headY = 0;

            var previousHeadX = 0;
            var previousHeadY = 0;

            var tailX = 0;
            var tailY = 0;

            var tailVisited = new Dictionary<string, int>();

            foreach (var command in commands)
            {
                var dir = command.Item1;
                var count = command.Item2;

                for (var i = 0; i < count; i++)
                {
                    previousHeadX = headX;
                    previousHeadY = headY;
                    
                    switch (dir)
                    {
                        case "R":
                            headX++;
                            break;
                        case "U":
                            headY++;
                            break;
                        case "L":
                            headX--;
                            break;
                        case "D":
                            headY--;
                            break;
                    }
                    
                    //Is Tail adjacent?
                    if(!AreAdjacent(headX,headY,tailX,tailY))
                    {
                        tailX = previousHeadX;
                        tailY = previousHeadY;

                        var point = $"{tailX},{tailY}";
                        if (!tailVisited.ContainsKey(point))
                        {
                            tailVisited.Add(point, 1);
                        }
                        else
                        {
                            tailVisited[point]++;
                        }

                    }
                    else
                    {
                        var point = $"{tailX},{tailY}";
                        if (!tailVisited.ContainsKey(point))
                        {
                            tailVisited.Add(point, 1);
                        }
                        else
                        {
                            tailVisited[point]++;
                        }
                    }

                    
                    
                }

                
            }
            foreach (var item in tailVisited)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"tail visitied {tailVisited.Count} points");

        }

        public static void PartTwo(string inputFile)
        {
            using var fs = new FileStream(inputFile, FileMode.Open);
            using var sr = new StreamReader(fs);



            var commands = new List<Tuple<string, int>>();
            var commandMatch = new Regex(@"(?<dir>[R|U|L|D]) (?<count>\d*)", RegexOptions.Compiled);

            do
            {
                var line = sr.ReadLine();
                var m = commandMatch.Match(line);
                commands.Add(new Tuple<string, int>(m.Groups["dir"].Value,
                    int.Parse(m.Groups["count"].Value)));
            }
            while (!sr.EndOfStream);

            var tailVisited = new Dictionary<string, int>
            {
                { "0,0", 1 }
            };


            var segments = new List<Segment>();
            segments.AddRange(Enumerable.Range(0, 10).Select(r => new Segment()));

            foreach (var command in commands)
            {

                var dir = command.Item1;
                var count = command.Item2;

                for (var i = 0; i < count; i++)
                {
                    segments[0].previousX = segments[0].X;
                    segments[0].previousY = segments[0].Y;

                    switch (dir)
                    {
                        case "R":
                            segments[0].X++;
                            break;
                        case "U":
                            segments[0].Y++;
                            break;
                        case "L":
                            segments[0].X--;
                            break;
                        case "D":
                            segments[0].Y--;
                            break;
                    }

                    for(var k = 1; k < segments.Count; k++)
                    {
                        //Is Tail adjacent?
                        if (!AreAdjacent(segments[k - 1], segments[k]))
                        {
                            //Console.WriteLine($"{i} {dir}: segment {k - 1} and {k} are not adjacent");
                            //segments[k].previousX = segments[k].X;
                            //segments[k].previousY = segments[k].Y;
                            //segments[k].X = segments[k - 1].previousX;
                            //segments[k].Y = segments[k - 1].previousY;

                           
                            if (Math.Abs(segments[k - 1].X - segments[k].X) > 1 && segments[k - 1].Y == segments[k].Y)
                            {
                                //Same row
                                
                                segments[k].previousX = segments[k].X;
                                segments[k].X = segments[k - 1].previousX;
                            }
                            else if (Math.Abs(segments[k - 1].Y - segments[k].Y) > 1 && segments[k - 1].X == segments[k].X)
                            {
                                //Same column
                                segments[k].previousY = segments[k].Y;
                                segments[k].Y = segments[k - 1].previousY;
                            }
                            else
                            {
                                //Diagonal

                                segments[k].previousX = segments[k].X;
                                segments[k].previousY = segments[k].Y;

                                if (segments[k].X < segments[k - 1].X)
                                {
                                    segments[k].X += 1;
                                }
                                else
                                {
                                    segments[k].X -= 1;
                                }

                                if (segments[k].Y < segments[k - 1].Y)
                                {
                                    segments[k].Y += 1;
                                }
                                else
                                {
                                    segments[k].Y -= 1;
                                }

                            }

                            if (k == segments.Count - 1)
                            {
                                var point = $"{segments[k].X},{segments[k].Y}";
                                if (!tailVisited.ContainsKey(point))
                                {
                                    tailVisited.Add(point, 1);
                                }
                                else
                                {
                                    tailVisited[point]++;
                                }
                            }
                        }

                        
                    }
                   // DrawSegments(segments);


                }


            }
            //foreach (var item in tailVisited)
            //{
            //    Console.WriteLine(item);
            //}

            Console.WriteLine($"tail visitied {tailVisited.Count} points");

        }
    }
}
