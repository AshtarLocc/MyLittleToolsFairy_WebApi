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
        /// Excute是執行的意思，顧名思義，該方法是用來 執行查詢 ，並且會回傳 IQueryble 類型的物件，意思是「延遲加載的查詢結果集合」，白話解釋為「還未真正執行查詢，直到你對其做後續動作」。
        /// 更簡單的說，他就是你查到的那些資料，但還不能直接用，等你用.Sort()之類的方法操作他時他才會執行查詢並且變成可以用的資料，不想後續操作的話通常會.ToList()。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IQueryable<T> ExcuteQuery<T>(string sql, SqlParameter[] parameters) where T : class;

        /// <summary>
        /// Excute 單純執行語句，無回傳值。
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