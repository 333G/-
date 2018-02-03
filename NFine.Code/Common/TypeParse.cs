using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace NFine.Code
{
    /// <summary>
    /// 类型转换类
    /// </summary>
    public class TypeParse
    {
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            if (expression == null) return false;
            var str = expression.ToString();
            if (str.Length <= 0 || str.Length > 11 || !Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$")) return false;
            return (str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1');
        }

        /// <summary>
        /// 是否为double类型
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object expression)
        {
            return expression != null && Regex.IsMatch(expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression == null) return defValue;
            if (String.Compare(expression.ToString(), "true", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return true;
            }
            if (String.Compare(expression.ToString(), "false", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return false;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型，转换失败则返回null
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static int? StrToInt(object expression)
        {
            var data = StrToInt(expression, -1);
            if (data == -1) return null;
            return data;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object expression, int defValue)
        {
            try
            {
                if (expression == null) return defValue;
                var str = expression.ToString();
                if (str.Length <= 0 || str.Length > 11 || !Regex.IsMatch(str, @"^[-]?[0-9]*$")) return defValue;
                if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                {
                    return Convert.ToInt32(str);
                }
                return defValue;
            }
            catch {
                return defValue;
            }
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            if ((strValue == null) || (strValue.ToString().Length > 10))
            {
                return defValue;
            }

            var intValue = defValue;
            {
                var isFloat = Regex.IsMatch(strValue.ToString(), @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (isFloat)
                {
                    intValue = Convert.ToSingle(strValue);
                }
            }
            return intValue;
        }


        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            return strNumber.Length >= 1 && strNumber.All(IsNumeric);
        }

        /// <summary>
        /// 将对象转换为Decimal类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static decimal StrToDecimal(object expression, decimal defValue)
        {
            try
            {
                return Convert.ToDecimal(expression);
            }
            catch
            {
                return defValue;
            }
        }

        /// <summary>
        /// 将对象转换为Decimal类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDataTime(object expression, DateTime defValue)
        {
            try
            {
                return Convert.ToDateTime(expression);
            }
            catch
            {
                return defValue;
            }
        }
    }
}
