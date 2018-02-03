/*-------------------------------------------------------------------------
 * 作者：NeoLu
 * Email：1113865828@qq.com
 * github:https://github.com/Neo-Lu
 * 创建时间： 2016/10/24 21:53:35
 * 版本号：v1.0
 * 本类主要用途描述：
 *  -------------------------------------------------------------------------*/

namespace NFine.Log
{
    public class LogHelper
    {
        #region 提供对ILog的访问

        public static Ilog GetLogManager
        {
            get { return GetLogImp.GetLogs; }
        }

        #endregion 提供对ILog的访问
    }
}
