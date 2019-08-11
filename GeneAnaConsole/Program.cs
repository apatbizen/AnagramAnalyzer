using AnagramAnalyzer;
using AnagramAnalyzer.MarkovChainProvider;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneAnaConsole {
    class Program {
        static void Main(string[] args) {
            var repo = new WordProvider();
            repo.LoadAll("./condenced3.tsv");

            var decoder = new AnagramDecoder<WordNode>(repo.Exhibit(), x => x.Kana);

            Stopwatch stopwatch = new Stopwatch();
            while (true) {
                Console.Write("文章を入力(空値入力で終了) >> ");
                var accepted = Console.ReadLine();
                if (accepted.Length == 0) break;

                stopwatch.Start();

                var result = decoder.Analyze(accepted);
                var count = 0;
                foreach (var set in result.AsParallel()) {
                    Console.WriteLine(string.Join(" + ", set.Select(x => x.Surface)));
                    count++;
                }
                Console.WriteLine($"{count} 件見つかりました。({stopwatch.Elapsed.TotalMilliseconds:0.000} ms)\n");
                stopwatch.Reset();
            }

            Console.WriteLine("プログラムを終了");
        }
    }
}
