using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnagramAnalyzer.CharRanker {
    public class HiraganaRanker : ICharRanker{

        private const int CODE_BASE = 0x3041;
        private const int CHARS_COUNT = 84;

        private const char HYPHEN = 'ー';

        private int[] nodes = new int[CHARS_COUNT];
        private int nobashiCount = 0;
        
        public void CheckIn(string v) {
            for(int i=0; i<nodes.Length; i++) {
                if (v.Any(x => x - CODE_BASE == i)) this.nodes[i]++;
            }
            //伸ばし棒だけコードが離れているため別にカウント
            if (v.Any(x => x == HYPHEN)) nobashiCount++;
        }

        public int PopularityOf(char c) {
            var idx = c - CODE_BASE;
            if (0 <= idx & idx < CHARS_COUNT) return this.nodes[idx];
            if (c == 0x30FC) return this.nobashiCount;

            throw new ArgumentOutOfRangeException("ひらがな以外の文字が指定されました。");
        }

        public int Compare(char x, char y) {
            var diff = this.PopularityOf(x) - this.PopularityOf(y);
            if (diff != 0) return diff;
            return y - x;
        }

        public IEnumerable<char> ListOfPopularityOrder => this.nodes.Select((x, i) => ((char)(CODE_BASE + i), x)).OrderByDescending(x => x.Item2).Select(x => x.Item1);

        public IEnumerable<char> ListOfRarityOrder => this.nodes.Select((x, i) => ((char)(CODE_BASE + i), x)).OrderBy(x=>x.Item2).Select(x=>x.Item1);

    }
}
