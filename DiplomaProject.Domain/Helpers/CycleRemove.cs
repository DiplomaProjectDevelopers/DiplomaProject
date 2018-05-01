using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiplomaProject.Domain.Helpers
{
    public class CycleRemove
    {
        public static List<Tuple<int,int>> RemoveCycles(int n, List<int> left, List<int> right)
        {
            for (int i = 0, j; i < n; i++)
            {
                for (j = i; j < n; j++)
                {
                    if (right[i] + 1 == left[j] && left[i] == right[j] && left[i] != left[j])
                    {
                        ReplaceNodes(n, i, right[i], left[j], left[i], left, right);
                        right[i] = left[i];
                    }
                }
            }
            var list = new List<Tuple<int, int>>();
            for (int i = 0; i < n; i++)
            {
                var tuple = new Tuple<int, int>(left[i], right[i]);
                if (left[i] != right[i] && !list.Contains(tuple))
                {
                    list.Add(tuple);
                }
            }
            return list;
        }

        private static void ReplaceNodes(int n, int i, int rem1, int rem2, int repl, List<int> left, List<int> right)
        {
            for (int j = 0; j < n; j++)
            {
                if (j != i)
                {
                    if (left[j] == rem1 || left[j] == rem2)
                    {
                        left[j] = repl;
                    }
                    if (right[j] == rem1 || right[j] == rem2)
                    {
                        right[j] = repl;
                    }
                }
            }
        }
    }
}
