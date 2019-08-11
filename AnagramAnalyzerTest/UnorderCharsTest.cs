using AnagramAnalyzer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests {
    public class UnorderCharsTest {
        private Comparer<char> comparer = new SimpleComparer();

        [Test]
        public void 生成とプロパティ() {
            var emptyOne = new UnorderChars("", comparer);
            Assert.IsTrue(emptyOne.IsEmpty, "IsEmptyプロパティが不正");
            Assert.AreEqual(0, emptyOne.Count, "Countプロパティが不正");
            Assert.Throws(typeof(InvalidOperationException), delegate { emptyOne.PrimaryChar.Equals('あ'); }, "PrimaryCharで想定した例外が発生しなかった",emptyOne);
            　
            var fullOne = new UnorderChars("ああいうえおお", comparer);
            Assert.IsFalse(fullOne.IsEmpty, "IsEmptyプロパティが不正");
            Assert.AreEqual(7, fullOne.Count, "Countプロパティが不正");
            Assert.AreEqual('あ', fullOne.PrimaryChar);
        }

        [Test]
        public void Method_Equals_等しい場合() {
            var a = new UnorderChars("あいうえお", comparer);
            var b = new UnorderChars("あいうえお", comparer);
            Assert.IsTrue(a.Equals(b), "Equalsに失敗");
            Assert.IsTrue(b.Equals(a), "Equalsに失敗");
            Assert.IsFalse(a == b, "比較演算に失敗");
        }

        [Test]
        public void Method_Equals_順番が異なる() {
            var a = new UnorderChars("あいうえお", comparer);
            var b = new UnorderChars("ういあおえ", comparer);
            Assert.IsTrue(a.Equals(b), "Equalsに失敗");
            Assert.IsTrue(b.Equals(a), "Equalsに失敗");
            Assert.IsFalse(a == b, "比較演算に失敗");
        }

        [Test]
        public void Method_Equals_異なる場合() {
            var a = new UnorderChars("あいうえお", comparer);
            var b = new UnorderChars("アイウエオ", comparer);
            Assert.IsFalse(a.Equals(b), "Equalsに失敗");
            Assert.IsFalse(b.Equals(a), "Equalsに失敗");
            Assert.IsFalse(a == b, "比較演算に失敗");
        }

        [Test]
        public void Method_Equals_包含関係の場合() {
            var a = new UnorderChars("あいうえお", comparer);
            var b = new UnorderChars("あいう", comparer);
            Assert.IsFalse(a.Equals(b), "Equalsに失敗");
            Assert.IsFalse(b.Equals(a), "Equalsに失敗");
            Assert.IsFalse(a == b, "比較演算に失敗");
        }

        [Test]
        public void Method_Subtract_単純成功() {
            var before = new UnorderChars("いろはにほへと", comparer);
            var after = before.Subtract(new UnorderChars("いろに", comparer));
            Assert.AreEqual(new UnorderChars("はほへと", comparer), after, "減算後の結果が異常");
            Assert.AreEqual(new UnorderChars("いろはにほへと", comparer), before, "減算前の状態が変化");
            Assert.IsFalse(before==after);
        }

        [Test]
        public void Method_Subtract_同じ文字が複数() {
            var result = new UnorderChars("しんしましま", comparer).Subtract(new UnorderChars("しまんし", comparer));
            Assert.AreEqual(new UnorderChars("まし", comparer), result);
        }

        [Test]
        public void Method_Subtract_例外() {
            Assert.Throws(typeof(InvalidOperationException)
                        , () => new UnorderChars("わ", comparer).Subtract(new UnorderChars("わわ", comparer))
                        , "例外が発生しなかった");
            Assert.Throws(typeof(InvalidOperationException)
                        , () => new UnorderChars("わをん", comparer).Subtract(new UnorderChars("わゐうゑをん", comparer))
                        , "例外が発生しなかった");
            Assert.Throws(typeof(InvalidOperationException)
                        , () => new UnorderChars("わをん", comparer).Subtract(new UnorderChars("わゐうゑをん", comparer))
                        , "例外が発生しなかった");
        }

        [Test]
        public void Method_IsInclude_True() {
            var source = new UnorderChars("わゐうゑをん", comparer);
            var text = "わをん";
            Assert.IsTrue(source.IsInclude(new UnorderChars(text, comparer)), "包含関係を見つけられなかった");
        }

        [Test]
        public void Method_IsInclude_False() {
            var source = new UnorderChars("わをん", comparer);
            var text = "わゐうゑをん";
            Assert.IsFalse(source.IsInclude(new UnorderChars(text, comparer)), "包含関係を誤検知");
        }

        [Test]
        public void Method_IsInclude_大文字小文字() {
            var source = new UnorderChars("abc", comparer);
            var text = "ABC";
            Assert.IsFalse(source.IsInclude(new UnorderChars(text, comparer)), "大文字と小文字を同一視した");
        }

        [Test]
        public void Method_IsInclude_全角半角() {
            var order = "ｱｲｳウイア".ToCharArray();
            var source = new UnorderChars("アイウ", comparer);
            var text = "ｱｲｳ";
            Assert.IsFalse(source.IsInclude(new UnorderChars(text, comparer)), "大文字と小文字を同一視した");
        }

        [Test]
        public void Method_IsInclude_包含関係() {
            var source = new UnorderChars("あおつ", comparer);
            Assert.IsTrue(source.IsInclude(new UnorderChars("あお", comparer)));
        }

        [Test]
        public void Method_Has_False() {
            var source = new UnorderChars("とうきょう", comparer);
            Assert.IsFalse(source.Has('あ'));
        }

        [Test]
        public void Method_Has_True() {
            var source = new UnorderChars("とうきょう", comparer);
            Assert.IsTrue(source.Has('と'));
        }

        [Test]
        public void Method_GetHashCode() {
            var s1 = new UnorderChars("たちばな", comparer);
            var s2 = new UnorderChars("たちばな", comparer);
            var s3 = new UnorderChars("たなばた", comparer);
            var s4 = new UnorderChars("たな", comparer);
            Assert.AreEqual(s1.GetHashCode(), s2.GetHashCode());
            Assert.AreNotEqual(s1.GetHashCode(), s3.GetHashCode());
            Assert.AreEqual(s1.Subtract(new UnorderChars("ちば", comparer)).GetHashCode(), s4.GetHashCode());
        }

        private class SimpleComparer : Comparer<char> {
            public override int Compare(char x, char y) {
                return x - y;
            }
        }

    }
}