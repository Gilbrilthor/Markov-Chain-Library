using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkovChainLib
{
    public class MarkovChain
    {
        private static Random rand = new Random();

        private HashSet<MarkovNode> markovNodes
        {
            get;
            set;
        }

        public MarkovChain()
        {
            markovNodes = new HashSet<MarkovNode>();
        }

        public virtual string GenerateChain(int length)
        {
            var sb = new StringBuilder();

            // Pick a random initiating node. Should only be one in this implementation
            // For now, just pick the first initiating node
            MarkovNode targetNode = markovNodes.First(node => node.Type == NodeType.Initiating);

            // Counter for length
            int i = 0;

            // Stick the prefix on the queue for the first node
            sb.AppendFormat(" {0}", targetNode.Prefix);

            do
            {
                // Stick the suffix. Prefix is not needed because it is the tail for the previous node
                foreach (var suffix in targetNode.SuffixList)
                {
                    sb.AppendFormat(" {0}", suffix);
                }

                // Get another qualifying node
                targetNode = pickRandomNode(targetNode);
            }
            while (targetNode.Type != NodeType.Terminating && ++i < length);

            return sb.ToString();
        }

        /// <summary>
        /// Picks a random node that meets the requirements.
        /// </summary>
        /// <param name="targetNode">The target node. The tail determines qualification.</param>
        /// <remarks>
        /// A node that meets requirements is a node who's prefix is the same as the targetNode's tail.
        /// </remarks>
        /// <returns>A qualifying node.</returns>
        private MarkovNode pickRandomNode(MarkovNode targetNode)
        {
            // Pick qualifying node
            // Generate list of qualifying nodes
            var qualifyingNodes = (
                from node in markovNodes
                where node.Prefix == targetNode.Tail
                select node);

            // Pick random node. Possible bottleneck due to extension code
            targetNode = qualifyingNodes.ElementAt(rand.Next(qualifyingNodes.Count()));
            return targetNode;
        }

        /// <summary>
        /// Parses the text and breaks it down into markov nodes of length nodeLength.
        /// </summary>
        /// <param name="inputText">The input text to break down.</param>
        /// <param name="nodeLength">Length of the node to create.</param>
        public virtual void ParseText(string inputText, int nodeLength)
        {
            // Split the text on each line
            var splitLines = inputText.Split();

            // Create a working array to copy into
            string[] workingWords = new string[nodeLength - 1];
            // Copy the first suffix from the splitLines
            Array.Copy(splitLines, 1, workingWords, 0, nodeLength - 1);

            // Add the first node to the set of markov nodes
            markovNodes.Add(new MarkovNode(NodeType.Initiating, splitLines[0], workingWords));

            // Clear the array
            ZeroArray<string>(workingWords, "");

            // In groups no larger than the nodelength, create nodes out of the text
            // Increment by 1 to capture successive groups
            for (int i = 1; i < splitLines.Length; i++)
            {
                // Copy the suffix from the splitLines
                try
                {
                    Array.Copy(splitLines, i + 1, workingWords, 0, nodeLength - 1);
                }
                catch (ArgumentException)
                {
                    int j = 0;
                    // Fill the array as much as we can
                    while (i + 1 + j < splitLines.Length)
                    {
                        workingWords[j] = splitLines[i + 1 + j];

                        j++;
                    }
                }

                // Add a new node to the set of markov nodes
                markovNodes.Add(new MarkovNode(NodeType.Normal, splitLines[i], workingWords));

                // Clear the array
                ZeroArray<string>(workingWords, "");
            }
        }

        private void ZeroArray<T1>(T1[] array, T1 value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }
    }
}