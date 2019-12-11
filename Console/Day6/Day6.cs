using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Console
{
    class Day6
    {
        static (string, string)[] Orbits = File.ReadAllLines($"Day6/{Program.Config}Day6.txt")
            .Select(x =>
            {
                var split = x.Split(')');
                return (split[0], split[1]);
            }).ToArray();

        public static int PartA()
        {
            var childrenByParent = Orbits.ToLookup(x => x.Item1, x => x.Item2);
            return FindDepthSum(childrenByParent, "COM", 0);
        }

        static int FindDepthSum(ILookup<string,string> tree, string currentNode, int currentDepth)
        {
            return currentDepth +
                tree[currentNode].Sum(x => FindDepthSum(tree, x, currentDepth + 1));
        }

        public static int PartB()
        {
            var sanParents = GetParents("SAN");
            var youParents = GetParents("YOU");

            for (int i = 0; i < sanParents.Count; i++)
            {
                for(int j = 0; j < youParents.Count; j++)
                {
                    if (sanParents[i] == youParents[j])
                        return i + j;
                }
            }

            throw new Exception("no intersecting parent found");
        }

        public static List<string> GetParents(string node)
        {
            var temp = node;
            var parents = new List<string>();
            while (true)
            {
                var parent = Orbits.FirstOrDefault(x => x.Item2 == temp).Item1;
                if(parent == null)
                {
                    break;
                }
                else
                {
                    parents.Add(parent);
                    temp = parent;
                }
            }
            return parents;
        }

    }
}
