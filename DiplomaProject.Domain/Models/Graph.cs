using System;
using System.Collections.Generic;


public class Graph
{
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

    // A recursive function to print DFS starting from v
    internal virtual List<int> DFSUtil(int v, bool[] visited)
    {
        // Mark the current node as visited and print it
        visited[v] = true;
        var list = new List<int>();
        list.Add(v);
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
    internal virtual Graph Transpose
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

    internal virtual void fillOrder(int v, bool[] visited, System.Collections.Stack stack)
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
                fillOrder(n, visited, stack);
            }
        }

        // All vertices reachable from v are processed by now,
        // push v to Stack
        stack.Push(new int?(v));
    }

    // The main function that finds and prints all strongly
    // connected components
    public virtual List<List<int>> PrintSCCs()
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
                fillOrder(i, visited, stack);
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
}