using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
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
            return Paint(0).Count;
        }

        public static void PartB()
        {
            var painted = Paint(1);
            var adjusted = painted.Keys.Select(x => ((x.Item1 + 50), (x.Item2 + 50)));
            var maxWidth = adjusted.Max(x => x.Item1);
            var maxHeight = adjusted.Max(x => x.Item2);
            using (var stream = File.OpenWrite("img.png"))
            using (var image = new Image<Rgba32>(maxWidth, maxHeight))
            {
                for (int i = 0; i < maxWidth; i++)
                    for (int j = 0; j < maxHeight; j++)
                    {
                        image[i, j] = Rgba32.Black;
                    }

                foreach(var panel in painted)
                {
                    image[panel.Key.Item1 + 50, panel.Key.Item2 + 50] = panel.Value == Color.Black ? Rgba32.Black : Rgba32.White;
                }

                image.SaveAsPng(stream);
            }

        }

        public static Dictionary<(int,int), Color> Paint(int input)
        {
            var painted = new Dictionary<(int, int), Color>();
            var computer = new IntCodeComputer(ReadOpCodes, input);
            var (x, y) = (0, 0);
            var currentOrientation = Orientation.Up;
            while (true)
            {
                computer.Run();
                if (computer.Complete) break;

                var color = (Color)computer.Output.Dequeue();
                var direction = (Direction)computer.Output.Dequeue();
                painted[(x, y)] = color;

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

            return painted;
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
