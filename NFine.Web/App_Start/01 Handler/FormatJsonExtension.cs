using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NFine.Code;
using NFine.Code.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NFine.Web
{

    /// <summary>
    /// json格式化扩展
    /// </summary>
    public static class FormatJsonExtension
    {
        /// <summary>
        /// 格式化json
        /// </summary>
        /// <param name="c"></param>
        /// <param name="data"></param>
        /// <param name="exceptMemberName"></param>
        /// <param name="only"></param>
        /// <param name="formatDate"></param>
        /// <returns></returns>
        public static FormatJsonResult JsonFormat(this Controller c, object data, string[] exceptMemberName, bool only, bool formatDate = false)
        {
            var result = new FormatJsonResult
            {
                Data = data,
                ExceptMemberName = exceptMemberName,
                Only = only,
                FormatDate = formatDate
            };
            return result;
        }
        /// <summary>
        /// 格式化json
        /// </summary>
        /// <param name="c"></param>
        /// <param name="data"></param>
        /// <param name="exceptMemberName"></param>
        /// <returns></returns>
        public static FormatJsonResult JsonFormat(this Controller c, object data, string[] exceptMemberName)
        {
            var result = new FormatJsonResult { Data = data, ExceptMemberName = exceptMemberName, Only = false };

            return result;
        }

        public static FormatJsonResult1 JsonFormat1(this Controller c, object data, string[] exceptMemberName)
        {
            var result = new FormatJsonResult1 { Data = data, ExceptMemberName = exceptMemberName, Only = false };

            return result;
        }
        /// <summary>
        /// 返回一个失败的json数据
        /// </summary>
        /// <param name="c"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static FormatJsonResult JsonResultFail(this Controller c, string msg = "")
        {
            return JsonFormat(c, new ExtMessage() { success = false, msg = msg });
        }
        /// <summary>
        /// 返回一个成功的json数据
        /// </summary>
        /// <param name="c"></param>
        /// <param name="msg"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static FormatJsonResult JsonResultSuccess(this Controller c, string msg = "", object id = null)
        {
            return JsonFormat(c, new ExtMessage() { success = true, msg = msg, id = id });
        }


        public static FormatJsonResult1 JsonResultSuccess1(this Controller c, string msg = "", object id = null)
        {
            return JsonFormat1(c, new ExtMessage() { success = true, msg = msg, id = id });
        }
        /// <summary>
        /// 返回json数据
        /// </summary>
        /// <param name="c"></param>
        /// <param name="success"></param>
        /// <param name="msg"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static FormatJsonResult JsonResult(this Controller c, bool success, string msg, object id)
        {
            return JsonFormat(c, new ExtMessage() { success = success, msg = msg, id = id });
        }
        /// <summary>
        /// 返回供extjs grid使用的json数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="total"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static FormatJsonResult ExtGirdResult<T>(this Controller c, long total, IList<T> data)
        {
            return JsonFormat(c, new ExtGirdData<T> { total = total, data = data });
        }
        /// <summary>
        /// 返回供easyui grid使用的json数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="total"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static FormatJsonResult EasyUIGirdResult<T>(this Controller c, long total, IList<T> data)
        {
            return JsonFormat(c, new EasyUIGirdData<T> { total = total, rows = data }, new string[] { }, true, formatDate: true);
        }
        /// <summary>
        /// 返回供extjs form使用的json数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static FormatJsonResult ExtFormResult<T>(this Controller c, T data)
        {
            return JsonFormat(c, new ExtFormLoadData<T>() { data = new T[] { data } });
        }
        /// <summary>
        /// 返回供extjs tree使用的json数据
        /// </summary>
        /// <param name="c"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ActionResult ExtTreeGridData(this Controller c, object data)
        {
            string json = JsonConvert.SerializeObject(data, TimeConverter);

            return new ContentResult() { Content = "{\"text\":\".\",\"children\":" + json + "}" };
        }
        /// <summary>
        /// 返回一个json数据
        /// </summary>
        /// <param name="c"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static FormatJsonResult JsonFormat(this Controller c, object data)
        {
            return JsonFormat(c, data, null);
        }

        public static FormatJsonResult1 JsonFormat1(this Controller c, object data)
        {
            return JsonFormat1(c, data, null);
        }
        /// <summary>
        /// 返回一个供extjs filter使用的数据
        /// </summary>
        /// <param name="c"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<DataFilter> DesDataFilter(this Controller c, string json)
        {
            if (string.IsNullOrEmpty(json))
                return null;
            return JsonConvert.DeserializeObject<List<DataFilter>>(json);
        }
        /// <summary>
        /// 返回一个供extjs sort使用的数据
        /// </summary>
        /// <param name="c"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<DataSort> DesDataSort(this Controller c, string json)
        {
            if (string.IsNullOrEmpty(json))
                return null;
            return JsonConvert.DeserializeObject<List<DataSort>>(json);
        }
        /// <summary>
        /// 把json数据反序列化成对应list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<T> DesJsonToList<T>(this Controller c, string json)
        {
            if (string.IsNullOrEmpty(json))
                return null;

            if (!json.StartsWith("["))
                json = "[" + json + "]";

            return JsonConvert.DeserializeObject<List<T>>(json);
        }
        /// <summary>
        /// 把json数据反序列化成对应entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DesJsonToEntity<T>(this Controller c, string json)
        {
            if (string.IsNullOrEmpty(json))
                return default(T);

            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 把对象序列化成json字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string JsonString(object data)
        {
            return JsonString(data, null);
        }
        /// <summary>
        /// 把对象序列化成json字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="exceptMemberName"></param>
        /// <returns></returns>
        public static string JsonString(object data, string[] exceptMemberName)
        {
            var sw = new StringWriter();
            var isoConvert = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            isoConvert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            //if (FormatDate)
            var jsonConverter = new JsonConverter[] { isoConvert };


            var serializer = JsonSerializer.Create(
                new JsonSerializerSettings
                {
                    Converters = jsonConverter,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                }
                );
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                //if (FormatJson != false)
                jsonWriter.Formatting = Formatting.Indented;
                serializer.Serialize(jsonWriter, data);
            }
            return sw.ToString();
        }

        /// <summary>
        /// json读取
        /// </summary>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static Dictionary<string, object> JsonReader(string jsonText)
        {
            if (!Utils.IsJson(jsonText)) return null;
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonText);
        }

        /// <summary>
        /// 日期转换
        /// </summary>
        public static IsoDateTimeConverter TimeConverter
        {
            get
            {
                var timeConverter = new IsoDateTimeConverter();
                //这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式     
                timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

                return timeConverter;
            }
        }
    }

    /// <summary>
    /// 格式化json输出
    /// </summary>
    public class FormatJsonResult : ActionResult
    {
        /// <summary>
        /// 要序列化的子对象
        /// </summary>
        public string[] ExceptMemberName { get; set; }
        /// <summary>
        /// 要序列化的对象
        /// </summary>
        public Object Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Only { get; set; }
        /// <summary>
        /// 格式化时间
        /// </summary>
        public bool FormatDate { get; set; }
        /// <summary>
        /// 格式化json格式
        /// </summary>
        public bool FormatJson { get; set; }
        /// <summary>
        /// 重写ExecuteResult
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var response = context.HttpContext.Response;

            response.ContentType = "application/json";

            var sw = new StringWriter();
            JsonSerializer serializer;

            var isoConvert = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            isoConvert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            var jsonConverter = new JsonConverter[] { new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter() };
            if (FormatDate)
                jsonConverter = new JsonConverter[] { isoConvert };

            if (Only == true)
            {
                serializer = JsonSerializer.Create(
                    new JsonSerializerSettings
                    {
                        Converters = jsonConverter,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Include
                    }
                );
            }
            else
            {
                serializer = JsonSerializer.Create(
                    new JsonSerializerSettings
                    {
                        Converters = jsonConverter,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Include
                    }
                );
            }

            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                if (FormatJson != false)
                    jsonWriter.Formatting = Formatting.Indented;
                serializer.Serialize(jsonWriter, Data);
            }



            //byte[] bytes = Encoding.UTF8.GetBytes(sw.ToString());
            //response.AddHeader("Content-Length", bytes.Length.ToString());
            //response.BinaryWrite(bytes);

            response.Write(sw.ToString());



            //response.ContentType = "text/html";

        }
    }



    public class FormatJsonResult1 : ActionResult
    {
        /// <summary>
        /// 要序列化的子对象
        /// </summary>
        public string[] ExceptMemberName { get; set; }
        /// <summary>
        /// 要序列化的对象
        /// </summary>
        public Object Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Only { get; set; }
        /// <summary>
        /// 格式化时间
        /// </summary>
        public bool FormatDate { get; set; }
        /// <summary>
        /// 格式化json格式
        /// </summary>
        public bool FormatJson { get; set; }
        /// <summary>
        /// 重写ExecuteResult
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var response = context.HttpContext.Response;
            if (response.ContentType == null)
                response.ContentType = "text/html";

            var sw = new StringWriter();
            JsonSerializer serializer;

            var isoConvert = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            isoConvert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            var jsonConverter = new JsonConverter[] { new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter() };
            if (FormatDate)
                jsonConverter = new JsonConverter[] { isoConvert };

            if (Only == true)
            {
                serializer = JsonSerializer.Create(
                    new JsonSerializerSettings
                    {
                        Converters = jsonConverter,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Include
                    }
                );
            }
            else
            {
                serializer = JsonSerializer.Create(
                    new JsonSerializerSettings
                    {
                        Converters = jsonConverter,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Include
                    }
                );
            }

            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                if (FormatJson != false)
                    jsonWriter.Formatting = Formatting.Indented;
                serializer.Serialize(jsonWriter, Data);
            }



            //byte[] bytes = Encoding.UTF8.GetBytes(sw.ToString());
            //response.AddHeader("Content-Length", bytes.Length.ToString());
            //response.BinaryWrite(bytes);

            response.Write(sw.ToString());



            //response.ContentType = "text/html";

        }
    }
}
