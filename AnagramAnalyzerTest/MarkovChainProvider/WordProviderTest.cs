using AnagramAnalyzer.MarkovChainProvider;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnagramAnalyzerTest.MarkovChainProvider {
    public class WordProviderTest {

        [Test]
        public void 生成とプロパティ() {
            var obj = new WordProvider();
            Assert.AreEqual(0, obj.Count);
        }

        [Test]
        public void Method_LoadAll() {
            var obj = new WordProvider();
            obj.LoadAll("./words.tsv");
            Assert.AreEqual(5, obj.Count);
        }

        [Test]
        public void Method_TakeById_SingleNode() {
            var obj = new WordProvider();
            obj.LoadAll("./words.tsv");
            IEnumerable<WordNode> nodes = obj.TakeById(1);
            Assert.AreEqual(1, nodes.Count());
            Assert.AreEqual("で", nodes.First().Surface);
        }


        [Test]
        public void Method_TakeById_DoubleNode() {
            var obj = new WordProvider();
            obj.LoadAll("./words.tsv");
            IEnumerable<WordNode> nodes = obj.TakeById(0);
            Assert.AreEqual(2, nodes.Count());
            Assert.AreEqual("うえの", nodes.First().Kana);
            Assert.AreEqual("こうずけ", nodes.Last().Kana);
            Assert.AreEqual("上野", nodes.First().Surface);
            Assert.AreEqual("上野", nodes.Last().Surface);
        }

        [Test]
        public void Method_TakeById_Nexts() {
            var obj = new WordProvider();
            obj.LoadAll("./words.tsv");
            IEnumerable<WordNode> nodes = obj.TakeById(0);
            Assert.AreEqual(2, nodes.Count());
            Assert.AreEqual(2, nodes.First().Chains.Count());
            Assert.AreEqual(1, nodes.First().Chains.First().Nodes.Count());
            Assert.AreEqual("で", nodes.First().Chains.First().Nodes.First().Kana);
            Assert.AreEqual("に", nodes.First().Chains.Last().Nodes.First().Kana);
        }

        [Test]
        public void Method_Exhibit() {
            var obj = new WordProvider();
            obj.LoadAll("./words.tsv");
            var result = obj.Exhibit();
        }

    }
}
