using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramAnalyzer {
    public interface IWordItem {
        /// <summary>表示名(漢字交じり)</summary>
        string Surface { get; }

        /// <summary>読みがな</summary>
        string Kana { get; }

        /// <summary>後続となり得るWordItemを検索する</summary>
        IEnumerable<IWordItem> FindNexts();
    }
}
