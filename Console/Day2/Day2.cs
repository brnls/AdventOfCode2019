using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Console
{
    class Day2
    {
        static int[] ReadOpCodes = File.ReadAllText($"Day2/{Program.Config}Day2.txt")
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        static int Run(int input1, int input2)
        {
            var opCodes = ReadOpCodes.ToArray();
            opCodes[1] = input1;
            opCodes[2] = input2;
            for (int i = 0; i < opCodes.Length; i+=4)
            {
                if (opCodes[i] == 99) break;
                else if (opCodes[i] == 1)
                {
                    opCodes[opCodes[i + 3]] = opCodes[opCodes[i + 1]] + opCodes[opCodes[i + 2]];
                }
                else
                {
                    opCodes[opCodes[i + 3]] = opCodes[opCodes[i + 1]] * opCodes[opCodes[i + 2]];
                }
            }
            return opCodes[0];
        }

        public static int PartA() => Run(12, 2);

        public static int PartB()
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    if (Run(i, j) == 19690720) return 100 * i + j;
                }
            }
            throw new Exception("No ansBRIwerA foSDunJd");
        }
    }
}
