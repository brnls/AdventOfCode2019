using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Console
{
    class Day3
    {
        static (Vector[], Vector[]) ParseInput()
        {
            var lines = File.ReadAllLines($"Day3/{Program.Config}Day3.txt");
            Vector[] ParseInput(string line) => line
                    .Split(',')
                    .Select(Vector.Parse)
                    .ToArray();

            return (ParseInput(lines[0]), ParseInput(lines[1]));
        }

        public static int PartA()
        {
            var (wire1, wire2) = ParseInput();
            var wire1Set = WireSet.FormWires(wire1);
            var wire2Set = WireSet.FormWires(wire2);
            var intersections = WireSet.GetIntersections(wire1Set, wire2Set);
            intersections.Remove((0, 0, 0));

            int min = Int32.MaxValue;
            foreach(var (x, y, _) in intersections)
            {
                var dist = ManhattanDistance(0, 0, x, y);
                if(dist < min)
                {
                    min = dist;
                }
            }
            return min;
        }

        public static int PartB()
        {
            var (wire1, wire2) = ParseInput();
            var wire1Set = WireSet.FormWires(wire1);
            var wire2Set = WireSet.FormWires(wire2);
            var intersections = WireSet.GetIntersections(wire1Set, wire2Set);
            intersections.Remove((0, 0, 0));
            return intersections.Min(x => x.distance);
        }

        public static int ManhattanDistance(int x1, int y1, int x2, int y2) =>
            Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
    }

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    class Vector
    {
        public Direction Direction { get; set; }
        public int Value { get; set; }
        public static Vector Parse(string str)
        {
            Direction ParseDirection(char c) =>
                c switch
                {
                    'U' => Direction.Up,
                    'D' => Direction.Down,
                    'L' => Direction.Left,
                    'R' => Direction.Right
                };

            return new Vector
            {
                Direction = ParseDirection(str[0]),
                Value = int.Parse(str.AsSpan(1))
            };
        }
    }

    class WireSet
    {
        public List<Line> Horizontal { get; set; }
        public List<Line> Vertical { get; set; }

        public static WireSet FormWires(Vector[] wires)
        {
            int x = 0;
            int y = 0;

            var horizontal = new List<Line>();
            var vertical = new List<Line>();
            int distance = 0;
            foreach (var wire in wires)
            {
                distance = distance + wire.Value;
                switch (wire.Direction)
                {
                    case Direction.Up:
                        vertical.Add(new Line
                        {
                            Start = y,
                            End = y + wire.Value,
                            Position = x,
                            Distance = distance
                        });
                        y = y + wire.Value;
                        break;
                    case Direction.Down:
                        vertical.Add(new Line
                        {
                            Start = y,
                            End = y - wire.Value,
                            Position = x,
                            Distance = distance
                        });
                        y = y - wire.Value;
                        break;
                    case Direction.Right:
                        horizontal.Add(new Line
                        {
                            Start = x,
                            End = x + wire.Value,
                            Position = y,
                            Distance = distance
                        });
                        x = x + wire.Value;
                        break;
                    case Direction.Left:
                        horizontal.Add(new Line
                        {
                            Start = x,
                            End = x - wire.Value,
                            Position = y,
                            Distance = distance
                        });
                        x = x - wire.Value;
                        break;
                }
            }
            return new WireSet
            {
                Horizontal = horizontal,
                Vertical = vertical
            };
        }

        public static List<(int x, int y, int distance)> GetIntersections(WireSet a, WireSet b)
        {
            return GetIntersections(a.Horizontal, b.Vertical).Concat(GetIntersections(a.Vertical, b.Horizontal)).ToList();
        }

        public static List<(int x, int y, int distance)> GetIntersections(List<Line> horizontalLines, List<Line> verticalLines)
        {
            var results = new List<(int, int, int)>();
            foreach(var horizontal in horizontalLines)
            {
                foreach(var vertical in verticalLines)
                {
                    if((Between(horizontal.Start, horizontal.End, vertical.Position) ||
                    Between(horizontal.End, horizontal.Start, vertical.Position)) && 
                    (Between(vertical.Start, vertical.End, horizontal.Position) ||
                    Between(vertical.End, vertical.Start, horizontal.Position)))
                    {
                        var horizontalTotal = horizontal.Distance - Math.Abs(horizontal.End - vertical.Position);
                        var verticalTotal = vertical.Distance - Math.Abs(vertical.End - horizontal.Position);
                        results.Add((vertical.Position, horizontal.Position, horizontalTotal + verticalTotal));
                    }
                }
            }
            return results;
        }

        public static bool Between(int x1, int x2, int value)
        {
            return value >= x1 && value <= x2;
        }
    }

    class Line
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Position { get; set; }
        public int Distance { get; set; }
    }
}
