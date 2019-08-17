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
            var arr = new[] { new TempWordItem("粟生", "あお", Enumerable.Empty<TempWordItem>()) };
            var decoder = new AnagramDecoder<TempWordItem>(arr);

            var result = decoder.Analyze("おあ");
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1, result.First().Count());
            Assert.AreEqual("あお", result.First().First().Kana);
        }

        [Test]
        public void Method_AddWord_追加後ヒットしない() {
            var arr = new[] { new TempWordItem("粟生", "あお", Enumerable.Empty<TempWordItem>()) };
            var decoder = new AnagramDecoder<TempWordItem>(arr);

            var result = decoder.Analyze("おあふ");
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void Property_WordCount() {
            var aioi = new TempWordItem("相生", "あいおい", Enumerable.Empty<TempWordItem>());
            var aino = new TempWordItem("愛野", "あいの", Enumerable.Empty<TempWordItem>());
            var aidu = new TempWordItem("会津若松", "あいづわかまつ", Enumerable.Empty<TempWordItem>());
            var aniai = new TempWordItem("阿仁合", "あにあい", Enumerable.Empty<TempWordItem>());
            var abekawa = new TempWordItem("安倍川", "あべかわ", Enumerable.Empty<TempWordItem>());

            var decoder = new AnagramDecoder<TempWordItem>(new[] { aioi, aino, aidu, aniai, abekawa });
            Assert.AreEqual(5, decoder.WordCount);
        }

        [Test]
        public void Property_HeadCount() {
            var aioi = new TempWordItem("相生", "あいおい", Enumerable.Empty<TempWordItem>());
            var aino = new TempWordItem("愛野", "あいの", new[] { aioi });
            var aidu = new TempWordItem("会津若松", "あいづわかまつ", new[] { aino });
            var aniai = new TempWordItem("阿仁合", "あにあい", new[] { aino });
            var abekawa = new TempWordItem("安倍川", "あべかわ", new[] { aidu, aioi });


            var decoder = new AnagramDecoder<TempWordItem>(new[] { aioi, aino, aidu, aniai, abekawa });
            Assert.AreEqual(2, decoder.HeadCount);
        }

        [Test]
        public void UseCase_解2_単語3() {
            var tu = new TempWordItem("津", "つ", Enumerable.Empty<TempWordItem>());
            var arr = new[] {
                new TempWordItem("粟生", "あお", new[]{ tu }),
                new TempWordItem("青津", "あおつ", Enumerable.Empty<TempWordItem>()),
                tu
            };
            var decoder = new AnagramDecoder<TempWordItem>(arr);

            var result = decoder.Analyze("あおつ");
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(x => x.Count()==1 && x.Any(item=>item.Kana=="あおつ")));
            Assert.IsTrue(result.Any(x => x.Count()==2 && x.Any(item=>item.Kana=="あお") && x.Any(item=>item.Kana=="つ")));
        }

        [Test]
        public void UseCase_解1_単語3() {
            var mishima = new TempWordItem("三島", "みしま", Enumerable.Empty<TempWordItem>());
            var kannami = new TempWordItem("函南", "かんなみ", new[] { mishima });
            var atami = new TempWordItem("熱海", "あたみ", new[] { kannami });
            var decoder = new AnagramDecoder<TempWordItem>(new[] { atami, mishima, kannami });

            var result = decoder.Analyze("みなみんみましたかあ");
            Assert.AreEqual(1, result.Count());
        }

        private class TempWordItem : IWordItem {
            public IEnumerable<IWordItem> nexts;
            public TempWordItem(string surface, string kana, IEnumerable<IWordItem> nexts) {
                this.Surface = surface;
                this.Kana = kana;
                this.nexts = nexts;
            }

            public string Surface { get; }

            public string Kana { get; }

            public IEnumerable<IWordItem> FindNexts() => this.nexts;
        }
    }
}
