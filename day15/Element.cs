public abstract class Element
{
    public Element(int x, int y, string drawn, ConsoleColor color)
    {
        X = x;
        Y = y;
        Drawn = drawn;
        Color = color;
    }

    public int X { get; }
    public int Y { get; }
    public string Drawn { get; }
    public ConsoleColor Color { get; }

    public override string ToString()
    {
        return $"{this.GetType().Name}: {X}, {Y}";
    }
}
