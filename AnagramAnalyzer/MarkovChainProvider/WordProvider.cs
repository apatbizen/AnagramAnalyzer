using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AnagramAnalyzer.MarkovChainProvider {
    public class WordProvider {
        private IList<WordData> data = new List<WordData>() ;

        public int Count { get => this.data.Count(); }

        public void LoadAll(string fileName) {
            using (var sr = new StreamReader(fileName)) {
                while (!sr.EndOfStream) {
                    var splitted = sr.ReadLine().Split('\t');

                    var nextsSource = (splitted.Length > 2) ? splitted[2] : "";
                    this.data.Add(new WordData() {
                        Parent = this,
                        Surface = splitted[0],
                        Kana = splitted[1].Split(','),
                        NextsList = nextsSource.Split(',')
                    }) ;
                }
            }
        }

        public IEnumerable<WordNode> Exhibit() => this.data.SelectMany(x => x.Give());

        public IEnumerable<WordNode> TakeById(int v) {
            return this.data[v].Give();
        }

        private class WordData {
            public WordProvider Parent;
            public string Surface;
            public IEnumerable<string> Kana;
            public IEnumerable<string> NextsList;

            public IEnumerable<WordNode> Give() {
                var nextsPath = NextsList.Select(x => {
                    var a = x.Split(':');
                    var idx = int.Parse(a[0], System.Globalization.NumberStyles.HexNumber);
                    var freq = int.Parse(a[1]);
                    return new WordNodePath(this.Parent.TakeById(idx), freq);
                });

                foreach(var k in this.Kana) {
                    yield return new WordNode(this.Surface, k, nextsPath);
                }
            }
        }
    }
}
