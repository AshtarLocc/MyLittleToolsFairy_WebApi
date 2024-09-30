using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using myLittleToolsFairy.IBusinessServices.Model;

namespace myLittleToolsFairy.IBusinessServices
{
    public interface IBaseService
    {
        #region Query

        /// <summary>
        /// 透過ID(主鍵)查詢
        /// 約束泛型只能接受 class 類型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<T> Find<T>(int id) where T : class;

        /// <summary>
        /// 整張資料表的資料全數返回供使用者操作，不安全，少用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> Set<T>() where T : class;

        /// <summary>
        /// 傳入條件查詢特定資料表並返回查詢結果
        /// public {查詢結果<T>} Query<T>({接收T並返回bool的泛型委託，且可被轉譯為表達式樹} funcWhere) where T : class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        public IQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere) where T : class;

        public PagingData<T> QueryPage<T, S>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, S>> funcOrderby, bool isAsc = true) where T : class;

        #endregion Query

        //#region Insert

        ///// <summary>
        ///// 單筆資料新增
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public T Insert<T>(T t) where T : class;

        ///// <summary>
        ///// 多筆資料新增
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="tList"></param>
        ///// <returns></returns>
        //public IEnumerable<T> Insert<T>(IEnumerable<T> tList) where T : class;

        //#endregion Insert

        #region Sql Execute

        /// <summary>
        /// 執行SQL語法，返回「IQueryble 延遲加載的查詢結果集合」，意思是還沒有真正執行查詢，並且支持在真正執行查詢(例如調用First()等等)之前再進行其他操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IQueryable<T> ExcuteQuery<T>(string sql, SqlParameter[] parameters) where T : class;

        /// <summary>
        /// 執行SQL語法，沒有返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        public void Excute<T>(string sql, SqlParameter[] parameters) where T : class;

        #endregion Sql Execute

        //#region Update

        //public void Update<T>(T t) where T : class;

        //public void Update<T>(IEnumerable<T> tList) where T : class;

        //#endregion Update

        //#region Delete

        //public void Delete<T>(T t) where T : class;

        //public void Delete<T>(int Id) where T : class;

        //public void Delete<T>(IEnumerable<T> tList) where T : class;

        //#endregion Delete

        //public void Add();

        //public void Delete();

        //public void Update();

        //public void Query();
    }
}