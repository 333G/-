using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Util;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuceneIO = Lucene.Net.Store;

namespace NFine.Search
{
    /// <summary>
    /// 分词帮助类
    /// </summary>
    public class LuceneHelper
    {
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <returns></returns>
        public void CreateIndex()
        {
            bool isPangu = true; //是否按照盘古分词器进行分词
            LuceneIndex.PrepareIndex(isPangu);//创建索引，保存在应用程序的index文件夹下
        }

        /// <summary>
        /// 针对内容进行分词，并显示结果
        /// </summary>
        /// <param name="content">文字内容</param>
        /// <param name="hasKeyWords">包含关键词</param>
        /// <returns></returns>
        public bool SearchCheck(string content, out List<string> hasKeyWords)
        {
            List<string> keyWords = new List<string>();
            hasKeyWords = new List<string>();
            bool result = false;
            IList<Analyzer> listAnalyzer = LuceneAnalyzer.BuildAnalyzers();
            //拆分句子，分词
            LuceneAnalyzer.TestAnalyzer(listAnalyzer, content, out keyWords);

            //检查分词结果
            foreach (string key in keyWords)
            {
                foreach (Analyzer analyzer in listAnalyzer)
                {
                    List<string> tempList = new List<string>();
                    int temp = LuceneSearch.PanguQueryTest(analyzer, "content", key, out tempList);//通过盘古分词搜索
                    if (temp > 0)
                    {
                        result = true;
                        hasKeyWords.AddRange(tempList);
                    }
                }
            }
            hasKeyWords = hasKeyWords.Distinct().ToList();
            return result;
        }


        public List<string>  Search(string content)
        {
            List<string> keyWords = new List<string>();
            IList<Analyzer> listAnalyzer = LuceneAnalyzer.BuildAnalyzers();
            //拆分句子，分词
            LuceneAnalyzer.TestAnalyzer(listAnalyzer, content, out keyWords);
            return keyWords;
        }
    }
}