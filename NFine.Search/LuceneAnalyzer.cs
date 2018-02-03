using System;
using System.Collections.Generic;
using System.IO;

namespace NFine.Search
{
    using Lucene.Net.Analysis;
    using Lucene.Net.Analysis.PanGu;

    public class LuceneAnalyzer
    {
        /// <summary>
        /// 构造常见的几种Analyzer列表
        /// </summary>
        /// <returns></returns>
        public static IList<Analyzer> BuildAnalyzers()
        {
            IList<Analyzer> listAnalyzer = new List<Analyzer>()
            {
                new PanGuAnalyzer(),
            };
            return listAnalyzer;
        }

        /// <summary>
        /// 测试不同的Analyzer分词效果
        /// </summary>
        /// <param name="listAnalyzer"></param>
        /// <param name="input"></param>
        /// <param name="keywords">拆分的结果</param>
        public static void TestAnalyzer(IList<Analyzer> listAnalyzer, string input, out List<string> keywords)
        {
            keywords = new List<string>();
            foreach (Analyzer analyzer in listAnalyzer)
            {
                using (TextReader reader = new StringReader(input))
                {
                    TokenStream stream = analyzer.ReusableTokenStream(string.Empty, reader);
                    Lucene.Net.Analysis.Token token = null;
                    while ((token = stream.Next()) != null)
                    {
                        Console.WriteLine(token.TermText());
                        keywords.Add(token.TermText());
                    }
                }
            }
        }
    }
}