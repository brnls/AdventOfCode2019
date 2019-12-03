using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Console
{
    class Day1
    {
        private static int[] ReadMass => File.ReadAllLines("Day1/AlliDay1.txt").Select(int.Parse).ToArray();
        public static int PartA()
        {
            return ReadMass.Select(x => x / 3 - 2).Sum();
        }

        public static int PartB()
        {
            return ReadMass.Select(CalculateFuel).Sum();
        }

        private static int CalculateFuel(int mass)
        {
            int sum = 0;
            while (mass > 0)
            {
                var fuel = Math.Max(0, mass / 3 - 2);
                sum += fuel;
                mass = fuel;
            }
            return sum;
        }

    }
}
