/*-------------------------------------------------------------------------
 * 作者：NeoLu
 * Email：1113865828@qq.com
 * github:https://github.com/Neo-Lu
 * 创建时间： 2016/10/24 21:52:07
 * 版本号：v1.0
 * 本类主要用途描述：
 *  -------------------------------------------------------------------------*/
using System;

namespace NFine.Log
{
    public interface Ilog
    {
        #region Debug

        void Debug(string message);

        void Debug(string message, Exception ex);

        #endregion Debug

        #region Error

        void Error(string message);

        void Error(string message, Exception ex);

        #endregion Error

        #region Fatal

        void Fatal(string message);

        void Fatal(string message, Exception ex);

        #endregion Fatal

        #region Info

        void Info(string message);

        void Info(string message, Exception ex);

        #endregion Info

        #region Warn

        void Warn(string message);

        void Warn(string message, Exception ex);

        #endregion Warn
        

        void LoadLog4netConfig();
    }
}
