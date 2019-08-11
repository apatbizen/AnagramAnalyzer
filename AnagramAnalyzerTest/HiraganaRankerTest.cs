using AnagramAnalyzer;
using AnagramAnalyzer.CharRanker;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnagramAnalyzerTest {
    class CharRankerTest {
        private HiraganaRanker ranker;

        [SetUp]
        public void SetUp() {
            this.ranker = new HiraganaRanker();
        }

        [Test]
        public void 生成() {
            Assert.AreEqual(0, this.ranker.PopularityOf('あ'));
        }

        [Test]
        public void Method_CheckIn_String() {
            this.ranker.CheckIn("みなみん");
            Assert.AreEqual(1, this.ranker.PopularityOf('み'));
            Assert.AreEqual(1, this.ranker.PopularityOf('な'));
            Assert.AreEqual(1, this.ranker.PopularityOf('ん'));
        }

        [Test]
        public void Method_CheckIn_伸ばし棒() {
            this.ranker.CheckIn("ぷーるまえ");
            Assert.AreEqual(1, this.ranker.PopularityOf('ー'));
        }

        [Test]
        public void Method_Compare() {
            this.ranker.CheckIn("ああいう");
            this.ranker.CheckIn("あい");
            Assert.AreEqual(0, this.ranker.Compare('あ', 'あ'));
            Assert.Greater(this.ranker.Compare('い', 'う'), 0);
            Assert.Less(this.ranker.Compare('う', 'あ'), 0);
            Assert.Greater(this.ranker.Compare('あ', 'い'), 0);
        }

        [Test]
        public void Property_ListOfPopularityOrder() {
            this.ranker.CheckIn("あいう");
            this.ranker.CheckIn("あえお");
            this.ranker.CheckIn("おか");

            var list = this.ranker.ListOfPopularityOrder;
            Assert.AreEqual('あ', list.First());
            Assert.AreEqual('お', list.Skip(1).First());
            Assert.AreEqual('い', list.Skip(2).First());
            Assert.AreEqual('う', list.Skip(3).First());
            Assert.AreEqual('え', list.Skip(4).First());
            Assert.AreEqual('か', list.Skip(5).First());
        }

        [Test]
        public void Property_ListOfRarityOrder() {
            this.ranker.CheckIn("あいう");
            this.ranker.CheckIn("あえお");
            this.ranker.CheckIn("おか");

            var list = this.ranker.ListOfRarityOrder.Where(x=>this.ranker.PopularityOf(x)>0);
            Assert.AreEqual('い', list.First());
            Assert.AreEqual('う', list.Skip(1).First());
            Assert.AreEqual('え', list.Skip(2).First());
            Assert.AreEqual('か', list.Skip(3).First());
            Assert.AreEqual('あ', list.Skip(4).First());
            Assert.AreEqual('お', list.Skip(5).First());
        }
    }
}
