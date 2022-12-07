using System.Text.RegularExpressions;
public static class Puzzle
{
    public static void PartOne(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        var folderSize = new Dictionary<string,int>();
        var fileSizeMatch = new Regex(@"(?<size>\d*)\s(?<file>.*)$", RegexOptions.Compiled);
        var currentDirStack = new List<string>();
        do
        {
            var line = sr.ReadLine();

            if(line.StartsWith("$ cd"))
            {
                var folder = line.Substring(4).Trim();
               // Console.WriteLine($"found folder {line.Substring(4)}");

                if(folder == "..")
                {
                    currentDirStack.RemoveAt(currentDirStack.Count - 1);

                }
                else
                {
                    if(folder == "/")
                    {
                        currentDirStack.Clear();
                    }
                    currentDirStack.Add(folder);
                    
                }
            }
            else if(line.StartsWith("$ ls"))
            {

            }
            else if (line.StartsWith("dir"))
            {

            }
            else //Is a file
            {
                var size = int.Parse(fileSizeMatch.Match(line).Groups["size"].Value);
                var file = fileSizeMatch.Match(line).Groups["file"].Value;
                var currentDir = string.Join('/',currentDirStack);
                Console.WriteLine($"Adding file {file} to folder {currentDir} (size: {size})");
                if(!folderSize.ContainsKey(currentDir))
                {
                    folderSize.Add(currentDir,size);
                }
                else
                {
                    folderSize[currentDir] += size;
                }

                if(!string.IsNullOrWhiteSpace(currentDir) && currentDirStack.Count > 1)
                {
                    for (var i = 0; i < currentDirStack.Count - 1; i++)
                    {
                        var revisedFolder = string.Join('/', currentDirStack.Take(i+1));
                        Console.WriteLine($"Adding file {file} to Parent folder {revisedFolder} (size: {size})");

                        if (!folderSize.ContainsKey(revisedFolder))
                        {
                            folderSize.Add(revisedFolder, size);
                        }
                        else
                        {
                            folderSize[revisedFolder] += size;
                        }
                    }
                }
            }
            
        }
        while(!sr.EndOfStream);

        foreach(var f in folderSize)
        {
            Console.WriteLine($"Folder {f.Key} contains {f.Value}");
        }

        
        var sumSmallFolders = folderSize
            .Where(r=>r.Value <= 100000)
            .OrderBy(r=>r.Value)
            .Sum(r=>r.Value);
        Console.WriteLine(sumSmallFolders);
    }

    public static void PartTwo(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs); 
        var folderSize = new Dictionary<string, int>();
        var fileSizeMatch = new Regex(@"(?<size>\d*)\s(?<file>.*)$", RegexOptions.Compiled);
        var currentDirStack = new List<string>();
        do
        {
            var line = sr.ReadLine();

            if (line.StartsWith("$ cd"))
            {
                var folder = line.Substring(4).Trim();
                // Console.WriteLine($"found folder {line.Substring(4)}");

                if (folder == "..")
                {
                    currentDirStack.RemoveAt(currentDirStack.Count - 1);

                }
                else
                {
                    if (folder == "/")
                    {
                        currentDirStack.Clear();
                    }
                    currentDirStack.Add(folder);

                }
            }
            else if (line.StartsWith("$ ls"))
            {

            }
            else if (line.StartsWith("dir"))
            {

            }
            else //Is a file
            {
                var size = int.Parse(fileSizeMatch.Match(line).Groups["size"].Value);
                var file = fileSizeMatch.Match(line).Groups["file"].Value;
                var currentDir = string.Join('/', currentDirStack);
                Console.WriteLine($"Adding file {file} to folder {currentDir} (size: {size})");
                if (!folderSize.ContainsKey(currentDir))
                {
                    folderSize.Add(currentDir, size);
                }
                else
                {
                    folderSize[currentDir] += size;
                }

                if (!string.IsNullOrWhiteSpace(currentDir) && currentDirStack.Count > 1)
                {
                    for (var i = 0; i < currentDirStack.Count - 1; i++)
                    {
                        var revisedFolder = string.Join('/', currentDirStack.Take(i + 1));
                        Console.WriteLine($"Adding file {file} to Parent folder {revisedFolder} (size: {size})");

                        if (!folderSize.ContainsKey(revisedFolder))
                        {
                            folderSize.Add(revisedFolder, size);
                        }
                        else
                        {
                            folderSize[revisedFolder] += size;
                        }
                    }
                }
            }

        }
        while (!sr.EndOfStream);

        foreach (var f in folderSize)
        {
            Console.WriteLine($"Folder {f.Key} contains {f.Value}");
        }

        var freeSpace = 70000000 - folderSize["/"];
        var requiredSpace = 30000000 - freeSpace;



        var sumSmallFolders = folderSize
            .Where(r => r.Value > requiredSpace)
            .OrderBy(r => r.Value)
            .First().Value;
        Console.WriteLine(sumSmallFolders);
    }
}