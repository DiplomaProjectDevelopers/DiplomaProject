using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.Models
{
    public class GraphModel
    {
        public List<Node> Nodes { get; set; } = new List<Node>();

        public List<Edge> Edges { get; set; } = new List<Edge>();


        public class Node
        {
            public int Id { get; set; }

            public string Value { get; set; }
        }

        public class Edge
        {
            public int Id { get; set; }
            public int FromNode { get; set; }
            public int ToNode { get; set; }
        }
    }
}
