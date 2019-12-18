using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using MathNet.Numerics;

namespace Console.Day13
{
    public class Problem
    {

        static long[] ReadOpCodes = File.ReadAllText($"Day13/{Program.Config}Day13.txt")
            .Split(',')
            .Select(long.Parse)
            .ToArray();

        public static int PartA()
        {
            var comp = new IntCodeComputer(ReadOpCodes);
            comp.Run();
            return 1;
        }

        public static void PartB()
        {
        }
    }

}
