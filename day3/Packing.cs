public static class Packing
{
    public static void PartOne(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        var sum = 0;
        do
        {
            var line = sr.ReadLine();
            var len = line.Length;
            var halfLen = len / 2;
            var lhs = line.Substring(0, halfLen);
            var rhs = line.Substring(halfLen, halfLen);

            // Console.WriteLine($"line = {lhs} {rhs}");

            var lhsArr = lhs.ToCharArray();
            var rhsArr = rhs.ToCharArray();

            var intersect = lhsArr.Intersect(rhsArr);

            foreach (var i in intersect)
            {
                var priority = (int)i;
                if (i >= (int)'a')
                {
                    priority = i - ((int)'a') + 1;
                }
                else
                {
                    priority = i - ((int)'A') + 27;
                }
                //Console.Write($"intesect on {i} value {priority}");
                sum += priority;
            }
            //Console.WriteLine();

        }
        while (!sr.EndOfStream);

        Console.WriteLine(sum);
    }

    public static void PartTwo(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        var sum = 0;
        do
        {
            var line1 = sr.ReadLine().ToCharArray();
            var line2 = sr.ReadLine().ToCharArray();
            var line3 = sr.ReadLine().ToCharArray();

            var intersect12 = line1.Intersect(line2);
            var intersect123 = intersect12.Intersect(line3);

            foreach (var i in intersect123)
            {
                var priority = (int)i;
                if (i >= (int)'a')
                {
                    priority = i - ((int)'a') + 1;
                }
                else
                {
                    priority = i - ((int)'A') + 27;
                }
                Console.Write($"intesect on {i} value {priority}");
                sum += priority;
            }
            Console.WriteLine();

            
        }
        while (!sr.EndOfStream);

        Console.WriteLine(sum);
    }
}