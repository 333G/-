/*-------------------------------------------------------------------------
 * 作者：NeoLu
 * Email：1113865828@qq.com
 * github:https://github.com/Neo-Lu
 * 创建时间： 2016/10/22 23:33:12
 * 版本号：v1.0
 * 本类主要用途描述：
 *  -------------------------------------------------------------------------*/

namespace NFine.Log
{
    class GetLogImp
    {
        public static Ilog GetLogs
        {
            get { return new LogManager(); }
        }
    }
}
