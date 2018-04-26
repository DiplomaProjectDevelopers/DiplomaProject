using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.Models
{
    public class DepthFirstOrder
    {
        private bool[] _marked;
        private Queue<int> _pre;
        private Queue<int> _post;
        private Stack<int> _reversePost;

        public DepthFirstOrder(Digraph G)
        {
            _marked = new bool[G.VertexCount];
            _pre = new Queue<int>();
            _post = new Queue<int>();
            _reversePost = new Stack<int>();
            for (int v = 0; v < G.VertexCount; v++)
            {
                if (!_marked[v]) DFS(G, v);
            }
        }

        public void DFS(Digraph G, int v)
        {
            _pre.Enqueue(v);
            _marked[v] = true;
            foreach (int w in G.GetVertex(v))
            {
                if (!_marked[w])
                {
                    DFS(G, w);
                }
            }
            _post.Enqueue(v);
            _reversePost.Push(v);
        }

        public IEnumerable<int> Pre { get { return _pre; } }
        public IEnumerable<int> Post { get { return _post; } }

        public IEnumerable<int> ReversePost { get { return _reversePost; } }
    }
}
