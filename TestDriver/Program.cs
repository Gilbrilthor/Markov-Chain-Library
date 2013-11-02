using MarkovChainLib;
using System;

namespace TestDriver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var testString = "The background generator is intended to remove the creative pressure that can sometimes frustrate players as they try to flesh out the murky details of their characters' backgrounds. This generator provides these details with either a simple roll of the die or the players deliberately selecting from options on the furnished tables and lists.";

            Console.WriteLine("Input String: {0}", testString);

            MarkovChain chain = new MarkovChain();

            chain.ParseText(testString, 3);

            Console.WriteLine(chain.GenerateChain(10));

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}