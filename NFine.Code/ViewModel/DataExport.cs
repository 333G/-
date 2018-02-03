using System.Collections.Generic;

namespace NFine.Code.ViewModel
{
    /// <summary>
    /// 数据导出
    /// </summary>
    public class DataExport
    {
        /// <summary>
        /// 导出的列
        /// </summary>
        public class ExportColumn
        {
            /// <summary>
            /// 列名
            /// </summary>
            public string text { get; set; }
            /// <summary>
            /// 字段
            /// </summary>
            public string dataIndex { get; set; }
            /// <summary>
            /// 宽度
            /// </summary>
            public int width { get; set; }
        }
        /// <summary>
        /// 导出的列
        /// </summary>
        public List<ExportColumn> ExportColumns { get; set; }
        /// <summary>
        /// 筛选条件
        /// </summary>
        public List<DataFilter> ExportFilters { get; set; }
        /// <summary>
        /// 排序条件
        /// </summary>
        public List<DataSort> ExportSorters { get; set; }
        /// <summary>
        /// 所选记录(json对象)
        /// </summary>
        public string SelectedRecords { get; set; }
    }
}
