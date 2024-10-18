namespace myLittleToolsFairy.Commons.Model
{
    public class ApiResult
    {
        /// <summary>
        /// 是否正常/成功回傳
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string? Message { get; set; }
    }

    /// <summary>
    /// 繼承自ApiResult，白話說，包含了ApiResult中的所有屬性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiDataResult<T> : ApiResult
    {
        /// <summary>
        /// 回傳數據
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// 冗餘數劇
        /// </summary>
        public object? OValue { get; set; }
    }
}