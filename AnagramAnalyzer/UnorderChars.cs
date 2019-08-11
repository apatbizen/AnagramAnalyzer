using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AnagramAnalyzer {
    /// <summary>
    /// 順序をもたない文字の集合
    /// </summary>
    public class UnorderChars{
        private readonly char[] charsMap;

        private Func<char> primaryProvider = () => throw new InvalidOperationException("空集合ではPrimaryCharの取得はできません。");

        private readonly IComparer<char> comparer;

        #region 【コンストラクタ】
        public UnorderChars(string source, IComparer<char> comparer) {
            if (string.IsNullOrEmpty(source)) {
                this.charsMap = new char[0];
            } else { 
                this.charsMap = source.ToCharArray().OrderBy(x => x, comparer).ToArray();
                this.primaryProvider = () => charsMap[0];
            }
            this.comparer = comparer;
        }
        private UnorderChars(IEnumerable<char> source, IComparer<char> comparer) {
            this.charsMap = source.ToArray();
            if (charsMap.Length > 0) this.primaryProvider = () => charsMap[0];
            this.comparer = comparer;
        }
        #endregion

        #region 【プロパティ】

        /// <summary>空であるか</summary>
        public bool IsEmpty { get => !this.charsMap.Any(); }
        
        /// <summary>保有している文字数</summary>
        public int Count { get => this.charsMap.Length; }

        /// <summary>保有している最も優先度の高い文字</summary>
        public char PrimaryChar => this.primaryProvider();

        #endregion

        #region 【メソッド】IsInclude 指定した文字列を包含するか判定する
        /// <summary>指定したオブジェクトを包含するか判定する</summary>
        /// <param name="target">包含されるオブジェクト</param>
        /// <returns>true:包含</returns>
        public bool IsInclude(UnorderChars target) {
            int otherIdx = 0;
            foreach (var myself in this.charsMap) {
                if (otherIdx >= target.charsMap.Length) return true;
                if (comparer.Compare(myself, target.charsMap[otherIdx]) > 0) return false;
                if (myself == target.charsMap[otherIdx]) otherIdx++;
            }
            return (otherIdx==target.charsMap.Length);
        }
        #endregion

        #region 【メソッド】Subtract 指定した文字列を減算する
        public UnorderChars Subtract(UnorderChars target) {
            if(!this.IsInclude(target)) throw new InvalidOperationException($"{this.ToString()}から{target.ToString()}を減算できません。");
            char[] result = new char[Math.Max(this.charsMap.Length - target.charsMap.Length,0)];

            int top = 0;
            int otherIdx = 0;
            foreach(var myself in this.charsMap){
                if (otherIdx >= target.charsMap.Length || comparer.Compare(target.charsMap[otherIdx],myself) > 0) {
                    result[top++] = myself;
                } else if(target.charsMap[otherIdx] == myself) {
                    otherIdx++;
                }
            }
            return new UnorderChars(result, this.comparer);
        }
        #endregion

        public bool Has(char v) => this.charsMap.Any(x => x == v);

        #region 【メソッド】基本オブジェクトのオーバーライド
        /// <summary>指定したオブジェクトと等価であるか</summary>
        /// <param name="obj">比較対象</param>
        public override bool Equals(object obj) {
            if (obj.GetType() != typeof(UnorderChars)) return false;

            var casted = ((UnorderChars)obj);
            return this.IsInclude(casted) && casted.IsInclude(this);
        }

        /// <summary>HashCodeを取得する</summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode() => string.Concat(this.charsMap).GetHashCode();

        public override string ToString() => string.Concat(this.charsMap);

        #endregion
    }
}