using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Util;
using System;
using System.IO;
using LuceneIO = Lucene.Net.Store;

namespace NFine.Search
{
    public class LuceneIndex
    {
        public static void PrepareIndex(bool isPangu)
        {
            Analyzer analyzer = null;
            if (isPangu)
            {
                analyzer = new PanGuAnalyzer();//盘古Analyzer
            }
            else
            {
                analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
            }
            DirectoryInfo dirInfo = Directory.CreateDirectory(Config.INDEX_STORE_PATH);
            LuceneIO.Directory directory = LuceneIO.FSDirectory.Open(dirInfo);
            //查询所有的敏感词
            var list = BLL.SMS_SensitiveWordsManager.Instance.AllList();

            IndexWriter writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED);
            foreach (var item in list)
            {
                CreateIndex(writer, item.F_Id, item.F_SensitiveWords);
            }
            writer.Optimize();
            writer.Close();
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="analyzer"></param>
        /// <param name="content"></param>
        private static void CreateIndex(IndexWriter writer, string id, string content)
        {
            try
            {
                Document doc = new Document();
                doc.Add(new Field("id", id.ToString(), Field.Store.YES, Field.Index.ANALYZED));//存储且索引
                doc.Add(new Field("content", content, Field.Store.YES, Field.Index.ANALYZED));//存储且索引
                writer.AddDocument(doc);
            }
            catch (FileNotFoundException fnfe)
            {
                throw fnfe;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}