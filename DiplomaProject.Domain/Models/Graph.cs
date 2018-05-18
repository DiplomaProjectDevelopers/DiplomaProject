using AutoMapper;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Helpers;
using DiplomaProject.Domain.Interfaces;
using DiplomaProject.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

public class Graph
{
    private readonly IDPService service;
    private readonly IMapper mapper;
    public Graph(IDPService service, IMapper mapper)
    {
        this.service = service;
        this.mapper = mapper;
    }
    private int vertexCount; // No. of vertices
    private LinkedList<int>[] adjacencyList; //Adjacency List

    //Constructor
    public Graph(int v)
    {
        vertexCount = v;
        adjacencyList = new LinkedList<int>[v];
        for (int i = 0; i < v; ++i)
        {
            adjacencyList[i] = new LinkedList<int>();
        }
    }

    //Function to add an edge into the graph
    public virtual void AddEdge(int v, int w)
    {
        adjacencyList[v].AddLast(w);
    }

    public virtual bool ContainsEdge(int v, int w)
    {
        return adjacencyList[v].Contains(w);
    }

    public virtual List<Tuple<int, int>> GetEdges()
    {
        var list = new List<Tuple<int, int>>();
        for (int i = 0; i < adjacencyList.Length; i++)
        {
            for (int j = 0; j < adjacencyList[i].Count; j++)
            {
                list.Add(new Tuple<int, int>(i, adjacencyList[i].ElementAt(j)));
            }
        }
        return list;
    }

    public virtual void RemoveEdge(int v, int w)
    {
        adjacencyList[v].Remove(w);
    }

    // A recursive function to print DFS starting from v
    private List<int> DFSUtil(int v, bool[] visited)
    {
        // Mark the current node as visited and print it
        visited[v] = true;
        var list = new List<int>
        {
            v
        };
        //Console.Write(v + " ");

        int n;

        // Recur for all the vertices adjacent to this vertex
        IEnumerator<int> i = adjacencyList[v].GetEnumerator();
        while (i.MoveNext())
        {
            n = i.Current;
            if (!visited[n])
            {
                list.AddRange(DFSUtil(n, visited));
            }
        }
        return list;
    }

    // Function that returns reverse (or transpose) of this graph
    private  Graph Transpose
    {
        get
        {
            Graph g = new Graph(vertexCount);
            for (int v = 0; v < vertexCount; v++)
            {
                // Recur for all the vertices adjacent to this vertex
                //JAVA TO C# CONVERTER WARNING: Unlike Java's ListIterator, enumerators in .NET do not allow altering the collection:
                IEnumerator<int> i = adjacencyList[v].GetEnumerator();
                while (i.MoveNext())
                {
                    g.adjacencyList[i.Current].AddLast(v);
                }
            }
            return g;
        }
    }

    private void FillOrder(int v, bool[] visited, System.Collections.Stack stack)
    {
        // Mark the current node as visited and print it
        visited[v] = true;

        // Recur for all the vertices adjacent to this vertex
        IEnumerator<int> i = adjacencyList[v].GetEnumerator();
        while (i.MoveNext())
        {
            int n = i.Current;
            if (!visited[n])
            {
                FillOrder(n, visited, stack);
            }
        }

        // All vertices reachable from v are processed by now,
        // push v to Stack
        stack.Push(new int?(v));
    }

    // The main function that finds and prints all strongly
    // connected components
    public virtual List<List<int>> GetSubgraphs()
    {
        System.Collections.Stack stack = new System.Collections.Stack();
        var subGraphs = new List<List<int>>();
        // Mark all the vertices as not visited (For first DFS)
        bool[] visited = new bool[vertexCount];
        for (int i = 0; i < vertexCount; i++)
        {
            visited[i] = false;
        }

        // Fill vertices in stack according to their finishing
        // times
        for (int i = 0; i < vertexCount; i++)
        {
            if (visited[i] == false)
            {
                FillOrder(i, visited, stack);
            }
        }

        // Create a reversed graph
        Graph gr = Transpose;

        // Mark all the vertices as not visited (For second DFS)
        for (int i = 0; i < vertexCount; i++)
        {
            visited[i] = false;
        }

        // Now process all vertices in order defined by Stack
        while (stack.Count > 0)
        {
            // Pop a vertex from stack
            int v = (int)stack.Pop();

            // Print Strongly connected component of the popped vertex
            if (visited[v] == false)
            {
                subGraphs.Add(gr.DFSUtil(v, visited));
            }
        }
        return subGraphs;
    }

    // A BFS based function to check whether d is reachable from s.
    public bool IsReachable(int s, int d)
    {
        // Base case
        if (s == d)
        {
            return true;
        }

        // Mark all the vertices as not visited
        bool[] visited = new bool[vertexCount];
        for (int i = 0; i < vertexCount; i++)
        {
            visited[i] = false;
        }

        // Create a queue for BFS
        LinkedList<int> queue = new LinkedList<int>();

        // Mark the current node as visited and enqueue it
        visited[s] = true;
        queue.AddLast(s);

        // it will be used to get all adjacent vertices of a vertex
        LinkedList<int>.Enumerator enumerator;

        while (queue.Count > 0)
        {
            // Dequeue a vertex from queue and print it
            s = queue.First.Value;
            queue.RemoveFirst();

            // Get all adjacent vertices of the dequeued vertex s
            // If a adjacent has not been visited, then mark it visited
            // and enqueue it
            for (enumerator = adjacencyList[s].GetEnumerator(); enumerator.MoveNext();)
            {
                // If this adjacent node is the destination node, then
                // return true
                if (enumerator.Current == d)
                {
                    return true;
                }

                // Else, continue to do BFS
                if (!visited[enumerator.Current])
                {
                    visited[enumerator.Current] = true;
                    queue.AddLast(enumerator.Current);
                }
            }
        }

        // If BFS is complete without visiting d
        return false;
    }

    public List<List<int>> GetSubjectSequence(int professionId)
    {
        var finaloutcomes = service.GetAll<FinalOutCome>().Select(o => mapper.Map<FinalOutCome>(o)).ToList();
        var subjects = service.GetAll<Subject>().Where(s => s.ProfessionId == professionId).Select(s => mapper.Map<SubjectViewModel>(s)).ToList();
        var edges = service.GetAll<Edge>().Where(e => e.ProfessionId == professionId).Select(s => mapper.Map<EdgeViewModel>(s)).ToList();
        List<int> left = new List<int>() { 1, 2, 2, 3, 3, 4 };
        List<int> right = new List<int>() { 2, 3, 4, 1, 5, 5 };
        foreach (var edge in edges)
        {
            var s1 = finaloutcomes.Find(o => o.Id == edge.FromNode).SubjectId;
            var s2 = finaloutcomes.Find(o => o.Id == edge.ToNode).SubjectId;
            if (s1.HasValue && s2.HasValue && s1 != s2)
            {
                subjects.Find(s => s.Id == s1).DependentSubjects.Add(s2.Value);
                left.Add(s1.Value);
                right.Add(s2.Value);
            }
        }

        var subjectEdges = CycleRemove.RemoveCycles(left.Count, left, right);
        var graph = new Graph(subjects.Count);
        foreach (var edge in subjectEdges)
        {
            var idx1 = subjects.FindIndex(s => s.Id == edge.Item1);
            var idx2 = subjects.FindIndex(s => s.Id == edge.Item2);
            graph.AddEdge(idx1, idx2);
        }
        foreach (var edge in subjectEdges)
        {
            var idx1 = subjects.FindIndex(s => s.Id == edge.Item1);
            var idx2 = subjects.FindIndex(s => s.Id == edge.Item2);
            graph.RemoveEdge(idx1, idx2);
            if (!graph.IsReachable(idx1, idx2))
            {
                graph.AddEdge(idx1, idx2);
            }
        }
        var edg = graph.GetEdges();
        var filteredEdges = new List<Tuple<int, int>>();
        for (int i = 0; i < edg.Count; i++)
        {
            var s1 = subjects[edg[i].Item1].Id;
            var s2 = subjects[edg[i].Item2].Id;
            filteredEdges.Add(new Tuple<int, int>(s1, s2));
        }
        var subjectLevels = new List<List<int>>(9);
        var rootNodes = subjects.Select(s => s.Id).Where(i => filteredEdges.All(e => e.Item2 != i)).ToList();
        subjectLevels[0].AddRange(rootNodes);
        for (int i = 1; i < subjectLevels.Count - 1; i++)
        {
            var prevoious = subjectLevels[i - 1];
            var currunt = subjects.Select(s => s.Id).Where(s => filteredEdges.Any(e => e.Item2 == s && prevoious.Contains(e.Item1)));
            subjectLevels[i] = currunt.ToList();
        }
        subjectLevels[8] = subjects.Select(s => s.Id).Where(s => subjectLevels.Any(l => l.Any(v => v == s))).ToList();
        return subjectLevels;
    }

}