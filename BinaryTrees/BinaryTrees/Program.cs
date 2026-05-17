using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrees
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] data = { 50, 30, 70, 20, 40, 60, 80 };

            BinarySearchTree bst = new BinarySearchTree();
            bst.BuildFromArray(data);

            Console.WriteLine("InOrder (отсортировано):");
            bst.InOrder(bst.Root);

            Console.WriteLine("\nPreOrder:");
            bst.PreOrder(bst.Root);

            Console.WriteLine("\nPostOrder:");
            bst.PostOrder(bst.Root);

            Console.WriteLine("\nLevelOrder (BFS):");
            bst.LevelOrder();

            Console.WriteLine("\n\nУдаляем 50:");
            bst.Delete(50);

            Console.WriteLine("InOrder после удаления:");
            bst.InOrder(bst.Root);


        }
    }
}
