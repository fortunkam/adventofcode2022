enum PlayerMove
{
    Rock,
    Paper,
    Scissors
}

enum GameResult
{
    Win,
    Lose,
    Draw
}

public static class Game
{
    public static void PartOne(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);



        var moveMap = new Dictionary<string, PlayerMove>{
            {"A", PlayerMove.Rock},
            {"B", PlayerMove.Paper},
            {"C", PlayerMove.Scissors},
            {"X", PlayerMove.Rock},
            {"Y", PlayerMove.Paper},
            {"Z", PlayerMove.Scissors}
        };

        var results = new List<Tuple<PlayerMove, PlayerMove, int>>()
        {
            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Rock, PlayerMove.Rock, 4),
            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Rock, PlayerMove.Paper, 8),
            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Rock, PlayerMove.Scissors, 3),

            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Paper, PlayerMove.Rock, 1),
            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Paper, PlayerMove.Paper, 5),
            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Paper, PlayerMove.Scissors, 9),

            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Scissors, PlayerMove.Rock, 7),
            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Scissors, PlayerMove.Paper, 2),
            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Scissors, PlayerMove.Scissors, 6),
        };

        var score = 0;
        var regex = new System.Text.RegularExpressions.Regex("(?<opp>[ABC]) (?<plr>[XYZ])", System.Text.RegularExpressions.RegexOptions.Compiled);
        do
        {
            var line = sr.ReadLine();
            var matches = regex.Match(line);
            var opp = moveMap[matches.Groups["opp"].Value];
            var plr = moveMap[matches.Groups["plr"].Value];



            var result = results.First(r => r.Item1 == opp && r.Item2 == plr).Item3;

           // Console.WriteLine($"Match {opp} vs {plr} score {result}");

            score += result;
        }
        while (!sr.EndOfStream);

        Console.WriteLine(score);
    }

    public static void PartTwo(string inputFile)
    {
        using var fs = new FileStream(inputFile, FileMode.Open);
        using var sr = new StreamReader(fs);

        var moveMap = new Dictionary<string, PlayerMove>{
            {"A", PlayerMove.Rock},
            {"B", PlayerMove.Paper},
            {"C", PlayerMove.Scissors},
        };

        var resultMap = new Dictionary<string, GameResult>{
            {"X", GameResult.Lose},
            {"Y", GameResult.Draw},
            {"Z", GameResult.Win},
        };

        var results = new List<Tuple<PlayerMove, PlayerMove, int>>()
        {
            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Rock, PlayerMove.Rock, 4),
            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Rock, PlayerMove.Paper, 8),
            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Rock, PlayerMove.Scissors, 3),

            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Paper, PlayerMove.Rock, 1),
            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Paper, PlayerMove.Paper, 5),
            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Paper, PlayerMove.Scissors, 9),

            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Scissors, PlayerMove.Rock, 7),
            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Scissors, PlayerMove.Paper, 2),
            new Tuple<PlayerMove, PlayerMove,int>(PlayerMove.Scissors, PlayerMove.Scissors, 6),
        };

        var resultGrid = new List<Tuple<PlayerMove, GameResult, PlayerMove>>()
        {
            new Tuple<PlayerMove, GameResult,PlayerMove>(PlayerMove.Rock, GameResult.Win, PlayerMove.Paper),
            new Tuple<PlayerMove, GameResult,PlayerMove>(PlayerMove.Rock, GameResult.Draw, PlayerMove.Rock),
            new Tuple<PlayerMove, GameResult,PlayerMove>(PlayerMove.Rock, GameResult.Lose, PlayerMove.Scissors),

            new Tuple<PlayerMove, GameResult,PlayerMove>(PlayerMove.Paper, GameResult.Win, PlayerMove.Scissors),
            new Tuple<PlayerMove, GameResult,PlayerMove>(PlayerMove.Paper, GameResult.Draw, PlayerMove.Paper),
            new Tuple<PlayerMove, GameResult,PlayerMove>(PlayerMove.Paper, GameResult.Lose, PlayerMove.Rock),

            new Tuple<PlayerMove, GameResult,PlayerMove>(PlayerMove.Scissors, GameResult.Win, PlayerMove.Rock),
            new Tuple<PlayerMove, GameResult,PlayerMove>(PlayerMove.Scissors, GameResult.Draw, PlayerMove.Scissors),
            new Tuple<PlayerMove, GameResult,PlayerMove>(PlayerMove.Scissors, GameResult.Lose, PlayerMove.Paper),
        };

        var score = 0;
        var regex = new System.Text.RegularExpressions.Regex("(?<opp>[ABC]) (?<plr>[XYZ])", System.Text.RegularExpressions.RegexOptions.Compiled);
        do
        {
            var line = sr.ReadLine();
            var matches = regex.Match(line);
            var opp = moveMap[matches.Groups["opp"].Value];
            var expectedResult = resultMap[matches.Groups["plr"].Value];

            var plr = resultGrid.First(r => r.Item1 == opp && r.Item2 == expectedResult).Item3;
            var result = results.First(r => r.Item1 == opp && r.Item2 == plr).Item3;

           // Console.WriteLine($"Match {opp} vs {plr} score {result}");

            score += result;
        }
        while (!sr.EndOfStream);

        Console.WriteLine(score);
    }
}
