using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnagramAnalyzer.CharRanker {
    public class CharacterRanker : ICharRanker{
        Dictionary<char, int> appearance = new Dictionary<char, int>();
        public void CheckIn(string v) {
            foreach(var p in v.ToCharArray().GroupBy(x => x)) {
                if (!this.appearance.ContainsKey(p.Key)) this.appearance.Add(p.Key, 1);
                else this.appearance[p.Key]++;
            }
        }

        public int PopularityOf(char c) => this.appearance.ContainsKey(c) ? this.appearance[c] : 0;

        public int Compare(char x, char y) {
            var diff = this.PopularityOf(x) - this.PopularityOf(y);
            //if (diff != 0) return diff;
            return y - x;
        }

        public IEnumerable<char> ListOfPopularityOrder => this.appearance.OrderBy(x=>x.Key).OrderByDescending(x => x.Value).Select(x => x.Key);

        public IEnumerable<char> ListOfRarityOrder => this.appearance.OrderBy(x => x.Key).OrderBy(x => x.Value).Select(x => x.Key);

    }
}
