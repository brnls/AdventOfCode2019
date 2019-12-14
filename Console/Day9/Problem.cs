using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Console.Day9
{
    class Problem
    {
        static long[] ReadOpCodes = File.ReadAllText($"Day9/{Program.Config}Day9.txt")
            .Split(',')
            .Select(long.Parse)
            .ToArray();

        public static long PartA()
        {
            var intComputer = new IntCodeComputer(ReadOpCodes.Concat(new long[10000]).ToArray(), 1);
            intComputer.Run();
            return intComputer.Output.Dequeue();
        }

        public static void PartB()
        {

        }
    }

}
