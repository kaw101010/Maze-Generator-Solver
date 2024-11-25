using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazes
{
    public class UnionFind
    {
        private int[] parent;
        private int[] rank;

        public UnionFind(int size)
        {
            parent = new int[size];
            rank = new int[size];
            for (int i = 0; i < size; i++)
            {
                parent[i] = i;
            }
        }

        public int Find(int a)
        {
            if (parent[a] != a)
                parent[a] = Find(parent[a]); // Path compression
            return parent[a];
        }

        public void Union(int a, int b)
        {
            int rootA = Find(a);
            int rootB = Find(b);

            if (rootA != rootB)
            {
                // Union by rank
                if (rank[rootA] > rank[rootB])
                    parent[rootB] = rootA;
                else if (rank[rootA] < rank[rootB])
                    parent[rootA] = rootB;
                else
                {
                    parent[rootB] = rootA;
                    rank[rootA]++;
                }
            }
        }
    }

}
