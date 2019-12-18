using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Console.Day12
{
    public class Problem
    {
        public static int PartA()
        {
            var input = DeathStar.AlliInput;
            for (int i = 0; i < 1000; i++)
            {
                DeathStar.RunTimeStep(input);
            }
            return input.Sum(i => i.GetTotalEnergy);
        }

        public static void PartB()
        {
            var cycleX = FindCycle(MoonParts.FromMoons(DeathStar.AlliInput, coordinate => coordinate.X));
            System.Console.WriteLine(cycleX);
            var cycleY = FindCycle(MoonParts.FromMoons(DeathStar.AlliInput, coordinate => coordinate.Y));
            System.Console.WriteLine(cycleY);
            var cycleZ = FindCycle(MoonParts.FromMoons(DeathStar.AlliInput, coordinate => coordinate.Z));
            System.Console.WriteLine(cycleZ);
        }

        public static int FindCycle(MoonParts moonParts)
        {
            var visited = new HashSet<MoonParts> { moonParts };
            int stepsTaken = 0;
            while (true)
            {
                moonParts.ApplyGravity();
                moonParts.ApplyVelocity();
                stepsTaken++;
                if(!visited.Add(moonParts)) break;
            }
            return stepsTaken;
        }

    }

    public class Vector
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public int GetTotalValue() => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
    }

    public class DeathStar
    {
        public Vector Velocity { get; set; } = new Vector { X = 0, Y = 0, Z = 0 };
        public Vector Position { get; set; }
        public int GetTotalEnergy => Velocity.GetTotalValue() * Position.GetTotalValue();

        public static void RunTimeStep(DeathStar[] moons)
        {
            for (int i = 0; i < moons.Length; i++)
            {
                for (int j = i+1; j < moons.Length; j++)
                {
                    ApplyGravity(moons[i], moons[j]);
                }
            }

            foreach(var moon in moons)
            {
                ApplyVelocity(moon);
            }
        }

        public static void ApplyGravity(DeathStar a, DeathStar b)
        {
            if (a.Position.X > b.Position.X)
            {
                a.Velocity.X--;
                b.Velocity.X++;
            }
            else if (a.Position.X < b.Position.X)
            {
                a.Velocity.X++;
                b.Velocity.X--;
            }
            
            if (a.Position.Y > b.Position.Y)
            {
                a.Velocity.Y--;
                b.Velocity.Y++;
            }
            else if (a.Position.Y < b.Position.Y)
            {
                a.Velocity.Y++;
                b.Velocity.Y--;
            }

            if (a.Position.Z > b.Position.Z)
            {
                a.Velocity.Z--;
                b.Velocity.Z++;
            }
            else if (a.Position.Z < b.Position.Z)
            {
                a.Velocity.Z++;
                b.Velocity.Z--;
            }
        }

        public static void ApplyVelocity(DeathStar a)
        {
            a.Position.X += a.Velocity.X;
            a.Position.Y += a.Velocity.Y;
            a.Position.Z += a.Velocity.Z;
        }

        public static DeathStar[] BrianInput => new[]
        {
            new DeathStar { Position = new Vector { X = -16, Y = -1, Z = -12 } },
            new DeathStar { Position = new Vector { X = 0, Y = -4, Z = -17 } },
            new DeathStar { Position = new Vector { X = -11, Y = 11, Z = 0 } },
            new DeathStar { Position = new Vector { X = 2, Y = 2, Z = -6 } },
        };

        public static DeathStar[] AlliInput => new[]
        {
            new DeathStar { Position = new Vector { X = -4, Y = -14, Z = 8 } },
            new DeathStar { Position = new Vector { X = 1, Y = -8, Z = 10 } },
            new DeathStar { Position = new Vector { X = -15, Y = 2, Z = 1 } },
            new DeathStar { Position = new Vector { X = -17, Y = -17, Z = 16 } },
        };
        public static DeathStar[] TestInput1 => new[]
        {
            new DeathStar { Position = new Vector { X = -1, Y = 0, Z = 2 } },
            new DeathStar { Position = new Vector { X = 2, Y = -10, Z = -7 } },
            new DeathStar { Position = new Vector { X = 4, Y = -8, Z = 8 } },
            new DeathStar { Position = new Vector { X = 3, Y = 5, Z = -1 } },
        };
        public static DeathStar[] TestInput2 => new[]
        {
            new DeathStar { Position = new Vector { X = -8, Y = -10, Z = 0 } },
            new DeathStar { Position = new Vector { X = 5, Y = 5, Z = 10 } },
            new DeathStar { Position = new Vector { X = 2, Y = -7, Z = 3 } },
            new DeathStar { Position = new Vector { X = 9, Y = -8, Z = -3 } },
        };
    }
}
