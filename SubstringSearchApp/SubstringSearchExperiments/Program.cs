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
            var algm = new KnuthMorrisPrattAlgorithm();
            Console.WriteLine(String.Join(" ", algm.Search("aabaabaaaabaabaaab", "aabaa")));
        }
    }
}
