namespace myLittleToolsFairy.IBusinessServices.Model
{
    public class PagingData<T> where T : class
    {
        /// <summary>
        /// 總資料筆數
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 當前頁數
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每頁筆數
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 主體資料類別，不固定，因此是泛型
        /// </summary>
        public List<T>? DataList { get; set; }

        /// <summary>
        /// 查詢條件
        /// </summary>
        public string? SearchString { get; set; }
    }
}