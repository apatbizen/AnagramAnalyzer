using AnagramAnalyzer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnagramAnalyzerTest {
    public class AnagramAnalyzerTest {
        [Test]
        public void Method_AddWord_追加後ヒットする() {
            var decoder = new AnagramDecoder<string>(new[] { "あお" }, x => x);

            var result = decoder.Analyze("おあ");
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1, result.First().Count());
            Assert.AreEqual("あお", result.First().First());
        }

        [Test]
        public void Method_AddWord_追加後ヒットしない() {
            var decoder = new AnagramDecoder<string>(new[] { "あお" }, x => x);

            var result = decoder.Analyze("おあふ");
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void UseCase_解2_単語3() {
            var decoder = new AnagramDecoder<string>(new[] { "あおつ", "あお", "つ" }, x => x);

            var result = decoder.Analyze("あおつ");
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(x => x.Count()==1 && x.Contains("あおつ")));
            Assert.IsTrue(result.Any(x => x.Count()==2 && x.Contains("あお") && x.Contains("つ")));
        }

        [Test]
        public void UseCase_解1_単語3() {
            var decoder = new AnagramDecoder<string>(new[] { "みしま", "かんなみ", "あたみ" }, x => x);

            var result = decoder.Analyze("みなみんみましたかあ");
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void UseCase_解1_単語10万() {
            var decoder = new AnagramDecoder<string>(this.HundredThousandHanshin(), x => x);
            var result = decoder.Analyze("せいぶちくいつもだね");
            Assert.AreEqual(10000, result.Count());
        }
        private IEnumerable<string> HundredThousandHanshin() {
            yield return "ちぶね";
            for (int i = 0; i < 10000; i++) {
                yield return "うめだ";
                yield return "ふくしま";
                yield return "のだ";
                yield return "よどがわ";
                yield return "ひめしま";
                yield return "くいせ";
                yield return "あまがさき";
                yield return "でやしき";
                yield return "ぷーるまえ";
                yield return "むこがわ";
            }
            yield return "だいもつ";
        }
    }
}
