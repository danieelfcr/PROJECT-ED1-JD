using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    public class AVL<T> : IAVL<T>
    {
        public Node<T> Root;
        public int count;
        public List<T> NodeList;
        Func<T, T, int> Comparer;

        public AVL(Func<T, T, int> Comparer)
        {
            Root = null;
            count = 0;
            NodeList = new List<T>();
            this.Comparer = Comparer;

        }

        public Node<T> Insert(Node<T> root, Node<T> newNode)
        {
            //Base case
            if (root == null)
            {
                count++;
                return newNode; //retorna el nodo que se quiere insertar
            }

            if (Comparer(root.Record, newNode.Record) == 1)   //If it's lesser, go to left subtree
                root.Left = Insert(root.Left, newNode);
            else if (Comparer(root.Record, newNode.Record) == -1) //If it's greater, go to right subtree
                root.Right = Insert(root.Right, newNode);
            else return root;


            //update of balance factor

            root.Height = 1 + Max(GetHeight(root.Left), GetHeight(root.Right)); //Find height

            root.BalanceFactor = CalculateBalanceFactor(root); //Find balance factor

            if (root.BalanceFactor > 1)
            {
                if (Comparer(root.Left.Record, newNode.Record) == 1) //Single right rotation
                    return RightRotation(root);
                else if (Comparer(root.Left.Record, newNode.Record) == -1) //Double right rotation
                {
                    root.Left = LeftRotation(root.Left);
                    return RightRotation(root);
                }
            }
            if (root.BalanceFactor < -1)
            {
                if (Comparer(root.Right.Record, newNode.Record) == 1) //Single left rotation 
                    return LeftRotation(root);
                else if (Comparer(root.Right.Record, newNode.Record) == 1)  //Double left rotation
                {
                    root.Right = RightRotation(root.Right);
                    return LeftRotation(root);
                }
            }
            return root;
        }

        public int GetHeight(Node<T> node)
        {
            if (node == null)
                return 0;
            return node.Height;

        }

        public int Max(int leftHeight, int rightHeight)
        {
            return (leftHeight > rightHeight) ? leftHeight : rightHeight;
        }

        public int CalculateBalanceFactor(Node<T> node)
        {
            if (node == null)
                return 0;
            return GetHeight(node.Left) - GetHeight(node.Right);
        }


        public Node<T> RightRotation(Node<T> node)
        {
            Node<T> newRoot = node.Left;
            Node<T> rightAux = newRoot.Right;
            newRoot.Right = node;
            node.Left = rightAux;
            node.Height = Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;
            newRoot.Height = Max(GetHeight(newRoot.Left), GetHeight(newRoot.Right)) + 1;

            return newRoot;
        }

        public Node<T> LeftRotation(Node<T> node)
        {
            Node<T> newRoot = node.Right;
            Node<T> leftAux = newRoot.Left;
            newRoot.Left = node;
            node.Right = leftAux;
            node.Height = Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;
            newRoot.Height = Max(GetHeight(newRoot.Left), GetHeight(newRoot.Right)) + 1;

            return newRoot;
        }

        public void InOrder(Node<T> root)
        {
            if (root == null) return;

            InOrder(root.Left);
            NodeList.Add(root.Record);
            InOrder(root.Right);
        }

        public Node<T> Search(Node<T> root, Node<T> Data)
        {
            if (root != null)
            {
                if (Comparer(root.Record, Data.Record) == 0) //Evaluate if they are the same
                {
                    return root;
                }
                else if ((Comparer(root.Record, Data.Record) == 1) && (Root.Left != null))   //Evaluate if it is smaller
                {
                    //If it is, go left
                    return Search(root.Left, Data);
                }
                else if ((Comparer(root.Record, Data.Record) == -1) && (Root.Right != null))
                {
                    //If it isn't, go right
                    return Search(root.Right, Data);
                }
            }
            return Data;
        }
    }
}
