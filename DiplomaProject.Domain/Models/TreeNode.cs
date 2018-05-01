using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.Models
{
    /// <summary>
    /// Represeting a node in DAG or Tree
    /// </summary>
    /// <typeparam name="T">Value of the node</typeparam>
    public class Node<T>
    {
        /// <summary>
        /// creats a node with no child nodes
        /// </summary>
        /// <param name="value">Value of the node</param>
        public Node(T value)
        {
            Value = value;
            ChildNodes = new List<Node<T>>();
        }

        /// <summary>
        /// Creates a node with given value and copy the collection of child nodes
        /// </summary>
        /// <param name="value">value of the node</param>
        /// <param name="childNodes">collection of child nodes</param>
        public Node(T value, IEnumerable<Node<T>> childNodes)
        {
            if (childNodes == null)
            {
                throw new ArgumentNullException("childNodes");
            }
            ChildNodes = new List<Node<T>>(childNodes);
            Value = value;
        }

        /// <summary>
        /// Determines if the node has any child node
        /// </summary>
        /// <returns>true if has any</returns>
        public bool HasChildNodes
        {
            get { return this.ChildNodes.Count != 0; }
        }


        /// <summary>
        /// Travearse the Graph recursively
        /// </summary>
        /// <param name="root">root node</param>
        /// <param name="visitor">visitor for each node</param>
        public void Traverse(Node<T> root, Action<Node<T>> visitor)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            if (visitor == null)
            {
                throw new ArgumentNullException("visitor");
            }

            visitor(root);
            foreach (var node in root.ChildNodes)
            {
                Traverse(node, visitor);
            }
        }

        /// <summary>
        /// Value of the node
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// List of all child nodes
        /// </summary>
        public List<Node<T>> ChildNodes { get; private set; }
    }
}
