using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.Models
{
    public class Digraph
    {
        private int _edgeCount;
        private Dictionary<int, List<int>> adj;
        public int EdgeCount { get { return _edgeCount; } }
        public int VertexCount { get; }
        public Digraph(List<int> vertices)
        {
            this._edgeCount = 0;
            this.VertexCount = vertices.Count;
            adj = new Dictionary<int, List<int>>();
            for (int i = 0; i < vertices.Count; i++)
            {
                adj[vertices[i]] = new List<int>();
            }
        }

        public void AddEdge(int v, int w)
        {
            adj[v].Add(w);
            _edgeCount++;
        }

        public IEnumerable<int> GetVertex(int v)
        {
            return adj[v];
        }

    }
}
