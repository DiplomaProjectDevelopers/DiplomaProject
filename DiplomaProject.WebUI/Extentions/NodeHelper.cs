using DiplomaProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.Extentions
{
    /// <summary>
    /// Helper class for Node 
    /// </summary>
    /// <typeparam name="T">Value of a node</typeparam>
    public static class NodeHelper
    {
        /// <summary>
        /// Converts Directed Acyclic Graph to Tree data structure using recursion.
        /// </summary>
        /// <param name="root">root of DAG</param>
        /// <param name="seenNodes">keep track of child elements to find multiple connections (f.e. A connects with B and C and B also connects with C)</param>
        /// <returns>root node of the tree</returns>
        public static Node<T> DAG2TreeRec<T>(this Node<T> root, HashSet<Node<T>> seenNodes)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            if (seenNodes == null)
            {
                throw new ArgumentNullException("seenNodes");
            }

            var length = root.ChildNodes.Count;
            for (int i = 0; i < length; ++i)
            {
                var node = root.ChildNodes[i];
                if (seenNodes.Contains(node))
                {
                    var nodeClone = new Node<T>(node.Value, node.ChildNodes);
                    node = nodeClone;
                }
                else
                {
                    seenNodes.Add(node);
                }
                DAG2TreeRec(node, seenNodes);
            }
            return root;
        }
        /// <summary>
        /// Converts Directed Acyclic Graph to Tree data structure using explicite stack.
        /// </summary>
        /// <param name="root">root of DAG</param>
        /// <param name="seenNodes">keep track of child elements to find multiple connections (f.e. A connects with B and C and B also connects with C)</param>
        /// <returns>root node of the tree</returns>
        public static Node<T> DAG2Tree<T>(this Node<T> root, HashSet<Node<T>> seenNodes)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            if (seenNodes == null)
            {
                throw new ArgumentNullException("seenNodes");
            }

            var stack = new Stack<Node<T>>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                var tempNode = stack.Pop();
                var length = tempNode.ChildNodes.Count;
                for (int i = 0; i < length; ++i)
                {
                    var node = tempNode.ChildNodes[i];
                    if (seenNodes.Contains(node))
                    {
                        var nodeClone = new Node<T>(node.Value, node.ChildNodes);
                        node = nodeClone;
                    }
                    else
                    {
                        seenNodes.Add(node);
                    }
                    stack.Push(node);
                }
            }
            return root;
        }
    }
}
