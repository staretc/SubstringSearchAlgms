using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubstringSearchLib;

namespace SubstringSearchTests
{
    [TestClass]
    public class UnitTest1
    {
        List<ISubstringSearch> algms = new List<ISubstringSearch>()
        {
            new BruteForceAlgorithm(),
            new RabinCarpAlgorithm(),
            new KnuthMorrisPrattAlgorithm(),
            new BoyerMooreAlgorithm()
        };
        [TestMethod]
        public void Search_RepeatingChars_ShouldFindAllSubstrings()
        {
            string text = "aaaaaaaaaa"; //10
            string pattern = "aa";
            var expected = Enumerable.Range(0, 9).ToList();
            foreach (var algm in algms)
            {
                var actual = algm.Search(text, pattern);
                CollectionAssert.AreEqual(expected, actual);
            }
        }
        [TestMethod]
        public void Search_String_ShouldFindAllSubstrings_ComparingWithBruteForce()
        {
            string text;
            using (var sr = new StreamReader("../../../anna.txt", Encoding.UTF8))
            {
                text = sr.ReadToEnd().ToLower();
            }

            int number = 100;
            Regex rg = new Regex(@"\w+");
            var bag = new HashSet<string>();
            var matches = rg.Matches(text);
            foreach (var match in matches)
            {
                bag.Add(match.ToString());
                if (bag.Count > number) break;
            }
            foreach (var pattern in bag)
            {
                var BF = new BruteForceAlgorithm();
                var expected = BF.Search(pattern, text);
                foreach (var algm in algms)
                {
                    var actual = algm.Search(pattern, text);
                    CollectionAssert.AreEqual(expected, actual);
                }
            }
        }
        [TestMethod]
        public void Search_PatternGreaterThanText_ShouldFindNothing()
        {
            string pattern = "aaaaaaaaaa"; //10
            string text = "aa";
            var expected = new List<int>() { -1 };
            foreach (var algm in algms)
            {
                var actual = algm.Search(text, pattern);
                CollectionAssert.AreEqual(expected, actual);
            }
        }
        [TestMethod]
        public void Search_EmptyText_ShouldFindNothing()
        {
            string text = "";
            string pattern = "aa";
            var expected = new List<int>() { -1 };
            foreach (var algm in algms)
            {
                var actual = algm.Search(text, pattern);
                CollectionAssert.AreEqual(expected, actual);
            }
        }
        [TestMethod]
        public void Search_EmptyPattern_ShouldFindNothing()
        {
            string text = "aaaaaaaaaa"; //10
            string pattern = "";
            var expected = new List<int>() { -1 };
            foreach (var algm in algms)
            {
                var actual = algm.Search(text, pattern);
                CollectionAssert.AreEqual(expected, actual);
            }
        }
        [TestMethod]
        public void Search_TextDoesnContainPаttern_ShouldFindNothing()
        {
            string text = "aaaaaaaaaa"; //10
            string pattern = "bb";
            var expected = new List<int>() { -1 };
            foreach (var algm in algms)
            {
                var actual = algm.Search(text, pattern);
                CollectionAssert.AreEqual(expected, actual);
            }
        }
        [TestMethod]
        public void Search_RepeatingString_ShouldFindAllSubstrings()
        {
            int count = 100;
            string pattern = "aabbc";
            string text = String.Concat(Enumerable.Repeat(pattern, 100));
            var expected = Enumerable.Range(0, pattern.Length * count).Where((item) => item % pattern.Length == 0).ToList();
            foreach (var algm in algms)
            {
                var actual = algm.Search(text, pattern);
                CollectionAssert.AreEqual(expected, actual);
            }
        }
    }
}
