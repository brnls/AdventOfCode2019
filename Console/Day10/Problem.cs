using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Console.Day10
{
    public class Problem
    {
        static Status [][] ReadOpCodes = File.ReadAllLines($"Day10/{Program.Config}Day10.txt")
            .Select(s => s.Select(c => c == '.' ? Status.Empty : Status.Asteroid).ToArray())
            .ToArray();

        public static IEnumerable<((int,int), IEnumerable<IGrouping<Slope, Slope>>)> CompareAsteroids()
        {
            for (int i = 0; i < ReadOpCodes.Length; i++)
            {
                for(int j = 0; j < ReadOpCodes[i].Length; j++)
                {
                    if (ReadOpCodes[i][j] == Status.Empty) continue;

                    var slopes = new List<Slope>();
                    for (int k = 0; k < ReadOpCodes.Length; k++)
                    {
                        for (int l = 0; l < ReadOpCodes[k].Length; l++)
                        {
                            if (k == i && l == j) continue;
                            if(ReadOpCodes[k][l] == Status.Asteroid)
                            {
                                slopes.Add(CalculateSlope((j, i), (l, k)));
                            }
                        }
                    }
                    yield return ((j,i), slopes.GroupBy(x => x, new SlopeEqualityComparer()));
                }
            }
        }

        public static int PartA()
        {
            return CompareAsteroids()
                .OrderByDescending(x => x.Item2.Count())
                .First().Item2
                .Count();
        }

        public static int PartB()
        {
            var (coordinates, slopes) = CompareAsteroids()
                .OrderByDescending(x => x.Item2.Count())
                .First();

            var orderedSlopes = slopes.OrderBy(g => g.Key, new ClockwiseSlopeComparer()).Select(g => g.ToList());
            int slopeCount = 0;
            while (slopeCount < 200)
            {
                foreach (List<Slope> slope in orderedSlopes)
                {

                    if (slope.Count > 0)
                    {
                        var closest = slope.OrderBy(x =>
                            CalculateDistance(coordinates, (x.B + coordinates.Item1, x.A + coordinates.Item2)))
                            .First();
                        var removed = (closest.B + coordinates.Item1, closest.A + coordinates.Item2);

                        slope.Remove(closest);

                        slopeCount += 1;
                        if (slopeCount == 200)
                        {
                            return removed.Item1 * 100 + removed.Item2;
                        }
                    }
                }
            }

            throw new Exception($"Could not find enough asteroids, got to {slopeCount}");
        }

        public static Slope CalculateSlope((int x, int y) a, (int x, int y) b)
        {
            return new Slope { A = b.y - a.y, B = b.x - a.x };
        }

        public static double CalculateDistance((int x, int y) a, (int x, int y) b)
        {
            return Math.Sqrt((b.y-a.y)^2 + (b.x-a.x)^2);
        }
    }

    public class Slope
    {
        public int A { get; set; }
        public int B { get; set; }
        public float Ratio => (float)A / B;
        public bool SameDirection(Slope other) => (A >= 0 == other.A >= 0) && (B >= 0 == other.B >= 0);
    }

    public class SlopeEqualityComparer : IEqualityComparer<Slope>
    {
        public bool Equals( Slope x,  Slope y)
        {
            return x.A * y.B == x.B * y.A && SameDirection(x, y);
        }

        public bool SameDirection(Slope x, Slope y) => (x.A >= 0 == y.A >= 0) && (x.B >= 0 == y.B >= 0);

        public int GetHashCode(Slope x) => (int)((float) x.A / x.B * 1000);
    }

    public class ClockwiseSlopeComparer : Comparer<Slope>
    {
        public override int Compare(Slope x, Slope y)
        {
            var thisQuadrant = GetQuadrant(x);
            var otherQuadrant = GetQuadrant(y);
            if (thisQuadrant != otherQuadrant) return thisQuadrant.CompareTo(otherQuadrant);

            return x.Ratio.CompareTo(y.Ratio);
        }

        public static int GetQuadrant(Slope slope)
        {
            if (slope.A < 0 && slope.B >= 0) return 1;
            else if (slope.A >= 0 && slope.B > 0) return 2;
            else if (slope.A > 0 && slope.B <= 0) return 3;
            else return 4;
        }
    }

    public enum Status
    {
        Empty,
        Asteroid
    }

}
