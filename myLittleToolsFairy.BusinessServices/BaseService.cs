using Microsoft.EntityFrameworkCore;
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
            var list = Set<T>();
            if (funcWhere != null)
            {
                list = list.Where<T>(funcWhere);
            }
            if (isAsc)
            {
                list = list.OrderBy(funcOrderBy);
            }
            else
            {
                list = list.OrderByDescending(funcOrderBy);
            }
            PagingData<T> result = new PagingData<T>()
            {
                DataList = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                RecordCount = list.Count()
            };
            return result;
        }

        #endregion Query
    }
}