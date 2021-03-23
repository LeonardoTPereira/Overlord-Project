using System;

public class Coordinates
{
    private int x, y;
    public Coordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }

    public override bool Equals(Object obj)
    {
        if((obj == null) || (! GetType().Equals(obj.GetType())))
            return false;
        Coordinates coordinates = (Coordinates) obj;
        return ((coordinates.x == x) && (coordinates.y == y));
    }

    public override string ToString()
    {
        return $"Coordinates X = {X}, Y = {Y}";
    }

    public override int GetHashCode()
    {
        return x ^ y;
    }
}
