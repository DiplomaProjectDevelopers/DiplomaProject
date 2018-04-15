using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaProject.Domain.Models
{
    class TreeNode<T>
    {
        private T value;
        private bool hasParent;
        private List<TreeNode<T>> children;
        public TreeNode(T value)
        {
            this.value = value;
            this.children = new List<TreeNode<T>>();
        }

        public T Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
        public bool HasParent { get; set; }
        public void AddChild(TreeNode<T> child)
        {
            child.hasParent = true;
            this.children.Add(child);
        }

        public TreeNode<T> GetChild(int index)
        {
            return this.children[index];
        }
    }
}
