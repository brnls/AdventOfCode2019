using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Console
{
    class Day7
    {
        static (string, string)[] Orbits = File.ReadAllLines($"Day6/{Program.Config}Day6.txt")
            .Select(x =>
            {
                var split = x.Split(')');
                return (split[0], split[1]);
            }).ToArray();

        public static void PartA()
        {
            var list = new[] { 1, 2, 3, 4, 5};

            foreach (var p in AllPermutations(list))
            {
                foreach (var val in p)
                {
                    System.Console.WriteLine(val);
                }
                System.Console.WriteLine("---------------------");
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

    }
}
