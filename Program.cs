using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Node<string>> nodeList = new List<Node<string>>{
                //new Node<string>
                //{
                //    Value = "K",
                //    Frequency = 6
                //},
                //new Node<string>
                //{
                //    Value = "E",
                //    Frequency = 4
                //},
                //new Node<string>
                //{
                //    Value = "I",
                //    Frequency = 5
                //},
                //new Node<string>
                //{
                //    Value = "R",
                //    Frequency = 2
                //},

                #region nodes
                 new Node<string>
                {
                    Value = "1",
                    Frequency = 5
                },
                new Node<string>
                {
                    Value = "2",
                    Frequency = 7
                },
                new Node<string>
                {
                    Value = "3",
                    Frequency = 10
                },
                new Node<string>
                {
                    Value = "4",
                    Frequency = 15
                },
                 new Node<string>
                {
                    Value = "5",
                    Frequency = 20
                },
                  new Node<string>
                {
                    Value = "6",
                    Frequency = 45
                },
                #endregion
            };

            HuffmannTree<string> huffmannTree = new HuffmannTree<string>();
            Node<string> root = huffmannTree.CreateTree(nodeList);
            huffmannTree.CreateKeyCodesFromTree(root);

           huffmannTree.PrintDictionary();
        }
    }

    class Node<T> : IComparable
    {
        public Node<T> Parent { get; set; }
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }
        public T Value { get; set; }
        public int Frequency { get; set; }
        public byte KeyCode { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;  // this is greater than obj

            if (!(obj is Node<T>)) throw new ArgumentException("Object is not a Node!");

            Node<T> other = (Node<T>)obj;
            return this.Frequency.CompareTo(other.Frequency);
        }

        // override object.Equals
        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Node<T> other = obj as Node<T>;
            return this.Value.Equals(other.Value);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    class HuffmannTree<T>
    {
        public Dictionary<string, string> dictionary = new Dictionary<string, string>();
        public Node<T> CreateTree(List<Node<T>> list)
        {
            if (list.Count == 0 || list == null)
            {
                throw new ArgumentNullException("The list is empty or null!");
            }

            Node<T> root;

            while (list.Count > 1)
            {
                int rootFreq;
                list.Sort();

                rootFreq = list[0].Frequency + list[1].Frequency;
                root = new Node<T>()
                {
                    Frequency = rootFreq,
                    Left = list[0],
                    Right = list[1],
                    KeyCode = 2     // to note that this is not a valid keycode yet! (it's maybe the tree root) 
                };

                list[0].Parent = root;
                list[1].Parent = root;
                list[0].KeyCode = 0;
                list[1].KeyCode = 1;

                list.RemoveAt(0);
                list.RemoveAt(0);
                list.Add(root);
            }
            return list[0];
        }


        /// <summary>
        /// Creates the dictionary for the keycodes, and prints the tree nodes in traversal order.
        /// Uses depth first search.
        /// </summary>
        public void CreateKeyCodesFromTree(Node<T> rootNode)
        {
            dictionary.Clear();
            if (rootNode == null)
            {
                Console.WriteLine("There isn't any node!");
                return;
            }
            Console.WriteLine("VALUE : FREQUENCY");


            Stack<byte> pathCode = new Stack<byte>();
            Stack<Node<T>> stack = new Stack<Node<T>>();
            stack.Push(rootNode);

            while (stack.Count > 0)
            {
                Node<T> current = stack.Pop();
                if (current.KeyCode != 2)
                {
                    pathCode.Push(current.KeyCode);
                }
                if (current.Value != null)
                {
                    string Value = ListToString(pathCode.ToList());
                    dictionary.Add(current.Value.ToString(), Value);
                }
                string currentValue = current.Value != null ? current.Value.ToString() : "*";
                Console.WriteLine($"{currentValue} : {current.Frequency}");

                if (current.Right != null)
                {
                    stack.Push(current.Right);
                }
                if (current.Left != null)
                {
                    stack.Push(current.Left);
                }
                if (current.Left == null && current.Right == null && stack.Count > 0)
                {
                    Node<T> nextNode = stack.Peek();
                    while (current != nextNode.Parent)
                    {
                        pathCode.Pop();
                        current = current.Parent;
                    }
                }
            }
        }

        public void PrintDictionary()
        {
            Console.WriteLine("CHAR = BYTECODE");
            foreach (var item in dictionary)
            {
                Console.WriteLine($"{item.Key} = {item.Value}");
            }
        }

        public static string ListToString(List<Byte> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException();
            }

            list.Reverse(); // Because the stack is in reversed order!
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append(item.ToString());
            }
            return sb.ToString();
        }

    }
}
