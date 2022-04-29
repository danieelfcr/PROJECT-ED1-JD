using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    public interface IAVL<T>
    {
        /// <summary>
        /// Use the AVL structure to store nodes of type T
        /// </summary>
       
        public Node<T> Insert(Node<T> root, Node<T> newNode);

        /// <summary>
        /// Gets the height of a specific node stored in the AVL structure
        /// </summary>
        
        public int GetHeight(Node<T> node);
        
        /// <summary>
        /// Gets the maximum value of two integers that represents the height of a 
        /// subtree
        /// </summary>
        
        public int Max(int leftHeight, int reghtHeight);

        /// <summary>
        /// Gets the subtraction of the heights of the left and right subtrees
        /// to modify the balance factor of a specific node
        /// </summary>
        
        public int CalculateBalanceFactor(Node<T> node);

        /// <summary>
        /// Modify the pointers of specific nodes to prevent the balance factor from being
        /// out of allowed range
        /// </summary>
        public Node<T> RightRotation(Node<T> node);

        /// <summary>
        /// Modify the pointers of specific nodes to prevent the balance factor from being
        /// out of allowed range
        /// </summary>
        public Node<T> LeftRotation(Node<T> node);

        /// <summary>
        /// Gets the node that matches with the data given by the user
        /// </summary>
        public Node<T> Search(Node<T> root, Node<T> Data);

        


    }
}
