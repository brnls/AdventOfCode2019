using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Console.Day8
{
    class Problem
    {
        static int[] Digits = File.ReadAllText($"Day8/{Program.Config}Day8.txt")
            .Select(c => int.Parse(c.ToString()))
            .ToArray();

        public static int PartA()
        {
            int[] counts = null;
            int minZero = int.MaxValue;
            int[] minZeroCounts = new[] { 0, 0, 0 };
            for (int i = 0; i < Digits.Length; i += 250)
            {
                counts = new[] { 0, 0, 0 };
                for (int j = i; j < i + 250; j++)
                {
                    switch (Digits[j])
                    {
                        case 0:
                            counts[0]++;
                            break;
                        case 1:
                            counts[1]++;
                            break;
                        case 2:
                            counts[2]++;
                            break;
                        default:
                            throw new System.Exception($"Unhandled value {Digits[j]}");
                    }
                }
                if (counts[0] < minZero)
                {
                    minZero = counts[0];
                    minZeroCounts[0] = counts[0];
                    minZeroCounts[1] = counts[1];
                    minZeroCounts[2] = counts[2];
                }
            }

            return minZeroCounts[1] * minZeroCounts[2];
        }
    }
}
