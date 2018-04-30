using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.Models
{
    public class DirectedCycle
    {
        private Dictionary<int, bool> _marked;
        private Dictionary<int, int> _edgeTo;
        private Stack<int> _cycle;
        private Dictionary<int,bool> _onStack;

        public DirectedCycle(Digraph G)
        {
            _onStack = new Dictionary<int, bool>(G.VertexCount);
            _edgeTo = new Dictionary<int,int>(G.VertexCount);
            _marked = new Dictionary<int,bool>(G.VertexCount);
            for (int i = 0; i < G.VertexCount; i++)
            {
                if (!_marked[i])
                {
                    DFS(G, i);
                }
            }
        }

        public bool HasCycle()
        {
            return _cycle != null;
        }

        private void DFS(Digraph G, int v)
        {
            _onStack[v] = true;
            _marked[v] = true;
            foreach (int w in G.GetVertex(v))
            {
                if (this.HasCycle()) return;
                else if (!_marked[w])
                {
                    _edgeTo[w] = v;
                    DFS(G, w);
                }
                else if (_onStack[w])
                {
                    _cycle = new Stack<int>();
                    for (int x = v; x != w; x = _edgeTo[x])
                    {
                        _cycle.Push(x);
                    }
                    _cycle.Push(w);
                    _cycle.Push(v);
                }
            }
            _onStack[v] = false;
        }

        public IEnumerable<int> Cycle { get { return _cycle; } }
    }

}
