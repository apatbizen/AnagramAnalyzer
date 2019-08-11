using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramAnalyzer {
    interface ICharRanker : IComparer<char> {
        IEnumerable<char> ListOfPopularityOrder { get; }
        IEnumerable<char> ListOfRarityOrder { get; }
        void CheckIn(string v);
        int PopularityOf(char c);
    }
}
