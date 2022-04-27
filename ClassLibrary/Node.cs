using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    public class Node<T>
    {
        public  T Record { get; set; }
        public Node<T> Left;
        public Node<T> Right;

        public int BalanceFactor { get; set; }
        public int Height { get; set; }

        public Node(T Record)
        {
            this.Record = Record;
            BalanceFactor = 0;
            Height = 1;
        }
    }
}
