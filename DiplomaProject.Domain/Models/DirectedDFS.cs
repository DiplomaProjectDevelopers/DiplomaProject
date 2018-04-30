using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.Models
{
    public class DirectedDFS
    {
        private Dictionary<int, bool> _marked;
        public DirectedDFS(Digraph G, int s)
        {
            _marked = new Dictionary<int, bool>(G.VertexCount);
            DFS(G, s);
        }

        public DirectedDFS(Digraph G, IEnumerable<int> sources)
        {
            _marked = new Dictionary<int, bool>(G.VertexCount);
            foreach (var s in sources)
            {
                if (!_marked[s])
                {
                    DFS(G, s);
                }
            }
        }

        private void DFS(Digraph G, int v)
        {
            _marked[v] = true;
            foreach (var w in G.GetVertex(v))
            {
                if (!_marked[w]) DFS(G, w);
            }
        }

        public bool IsMarked(int v)
        {
            return _marked[v];
        }
    }
}
