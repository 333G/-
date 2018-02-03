using NFine.Code;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace NFine.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 启动应用程序
        /// </summary>
     
//代码的处理逻辑很简单：如果身份验证码匹配成功，则通过base.SendAsync继续将请求向下传递，否则返回直接中断请求的传递，直接返回一个响应码为403的响应，指示没有权限。
        //注意由于SendAsync的返回值需要封装在Task之中，所以需要使用Task.Factory.StartNew将返回值包含在Task中将customHandler注入到HOST中本例中WebAPI HOST在IIS上，
        //所以我们只需将我们定义的CustomHandler在Application_Start中定义即可 
        protected void Application_Start()
        {
            //省略其他逻辑代码 
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new SecurityHandler());
        }

        //服务端服务端的customHandler用于解析HTTP报头中的身份认证码 
        public class SecurityHandler : DelegatingHandler
        {
            protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken
               cancellationToken)
            {
                int matchHeaderCount = request.Headers.Count((item) =>
                {
                    if ("keyword".Equals(item.Key))
                    {
                        foreach (var str in item.Value)
                        {
                            if ("ibeifeng".Equals(str))
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                });
                if (matchHeaderCount > 0)
                {
                    return base.SendAsync(request, cancellationToken);
                }
                return Task.Factory.StartNew<HttpResponseMessage>(() => { return new HttpResponseMessage(HttpStatusCode.Forbidden); });
            }
        }

    }
}