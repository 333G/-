using System;
using System.IO;

namespace NFine.Search
{
    using Lucene.Net.Search;
    using LuceneIO = Lucene.Net.Store;

    public class Config
    {
        #region fields

        public static readonly string INDEX_STORE_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dict");//索引所在目录
        private static IndexSearcher searcher;//索引搜索器

        #endregion fields

        #region methods

        public static IndexSearcher GenerateSearcher()
        {
            DirectoryInfo dirInfo = Directory.CreateDirectory(Config.INDEX_STORE_PATH);
            LuceneIO.Directory directory = LuceneIO.FSDirectory.Open(dirInfo);
            searcher = new IndexSearcher(directory, true);
            return searcher;
        }

        #endregion methods
    }
}