using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace NFine.BLLTests
{
    [TestClass]
    public class SearchUnitTest
    {
        [TestMethod]
        public void TestIndexMethod()
        {
            NFine.Search.LuceneHelper helper = new Search.LuceneHelper();
            helper.CreateIndex();
        }

        public void TestSearchMethod()
        {
            NFine.Search.LuceneHelper helper = new Search.LuceneHelper();
            List<string> list = new List<string>();
            helper.SearchCheck("中国是有一个男孩，名字叫：卢亚东。他老婆是王志英", out list);
            foreach (string item in list)
            {
                Console.WriteLine(item);
            }
        }
    }
}