using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrees
{
    internal class BinarySearchTree
    {

        public Node Root;   // корень дерева


        //  Добавление узла
        public void Insert(int key)
        {
            Root = InsertRec(Root, key);
        }

        private Node InsertRec(Node root, int key)
        {
            if (root == null)
                return new Node(key);

            if (key < root.Key)
                root.Left = InsertRec(root.Left, key);
            else if (key > root.Key)
                root.Right = InsertRec(root.Right, key);

            return root;
        }


        //   Удаление узла
        public void Delete(int key)
        {
            Root = DeleteRec(Root, key);
        }

        private Node DeleteRec(Node root, int key)
        {
            if (root == null) return null;

            if (key < root.Key)
                root.Left = DeleteRec(root.Left, key);
            else if (key > Root.Key)
                root.Right = DeleteRec(root.Right, key);
            else
            {
                //  0 или 1 потомок
                if (root.Left == null) return root.Right;
                if (root.Right == null) return root.Left;


                //  2 потомка
                Node min = FindMin(root.Right);
                root.Key = min.Key;
                root.Right = DeleteRec(root.Right, min.Key);
            }

            return root;
        }

        private Node FindMin(Node node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }

        //  ОБХОДЫ В ГЛУБИНУ (BFS)


        //  Префиксный

        public void PreOrder(Node root)
        {
            if (root == null) return;
            Console.Write(root.Key + " ");
            PreOrder(root.Left);
            PreOrder(root.Right);
        }

        //   Инфискный  

        public void InOrder(Node root)
        {
            if (root == null) return;
            InOrder(root.Left);
            Console.Write(root.Key + " ");
            InOrder(root.Right);
        }

        //  Постфиксный
        public void PostOrder(Node root)
        {
            if (root == null) return;
            PostOrder(root.Left);
            PostOrder(root.Right);
            Console.Write(root.Key + " ");
        }


        //   ОБХОДЫ В ШИРИНУ (BFS)

        public void LevelOrder()
        {
            if (Root == null) return;

            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(Root);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();
                Console.Write(current.Key + " ");

                if (current.Left != null)
                    queue.Enqueue(current.Left);

                if (current.Right != null)
                    queue.Enqueue(current.Right);
            }
        }


        //  Построение из массива

        public void BuildFromArray(int[] arr)
        {
            foreach(var i in arr)
            {
                Insert(i);
            }
        }

    }
}
