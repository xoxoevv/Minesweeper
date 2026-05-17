using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrees
{
    internal class Node
    {
        public int Key;      // значение узла
        public Node Left;        // левый потомок
        public Node Right;          // правый потомок

        public Node(int key)
        {
            Key = key;
        }
    }
}
