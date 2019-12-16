using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Console.Day11
{
    public class Problem
    {
        static long[] ReadOpCodes = File.ReadAllText($"Day11/{Program.Config}Day11.txt")
            .Split(',')
            .Select(long.Parse)
            .ToArray();


        public static int PartA()
        {
            var painted = new Dictionary<(int, int), Color>();
            var computer = new IntCodeComputer(ReadOpCodes, 0);
            var (x, y) = (0, 0);
            var currentOrientation = Orientation.Up;
            while (true)
            {
                computer.Run();
                if (computer.Complete) break;

                var color = (Color)computer.Output.Dequeue();
                var direction = (Direction)computer.Output.Dequeue();
                painted.Add((x, y), color);

                if (direction == Direction.Left)
                {
                    if (currentOrientation == Orientation.Up)
                    {
                        x -= 1;
                        currentOrientation = Orientation.Left;
                    }
                    else if (currentOrientation == Orientation.Down)
                    {
                        x += 1;
                        currentOrientation = Orientation.Right;

                    }
                    else if (currentOrientation == Orientation.Left)
                    {
                        y -= 1;
                        currentOrientation = Orientation.Down;

                    }
                    else //orientation right
                    {
                        y += 1;
                        currentOrientation = Orientation.Up;

                    }
                }
                else if (direction == Direction.Right)
                {
                    if (currentOrientation == Orientation.Up)
                    {
                        x += 1;
                        currentOrientation = Orientation.Right;
                    }
                    else if (currentOrientation == Orientation.Down)
                    {
                        x -= 1;
                        currentOrientation = Orientation.Left;
                    }
                    else if (currentOrientation == Orientation.Left)
                    {
                        y += 1;
                        currentOrientation = Orientation.Up;
                    }
                    else // orientation right
                    {
                        y -= 1;
                        currentOrientation = Orientation.Down;
                    }
                }

                if (painted.ContainsKey((x, y)))
                    computer.Input.Enqueue((long)painted[(x, y)]);
                else computer.Input.Enqueue((long)Color.Black);
            }
            return painted.Count;
        }

        public static void PartB()
        {
        }

    }

    public enum Color
    {
        Black = 0,
        White = 1
    }

    public enum Direction
    {
        Left = 0,
        Right = 1
    }

    public enum Orientation
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3
    }
}
