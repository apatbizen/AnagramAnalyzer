using AnagramAnalyzer;
using AnagramConsole;
using System;
using System.Diagnostics;
using System.Linq;

namespace EkiAnaConsoleV2 {
    class Program {
        static void Main(string[] args) {
            var repo = new StationRepository();
            var decoder = new AnagramDecoder<StationModel>(repo.LoadAll(), x => x.Ruby);

            Stopwatch stopwatch = new Stopwatch();
            while (true) {
                Console.Write("文章を入力(空値入力で終了) >> ");
                var accepted = Console.ReadLine();
                if (accepted.Length == 0) break;

                stopwatch.Start();

                var result = decoder.Analyze(accepted);
                var count = 0;
                foreach (var set in result.AsParallel()) {
                    Console.WriteLine(string.Join(" + ", set.Select(x => x.Name)));
                    count++;
                }
                Console.WriteLine($"{count} 件見つかりました。({stopwatch.Elapsed.TotalMilliseconds:0.000} ms)\n");
                stopwatch.Reset();
            }

            Console.WriteLine("プログラムを終了");
        }
    }

}
