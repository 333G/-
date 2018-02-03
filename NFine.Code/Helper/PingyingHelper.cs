using System.Text;
using System.Data;

namespace NFine.Code
{
    /// <summary>
    /// 对SEO友好的汉字标题转拼音
    /// </summary>
    public class PingyingHelper
    {
        private static string GetEnglish(string str)
        {
            str = str.Replace("中国", " china ");
            str = str.Replace("策划", " plan ");
            str = str.Replace("免费", " free ");
            str = str.Replace("介绍", " intro ");
            str = str.Replace("技巧", " skill ");
            str = str.Replace("生活", " life ");
            str = str.Replace("活动", " activity ");
            str = str.Replace("工具", " tool ");
            str = str.Replace("联盟", " union ");
            str = str.Replace("注册", " register ");
            str = str.Replace("经验", " experience ");
            str = str.Replace("翻译", " translate ");
            str = str.Replace("项目", " item ");
            str = str.Replace("网站", " web-site ");
            str = str.Replace("英语", " english ");
            str = str.Replace("英文", " english ");
            str = str.Replace("雅虎", " yahoo ");
            str = str.Replace("新浪", " sina ");
            str = str.Replace("支付宝", " alipay ");
            str = str.Replace("交易", " trade ");
            str = str.Replace("网店", " b2c ");
            str = str.Replace("升级", " update ");
            str = str.Replace("杂志", " magazine ");
            str = str.Replace("空间", " space ");
            str = str.Replace("爱情", " love ");
            str = str.Replace("朋友", " friend ");
            str = str.Replace("友情", " friend ");
            str = str.Replace("链接", " like ");
            str = str.Replace("标签", " label ");
            str = str.Replace("运行", " running ");
            str = str.Replace("管理", " manager ");
            str = str.Replace("管理", " manage ");
            str = str.Replace("页面", " page ");
            str = str.Replace("模板", " template ");
            str = str.Replace("游戏", " game ");
            str = str.Replace("论坛", " forum ");
            str = str.Replace("新闻", " news ");
            str = str.Replace("音乐", " music ");
            str = str.Replace("帮助", " help ");
            str = str.Replace("优化", " optimize ");
            str = str.Replace("软件", " soft ");
            str = str.Replace("教程", " tech ");
            str = str.Replace("下载", " download ");
            str = str.Replace("搜索", " search ");
            str = str.Replace("引擎", " engine ");
            str = str.Replace("蜘蛛", " spider ");
            str = str.Replace("日志", " log ");
            str = str.Replace("博客", " blog ");
            str = str.Replace("百度", " baidu ");
            str = str.Replace("谷歌", " google ");
            str = str.Replace("邮箱", " mailbox ");
            str = str.Replace("邮件", " mail ");
            str = str.Replace("域名", " domain ");
            str = str.Replace("测试", " test");
            str = str.Replace("演示", " demo ");
            str = str.Replace("音乐", " music ");
            str = str.Replace("笑话", " joke ");
            str = str.Replace("产品", " product ");
            str = str.Replace("留言", " message ");
            str = str.Replace("反馈", " freedback ");
            str = str.Replace("评论", " comment ");
            str = str.Replace("推荐", " commend ");
            str = str.Replace("共享", " share ");
            str = str.Replace("资源", " resource ");
            str = str.Replace("插件", " plugins ");
            str = str.Replace("本本", " notebook ");
            str = str.Replace("电脑", " computer ");
            str = str.Replace("系统", " system ");
            str = str.Replace("学校", " school ");
            str = str.Replace("无忧", " 5u ");
            str = str.Replace("工作", " job ");
            str = str.Replace("信息", " info ");
            str = str.Replace("娱乐", " ent ");
            str = str.Replace("汽车", " car ");
            str = str.Replace("手机", " mobile ");
            str = str.Replace("网络", " network ");
            str = str.Replace("老板", " boss ");
            str = str.Replace("狗", " dog ");
            str = str.Replace("电视", " tv ");
            str = str.Replace("电影", " movie ");
            str = str.Replace("其他", " other ");
            str = str.Replace("出租车", " taix ");
            str = str.Replace("司机", " driver ");
            str = str.Replace("项目", " project ");
            return str;
        }

        /// <summary>
        /// 转换成拼音形式的url
        /// </summary>
        /// <param name="chinese"></param>
        /// <returns></returns>
        public static string Pinyin(string chinese)
        {
            chinese = chinese.Replace("/", "");
            chinese = chinese.Replace("\\", "");
            chinese = chinese.Replace("*", "");
            chinese = chinese.Replace("]", "");
            chinese = chinese.Replace("[", "");
            chinese = chinese.Replace("}", "");
            chinese = chinese.Replace("{", "");
            chinese = chinese.Replace("'", "");
            chinese = GetEnglish(chinese); //在这里使用getEnglish先将特殊词语转换

            var pinyinstr = "";
            var isCn = true;

            //打开拼音库
            try
            {
                var dt = new DataTable();

                for (var i = 0; i < chinese.Length; i++)
                {
                    var iIsCn = isCn;
                    var istr = chinese.Substring(i, 1);
                    var x = Ascii(istr);
                    if ((x >= 65 && x <= 90) || (x >= 97 && x <= 122) || (x >= 48 && x <= 57) || istr == " ")
                    {
                        isCn = false; // 这些是英文,数字(保留字符),不改动
                        if (istr == " ") istr = "-";
                    }
                    else
                    {
                        var dr = dt.Select("[content] like '%" + istr + "%'");
                        if (dr.Length > 0)
                        {
                            istr = dr[0]["pinyin"].ToString().ToLower() + "-";
                            isCn = true;
                        }
                        else
                        {
                            isCn = false;
                            if (istr == " ")
                                istr = "-";
                            else
                                istr = ""; //将空格转换成-,如果是其他字符则清除
                        }
                    }
                    if (iIsCn == isCn)
                        pinyinstr = pinyinstr + istr;
                    else
                        pinyinstr = pinyinstr + "-" + istr;
                    pinyinstr = pinyinstr.Replace("--", "-");
                    pinyinstr = pinyinstr.Replace("__", "_");
                }

                if (pinyinstr.Substring(0, 1) == "-" || pinyinstr.Substring(0, 1) == "_") pinyinstr = pinyinstr.Remove(0, 1);
                if (pinyinstr.Substring(pinyinstr.Length - 1, 1) == "-" || pinyinstr.Substring(pinyinstr.Length - 1, 1) == "_") pinyinstr = pinyinstr.Remove(pinyinstr.Length - 1, 1);

                return pinyinstr.Trim().ToLower();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 是否为双字节字符。
        /// </summary>
        private static bool Istwobyteschar(string chr)
        {
            var str = chr;
            // 使用中文支持编码
            var ecode = Encoding.GetEncoding("gb18030");
            return ecode.GetByteCount(str) == 2;
        }

        /// <summary>
        /// 得到字符的ascii码
        /// </summary>
        private static int Ascii(string chr)
        {
            var ecode = Encoding.GetEncoding("gb18030");
            var codebytes = ecode.GetBytes(chr);
            if (Istwobyteschar(chr))
            {
                // 双字节码为高位乘256，再加低位
                // 该为无符号码，再减65536
                return (int)codebytes[0] * 256 + (int)codebytes[1] - 65536;
            }
            else
            {
                return (int)codebytes[0];
            }
        }

    }
}
