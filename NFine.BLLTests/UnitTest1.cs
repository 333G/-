using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFine.Code;

namespace NFine.BLLTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Add_Test()
        {
            Entity.SMS_Test model = new Entity.SMS_Test();
            model.TestName = "测试名称";
            model.TestDescription = "测试描述";
            model.TestState = 1;
            
            int addResult=BLL.SMS.SMS_TestBLL.Instance.Add(model);
            Console.WriteLine("addResult:" + addResult);
        }

        [TestMethod]
        public void FindEntityTest()
        {
            int id = 1;
            Entity.SMS_Test model = BLL.SMS.SMS_TestBLL.Instance.FindEntity(id);
            Console.WriteLine("获取实体："+model.TestName);
        }

        [TestMethod]
        public void UpdateTest()
        {
            int id = 1;
            Entity.SMS_Test model = BLL.SMS.SMS_TestBLL.Instance.FindEntity(id);
            model.TestName = "测试名称修改后";

            bool updateResult = BLL.SMS.SMS_TestBLL.Instance.Update(model);
            Console.WriteLine("updateResult:" + updateResult);
        }

        [TestMethod]
        public void DeleteTest()
        {
            int id = 1;
            bool deleteResult = BLL.SMS.SMS_TestBLL.Instance.Delete(id);
            Console.WriteLine("deleteResult:"+deleteResult);
        }

        [TestMethod]
        public void FindListTest()
        {
            string name = "测试名称";
            var result = BLL.SMS.SMS_TestBLL.Instance.FindList(name);
            Console.WriteLine("查询结果："+result.Count);
        }
        
        [TestMethod]
        public void FindListTest2()
        {
            string name = "测试名称";
            string creator = "测试";

            var result = BLL.SMS.SMS_TestBLL.Instance.FindList(name, creator);
            Console.WriteLine("查询结果："+result.Count);
        }
        /// <summary>
        /// 分页查询结果
        /// </summary>
        [TestMethod]
        public void FindPageListTest()
        {
            string name = "测试";
            Pagination pagination = new Pagination();
            //页码
            pagination.page = 1;
            //每页显示的数量
            pagination.rows = 10;

            pagination.sidx = "CreateTime desc";

            var result = BLL.SMS.SMS_TestBLL.Instance.FindPageList(pagination,name);
            Console.WriteLine("分页查询结果："+result.Count);
        }
        [TestMethod]
        public void FindPageListTest2()
        {
            Pagination pagination = new Pagination();
            //页码
            pagination.page = 1;
            //每页显示的数量
            pagination.rows = 10;

            pagination.sidx = "CreateTime desc";

            var result = BLL.SMS.SMS_TestBLL.Instance.FindPageList(pagination);
            Console.WriteLine("分页查询结果：" + result.Count);
        }

        [TestMethod]
        public void MultipleTableTest()
        {
            var result = BLL.Self.TestBLL.Instance.MultipleTable();
            Console.WriteLine("多表联查结果："+result);
        }
    }
}
