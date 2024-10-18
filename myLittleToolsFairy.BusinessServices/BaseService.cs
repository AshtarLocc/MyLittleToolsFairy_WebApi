using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using myLittleToolsFairy.IBusinessServices;
using myLittleToolsFairy.IBusinessServices.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace myLittleToolsFairy.BusinessServices
{
    public class BaseService : IBaseService
    {
        // protected 屬性只能在該類別和其的子類別中存取，換句話說，該類別所有的子類別都可以不必重複DI注入Context，可以直接繼承父類別
        protected DbContext Context { get; set; }

        // 建構函式，代表當 BaseService 建構時，必須傳入一個 DbContext 的實例傳入，白話講就是你在調用我的時候一定要告訴我你要我操作哪一個資料庫
        public BaseService(DbContext context)
        {
            Context = context;
        }

        #region Query

        /// <summary>
        /// 透過ID(主鍵)查詢
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T> Find<T>(int id) where T : class
        {
            return await this.Context.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// 整張表查詢，不安全，少用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> Set<T>() where T : class
        {
            return this.Context.Set<T>();
        }

        /// <summary>
        /// 傳入參數查詢，使用.Where()可以篩選條件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        public IQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere) where T : class
        {
            return this.Context.Set<T>().Where(funcWhere);
        }

        public PagingData<T> QueryPage<T, S>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, S>> funcOrderBy, bool isAsc = true) where T : class
        {
            // 定義要查詢的類別
            var list = Set<T>();

            // 若有傳入搜尋條件，就將條件代入並使用Where()來查詢list
            if (funcWhere != null)
            {
                list = list.Where<T>(funcWhere);
            }
            // 根據isAsc決定是使用OrderBy() 升冪排序，還是使用OrderByDescending() 降冪排序，並用funcOrderBy作為排序的依據
            if (isAsc)
            {
                list = list.OrderBy(funcOrderBy);
            }
            else
            {
                list = list.OrderByDescending(funcOrderBy);
            }

            // pageIndex : 傳入的參數，指 當前頁數 。  pageSize : 傳入的參數，指 每頁筆數 。
            PagingData<T> result = new PagingData<T>()
            {
                // 使用.Skip()方法跳過指定的「資料筆數」，將 pageIndex 當前頁數 -1 即可得知在當頁之前還有多少其他需要跳過的頁，再將需跳過的頁數與 pageSize 相乘即可得知欲跳過的總資料筆數。
                // 使用Take()方法來從「被跳過的資料筆數」後取出指定筆數的資料，舉例來說，若已知跳過100筆，且每頁有20筆資料，那麼Take(pageSize)就會從前100筆之後開始拿出20資料。
                DataList = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                // list 是尚未進行分頁處理的原始查詢結果，計算總筆數即可
                RecordCount = list.Count()
            };
            return result;
        }

        #endregion Query

        #region Insert

        //public T Insert<T>(T t) where T : class
        //{
        //    this.Context.Set<T>().Add(t);
        //    this.co
        //    return t;
        //}

        //public IEnumerable<T> Insert<T>(IEnumerable<T> tList) where T : class
        //{
        //    throw new NotImplementedException();
        //}

        #endregion Insert

        #region Other

        public void Commit()
        {
            Context.SaveChanges();
        }

        public IQueryable<T> ExecuteQuery<T>(string sql, SqlParameter[] parameters) where T : class
        {
            return this.Context.Set<T>().FromSqlRaw(sql, parameters);
        }

        public void Execute<T>(string sql, SqlParameter[] parameters) where T : class
        {
            // IDbContextTransaction : Entity Framework Core 自帶，處理交易行為的接口。
            IDbContextTransaction trans = null;
            try
            {
                // BeginTransaction() 開始交易的意思。
                trans = Context.Database.BeginTransaction();
                // 執行查詢
                this.Context.Database.ExecuteSqlRaw(sql, parameters);
                // 用我們自己定義的 Commit() 方法來 儲存改動
                trans.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                throw;
            }
        }

        /// <summary>
        /// 釋放Context的資源
        /// 聲明為virtual，代表可以覆寫，在必要時可以自訂義Dispose的邏輯。
        /// </summary>
        public virtual void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }

        #endregion Other
    }
}