﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Console.Day7
{
    class Problem
    {
        static int[] ReadOpCodes = File.ReadAllText($"Day7/{Program.Config}Day7.txt")
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        public static int PartA()
        {
            var list = new[] { 0, 1, 2, 3, 4, };
            return AllPermutations(list)
                .Max(x => RunAmplifiers(x.ToArray()));
        }
        public static int PartB()
        {
            var list = new[] { 5,6,7,8,9 };
            return AllPermutations(list)
                .Max(x => RunAmplifiers(x.ToArray()));
        }

        static int RunAmplifiers(int[] amplifiers)
        {
            int output = 0;
            int index = 0;
            IntCodeComputer[] computers = amplifiers
                .Select(i => new IntCodeComputer(instructions:ReadOpCodes.ToArray(), initialInput: i)).ToArray();

            while(true)
            {
                computers[index].Input.Enqueue(output);
                var result = computers[index].Run();
                if (computers[index].Complete) return computers[^1].LastOutput;
                output = computers[index].Output.Dequeue();
                index = (index + 1) % amplifiers.Length;
            }
        }

        static IEnumerable<IEnumerable<T>> AllPermutations<T>(T[] list)
        {
            IEnumerable<IEnumerable<T>> allPermutations = new T[][] { new T[] { } };
            int i = 0;
            while (i < list.Length)
            {
                int j = i;
                allPermutations = allPermutations.SelectMany(x => PermutationsFrom(list[j], x));
                i++;
            }
            return allPermutations;
        }

        static IEnumerable<IEnumerable<T>> PermutationsFrom<T>(T t, IEnumerable<T> list)
        {
            int i = 0;
            var enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return InsertAtIndex(list, t, i);
                i++;
            }

            yield return InsertAtIndex(list, t, i);
        }

        static IEnumerable<T> InsertAtIndex<T>(IEnumerable<T> list, T t, int index)
        {
            var enumerator = list.GetEnumerator();
            int i = 0;
            var added = false;
            while (enumerator.MoveNext())
            {
                if (i == index)
                {
                    yield return t;
                    yield return enumerator.Current;
                    added = true;
                    break;
                }
                else yield return enumerator.Current;
                i++;
            }

            if (index >= i && !added)
            {
                yield return t;
            }

            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }

        class State
        {
            public int InstructionOffset { get; set; }
            public int[] Memory { get; set; }
        }
    }
}
