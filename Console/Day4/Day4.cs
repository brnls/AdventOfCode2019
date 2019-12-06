using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Console
{
    class Day4
    {
        static int[] PwRanges = File.ReadAllText($"Day4/{Program.Config}Day4.txt")
            .Split('-')
            .Select(int.Parse)
            .ToArray();

        public static int PartA()
        {
            int numPasswords = 0;
            for (int i = PwRanges[0]; i < PwRanges[1]; i++)
            {
                var num = i.ToString();
                if (ContainsDouble(num) && IncreasingOrder(num)) numPasswords += 1;
            }
            return numPasswords;
        }

        public static bool ContainsDouble(string num)
        {
            int[] numArr = new int[10];
            for (int i = 0; i < num.Length-1; i++)
            {
                if (num[i] == num[i + 1])
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IncreasingOrder(string num)
        {
            for (int i = 1; i < num.Length; i++)
            {
                if (num[i] < num[i - 1]) return false;
            }
            return true;
        }
        public static bool ContainsModifiedDouble(string num)
        {
            int[] numArr = new int[10];
            for (int i = 0; i < num.Length-1; i++)
            {
                if (num[i] == num[i + 1])
                {
                    int index = (int)Char.GetNumericValue(num[i]);
                    numArr[index] += 1;
                }
            }
            return numArr.Contains(1);
        }

        public static int PartB()
        {
            int numPasswords = 0;
            for (int i = PwRanges[0]; i < PwRanges[1]; i++)
            {
                var num = i.ToString();
                if (ContainsModifiedDouble(num) && IncreasingOrder(num)) numPasswords += 1;
            }
            return numPasswords;
        }

    }
}

