using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubstringSearchLib;

namespace SubstringSearchExperiments
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var text = "aabaabaaaabaabaaab";
            var pattern = "aabaa";
            var algms = new ISubstringSearch[]
            {
                new KnuthMorrisPrattAlgorithm(),
                new RabinCarpAlgorithm()
            };

            Console.WriteLine("text: " + text);
            Console.WriteLine("pattern: " + pattern);

            foreach (var algm in algms)
            {
                Console.WriteLine("Found substrings at indexes: " + String.Join(" ", algm.Search(text, pattern)));
            }
        }
    }
}
