using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MarkovChain
{
    private HashSet<MarkovNode> markovNodes
    {
        get;
        set;
    }

        public MarkovChain()
        {
            markovNodes = new HashSet<MarkovNode>();
        }

        public virtual void GenerateChain(string length)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ParseText(string inputText, int nodeLength)
        {
            // Split the text on each line
            var splitLines = inputText.Split();

            // Create a working array to copy into
            string[] workingWords = new string[nodeLength];
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
                catch (ArgumentOutOfRangeException)
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
                markovNodes.Add(new MarkovNode(NodeType.Initiating, splitLines[0], workingWords));

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