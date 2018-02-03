using System;
using System.Collections.Generic;
using System.Text;

namespace NFine.Search
{
    using Lucene.Net.Analysis;
    using Lucene.Net.Analysis.PanGu;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Lucene.Net.Util;
    using PanGu;

    public class LuceneSearch
    {
        #region 搜索表达式

        public static int NormalQueryParserTest(Analyzer analyzer, string field, string keyword, out List<string> keyList)
        {
            QueryParser parser = new QueryParser(Version.LUCENE_29, field, analyzer);
            Query query = parser.Parse(keyword);
            return SearchToShow(query, out keyList);
        }

        public static int TermQueryTest(Analyzer analyzer, string field, string keyword, out List<string> keyList)
        {
            TermQuery query = new TermQuery(new Term(field, keyword));
            return SearchToShow(query, out keyList);
        }

        public static int BooleanQueryTest(Analyzer analyzer, string field, string keyword, BooleanClause.Occur[] flags, out List<string> keyList)
        {
            string[] arrKeywords = keyword.Trim().Split(new char[] { ' ', ',', '，', '、' }, StringSplitOptions.RemoveEmptyEntries);
            QueryParser parser = new QueryParser(Version.LUCENE_29, field, analyzer);
            BooleanQuery bq = new BooleanQuery();
            int counter = 0;
            foreach (string item in arrKeywords)
            {
                Query query = parser.Parse(item);
                bq.Add(query, flags[counter]);
                counter++;
            }
            return SearchToShow(bq, out keyList);
        }

        public static int RangeQueryTest(Analyzer analyzer, string field, string lowTerm, string upperTerm, out List<string> keyList)
        {
            Query query = new TermRangeQuery(field, lowTerm, upperTerm, true, true);
            query = new TermRangeQuery(field, lowTerm, upperTerm, true, false);
            return SearchToShow(query, out keyList);
        }

        public static int PrefixQueryTest(Analyzer analyzer, string field, string keyword, out List<string> keyList)
        {
            PrefixQuery query = new PrefixQuery(new Term(field, keyword));
            return SearchToShow(query, out keyList);
        }

        public static int WildcardQueryTest(Analyzer analyzer, string field, string keyword, out List<string> keyList)
        {
            WildcardQuery query = new WildcardQuery(new Term(field, keyword));
            return SearchToShow(query, out keyList);
        }

        public static int FuzzyQueryTest(Analyzer analyzer, string field, string keyword, out List<string> keyList)
        {
            FuzzyQuery query = new FuzzyQuery(new Term(field, keyword));
            return SearchToShow(query, out keyList);
        }

        public static int PhraseQueryTest(Analyzer analyzer, string field, string keyword, int slop, out List<string> keyList)
        {
            string[] arr = keyword.Trim().Split(new char[] { ' ', ',', '，', '、' }, StringSplitOptions.RemoveEmptyEntries);
            PhraseQuery query = new PhraseQuery();
            foreach (string item in arr)
            {
                query.Add(new Term(field, item));
            }
            query.SetSlop(slop);
            return SearchToShow(query, out keyList);
        }

        public static int MulFieldsSearchTest(Analyzer analyzer, string[] fields, string keyword, BooleanClause.Occur[] flags, out List<string> keyList)
        {
            MultiFieldQueryParser parser = new MultiFieldQueryParser(Version.LUCENE_29, fields, analyzer);
            //Query query = parser.Parse(keyword);
            Query query = MultiFieldQueryParser.Parse(Version.LUCENE_29, keyword, fields, flags, analyzer);
            return SearchToShow(query, out keyList);
        }

        #endregion 搜索表达式

        #region 辅助方法

        /// <summary>
        /// 搜索并显示结果
        /// </summary>
        /// <param name="query"></param>
        private static int SearchToShow(Query query, out List<string> keyList)
        {
            int n = 10;//最多返回多少个结果
            TopDocs docs = Config.GenerateSearcher().Search(query, (Filter)null, n);
            return ShowSearchResult(docs, out keyList);
        }

        /// <summary>
        /// 显示搜索结果
        /// </summary>
        /// <param name="queryResult"></param>
        /// <param name="keyList">包含的关键词</param>
        private static int ShowSearchResult(TopDocs queryResult, out List<string> keyList)
        {
            keyList = new List<string>();
            if (queryResult == null || queryResult.totalHits == 0)
                return 0;

            int counter = 1;
            foreach (ScoreDoc sd in queryResult.scoreDocs)
            {
                try
                {
                    Document doc = Config.GenerateSearcher().Doc(sd.doc);
                    //string id = doc.Get("id");
                    string content = doc.Get("content");
                    keyList.Add(content);
                }
                catch (Exception ex)
                {
                }
                counter++;
            }
            return counter;
        }

        #endregion 辅助方法

        #region 盘古分词搜索

        public static int PanguQueryTest(Analyzer analyzer, string field, string keyword, out List<string> keyList)
        {
            QueryParser parser = new QueryParser(Version.LUCENE_29, field, analyzer);
            string panguQueryword = GetKeyWordsSplitBySpace(keyword, new PanGuTokenizer());//对关键字进行分词处理
            Query query = parser.Parse(panguQueryword);
            return SearchToShow(query, out keyList);
        }

        public static string GetKeyWordsSplitBySpace(string keywords, PanGuTokenizer ktTokenizer)
        {
            StringBuilder result = new StringBuilder();
            ICollection<WordInfo> words = ktTokenizer.SegmentToWordInfos(keywords);
            foreach (WordInfo word in words)
            {
                if (word == null)
                {
                    continue;
                }
                result.AppendFormat("{0}^{1}.0 ", word.Word, (int)Math.Pow(3, word.Rank));
            }
            return result.ToString().Trim();
        }

        #endregion 盘古分词搜索
    }
}