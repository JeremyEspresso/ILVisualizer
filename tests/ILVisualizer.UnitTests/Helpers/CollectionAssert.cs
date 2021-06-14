using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.UnitTests.Helpers
{
    public static class CollectionAssert
    {
        public static void Equal<T>(IList<T> first, IList<T> second)
        {
            if (first.Count != second.Count) throw new Exception($"CollectionAssert.Equal failed: First collection was {first.Count} big while second collection was {second.Count} big.");

            for (int i = 0; i < first.Count; i++)
            {
                if (first[i] == null)
                {
                    if (second[i] != null)
                        ThrowNoMatch(i);
                }
                else
                {
                    if (!first[i]!.Equals(second[i]))
                        ThrowNoMatch(i);
                }
            }

            static void ThrowNoMatch(int i)
            {
                throw new Exception($"CollectionAssert.Equal failed: Index {i} doesn't match!");
            }
        }
    }
}
