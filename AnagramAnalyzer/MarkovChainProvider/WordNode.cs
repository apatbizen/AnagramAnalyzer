using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramAnalyzer.MarkovChainProvider {
    public class WordNode {
        public WordNode(string surface, string kana, IEnumerable<WordNodePath> nexts) {
            Surface = surface;
            Kana = kana;
            Chains = nexts;
        }

        public string Surface { get; }
        public string Kana { get; }
        public IEnumerable<WordNodePath> Chains { get; set; }
    }

    public class WordNodePath {
        public WordNodePath(IEnumerable<WordNode> nexts, int frequency) {
            Nodes = nexts;
            Frequency = frequency;
        }

        public IEnumerable<WordNode> Nodes { get; }
        public int Frequency { get; }
    }
}
