using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;

namespace TestWPF
{
    public class Node
    {
        public char Symbol { get; set; }
        public int Frequency { get; set; }
        public Node Right { get; set; }
        public Node Left { get; set; }

        public List<bool> Traverse(char symbol, List<bool> data)
        {
            if (Right == null && Left == null)
            {
                if (symbol.Equals(Symbol))
                {
                    return data;
                }
                return null;
            }
            List<bool> left = null;
            List<bool> right = null;

            if (Left != null)
            {
                var leftPath = new List<bool>();
                leftPath.AddRange(data);
                leftPath.Add(false);

                left = Left.Traverse(symbol, leftPath);
            }

            if (Right != null)
            {
                var rightPath = new List<bool>();
                rightPath.AddRange(data);
                rightPath.Add(true);
                right = Right.Traverse(symbol, rightPath);
            }

            if (left != null)
            {
                return left;
            }
            return right;
        }
    }

    public class HuffmanTree
    {
        private List<Node> nodes = new List<Node>();
        public Node Root { get; set; }
        public Dictionary<char, int> Frequencies = new Dictionary<char, int>();

        public void Build(string source)
        {
            foreach (char t in source)
            {
                if (!Frequencies.ContainsKey(t))
                {
                    Frequencies.Add(t, 0);
                }

                Frequencies[t]++;
            }

            foreach (KeyValuePair<char, int> symbol in Frequencies)
            {
                nodes.Add(new Node { Symbol = symbol.Key, Frequency = symbol.Value });
            }

            while (nodes.Count > 1)
            {
                var orderedNodes = nodes.OrderBy(node => node.Frequency).ToList();

                if (orderedNodes.Count >= 2)
                {
                    var taken = orderedNodes.Take(2).ToList();

                    var parent = new Node
                    {
                        Symbol = '*',
                        Frequency = taken[0].Frequency + taken[1].Frequency,
                        Left = taken[0],
                        Right = taken[1]
                    };

                    nodes.Remove(taken[0]);
                    nodes.Remove(taken[1]);
                    nodes.Add(parent);
                }

                Root = nodes.FirstOrDefault();

            }
        }

        public BitArray Encode(string source)
        {
            var encodedSource = new List<bool>();

            foreach (char c in source)
            {
                List<bool> encodedSymbol = Root.Traverse(c, new List<bool>());
                encodedSource.AddRange(encodedSymbol);
            }

            var bits = new BitArray(encodedSource.ToArray());

            return bits;
        }

        public string Decode(BitArray bits)
        {
            Node current = Root;
            string decoded = "";

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (current.Right != null)
                    {
                        current = current.Right;
                    }
                }
                else
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                    }
                }

                if (IsLeaf(current))
                {
                    decoded += current.Symbol;
                    current = Root;
                }
            }

            return decoded;
        }

        public bool IsLeaf(Node node)
        {
            return (node.Left == null && node.Right == null);
        }

//#region save_tree
        
//        public static void SaveTree(TreeView tree, string filename)
//        {
//            using (Stream file = File.Open(filename, FileMode.Create))
//            {
//                BinaryFormatter bf = new BinaryFormatter();
//                bf.Serialize(file, tree.Nodes.Cast<TreeNode>().ToList());
//            }
//        }
        
//        public static void LoadTree(TreeView tree, string filename)
//        {
//            using (Stream file = File.Open(filename, FileMode.Open))
//            {
//                BinaryFormatter bf = new BinaryFormatter();
//                object obj = bf.Deserialize(file);

//                TreeNode[] nodeList = (obj as IEnumerable<TreeNode>).ToArray();
//                tree.Nodes.AddRange(nodeList);
//            }
//        }

//#endregion
    }
}



// statystyka H - jest związana z występującym plikiem, liczona jest entropia i wiadomo ile średnio można zakodować bitów/znak - najlepsze możliwe
// ststystyka L - zwiazane jest ściśle z plikiem