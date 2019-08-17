using AnagramAnalyzer.CharRanker;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnagramAnalyzer {

    public class AnagramDecoder<T> where T:IWordItem {
        #region 【フィールド】
        private readonly CharacterRanker ranker = new CharacterRanker();

        /// <summary>単語オブジェクトとそれに対応するUnorderChars</summary>
        private Dictionary<T, UnorderChars> sortedWords;

        private List<T> headWords = new List<T>();

        #endregion

        #region 【コンストラクタ】
        /// <summary>コンストラクタ</summary>
        /// <param name="words">単語オブジェクト群</param>
        /// <param name="getCharsFunction">かな表記の文字列を単語オブジェクトから得るための関数</param>
        public AnagramDecoder(IEnumerable<T> words) {
            words.Select(x=>x.Kana).ToList().ForEach(this.ranker.CheckIn);
            this.sortedWords = new Dictionary<T, UnorderChars>();
            foreach (var word in words) {
                this.WordCount++;
                this.sortedWords.Add(word, new UnorderChars(word.Kana, this.ranker));

                headWords.RemoveAll(x => word.FindNexts().Contains(x));
                headWords.Add(word);
            }
        }

        /// <summary>登録されている単語数</summary>
        public int WordCount { get; private set; } = 0;

        /// <summary>検索を開始する単語数</summary>
        public int HeadCount => this.headWords.Count();
        #endregion

        #region 【メソッド】Analyze アナグラムとして指定した文章を作ることのできる単語の組み合わせを列挙する
        /// <summary>
        /// アナグラムとして指定した文章を作ることのできる単語の組み合わせを列挙する
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public IEnumerable<IEnumerable<T>> Analyze(string v) {
            if (string.IsNullOrEmpty(v)) return Enumerable.Empty<IEnumerable<T>>();

            var target = new UnorderChars(v, this.ranker);
            return this.headWords.SelectMany(x=>this.Analyze(target, x));
        }

        private IEnumerable<IEnumerable<T>> Analyze(UnorderChars chars, T rootItem) {
            var uoc = sortedWords[rootItem];
            if (!chars.IsInclude(uoc)) return Enumerable.Empty<IEnumerable<T>>();

            var myselfArray = new[] { rootItem };
            var subtracted = chars.Subtract(uoc);
            if (subtracted.IsEmpty) return new List<IEnumerable<T>> { myselfArray };

            return rootItem.FindNexts().SelectMany(x => this.Analyze(chars.Subtract(uoc), (T)x))
                                 .Select(x=>myselfArray.Concat(x));
        }
        #endregion
    }
}