using AnagramAnalyzer.CharRanker;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnagramAnalyzer {

    public class AnagramDecoder<T> {
        #region 【フィールド】

        private readonly Func<T, string> getStringFunction;

        private readonly CharacterRanker ranker = new CharacterRanker();

        //private readonly Dictionary<UnorderChars, int> failsBorder = new Dictionary<UnorderChars, int>();
        private readonly Dictionary<UnorderChars, int> failsBorder = new Dictionary<UnorderChars, int>();

        /// <summary>単語オブジェクトとそれに対応するUnorderChars</summary>
        private WordNode<T>[] sortedWords;

        #endregion

        #region 【コンストラクタ】
        /// <summary>コンストラクタ</summary>
        /// <param name="words">単語オブジェクト群</param>
        /// <param name="getCharsFunction">かな表記の文字列を単語オブジェクトから得るための関数</param>
        public AnagramDecoder(IEnumerable<T> words, Func<T, string> getCharsFunction) {
            this.getStringFunction = getCharsFunction;
            words.Select(getCharsFunction).ToList().ForEach(this.ranker.CheckIn);
            this.sortedWords = words.Select(x => new WordNode<T>(x, new UnorderChars(getCharsFunction(x), this.ranker)))
                                    .OrderBy(x => x.Chars.PrimaryChar, this.ranker)
                                    .ToArray();
        }
        #endregion

        #region 【メソッド】Analyze アナグラムとして指定した文章を作ることのできる単語の組み合わせを列挙する
        /// <summary>
        /// アナグラムとして指定した文章を作ることのできる単語の組み合わせを列挙する
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public IEnumerable<IEnumerable<T>> Analyze(string v) {
            if (string.IsNullOrEmpty(v)) return Enumerable.Empty<IEnumerable<T>>();

            //集計処理と並べ替え
            var target = new UnorderChars(v, this.ranker);
            return this.Analyze(target, Enumerable.Empty<T>(), 0);
        }

        private IEnumerable<IEnumerable<T>> Analyze(UnorderChars chars, IEnumerable<T> currentInput, int skipCount) {
            if (chars.IsEmpty) {
                yield return currentInput;
                yield break;
            }

#if SHOW
            Console.WriteLine($"採用：{string.Join(" + ", currentInput.Select(x => this.getStringFunction(x)))}\t残り：{chars.ToString()}、単語数：{sortedWords.Skip(skipCount).Where(x => x.Chars.PrimaryChar == chars.PrimaryChar).Count()}");
#endif

            if (this.failsBorder.ContainsKey(chars) && this.failsBorder[chars] <= skipCount) yield break;

            int i = skipCount;
            int count = 0;

            while (i < this.sortedWords.Length && this.sortedWords[i].Chars.PrimaryChar != chars.PrimaryChar) i++;
            while (i < this.sortedWords.Length && this.sortedWords[i].Chars.PrimaryChar == chars.PrimaryChar) {
                var word = this.sortedWords[i++];
                if (!chars.IsInclude(word.Chars)) continue;

                var found = chars.Subtract(word.Chars);
                var result = this.Analyze(found, currentInput.Concat(new[] { word.Body }), i);
                foreach (var r in result) {
                    yield return r;
                    count++;
                }
            }

            if (count == 0) {
                if (this.failsBorder.ContainsKey(chars)) this.failsBorder[chars] = skipCount;
                else this.failsBorder.Add(chars, skipCount);
            }
        }
        #endregion

        private class WordNode<T2> {
            public WordNode(T2 body, UnorderChars chars) {
                Body = body;
                Chars = chars;
            }

            public T2 Body { get; }
            public UnorderChars Chars { get; }
        }
    }
}