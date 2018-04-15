using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.Models
{
    class Tree<T>
    {
        private TreeNode<T> root;

        public Tree(T value)
        {
            
        }
        public Tree(T value, params Tree<T>[] children) : this(value)
        {
            foreach (var child in children)
            {
                this.root.AddChild(child.root);
            }
        }

        public TreeNode<T> Root { get { return root;  } }
    }
}
