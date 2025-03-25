using System;

[Flags]
public enum Directions
{
    None = 1,
    
    Up = 1 << 1,
    Right = 1 << 2,
    Down = 1 << 3,
    Left = 1 << 4,
    
    Horizontal = Right | Left,
    Vertical = Up | Down,
    All = Up | Right | Down | Left
}