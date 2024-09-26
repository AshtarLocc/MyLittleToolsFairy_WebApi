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

		/// <summary>
		/// 透過ID(主鍵)查詢
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<T> Find<T>(int id) where T : class
		{
			var entity = await this.Context.Set<T>().FindAsync(id);
			if (entity == null)
			{
				throw new KeyNotFoundException($"沒有找到與 ID = {id} 匹配的實體。");
			}
			return entity;
		}

		//#region Query

		///// <summary>
		///// 透過ID(主鍵)查詢
		///// </summary>
		///// <typeparam name="T"></typeparam>
		///// <param name="id"></param>
		///// <returns></returns>
		//public T Find<T>(int id) where T : class
		//{
		//	return this.Context.Set<T>().Find(id);
		//}

		//public IQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere) where T : class
		//{
		//	throw new NotImplementedException();
		//}

		//public PagingData<T> QueryPage<T, S>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageInex, Expression<Func<T, S>> funcOrderby, bool isAsc = true) where T : class
		//{
		//	throw new NotImplementedException();
		//}

		//public IQueryable<T> Set<T>() where T : class
		//{
		//	throw new NotImplementedException();
		//}

		//#endregion Query

		//public void Delete<T>(T t) where T : class
		//{
		//	throw new NotImplementedException();
		//}

		//public void Delete<T>(int Id) where T : class
		//{
		//	throw new NotImplementedException();
		//}

		//public void Delete<T>(IEnumerable<T> tList) where T : class
		//{
		//	throw new NotImplementedException();
		//}

		//public void Excute<T>(string sql, SqlParameter[] parameters) where T : class
		//{
		//	throw new NotImplementedException();
		//}

		//public IQueryable<T> ExcuteQuery<T>(string sql, SqlParameter[] parameters) where T : class
		//{
		//	throw new NotImplementedException();
		//}

		//public T Insert<T>(T t) where T : class
		//{
		//	throw new NotImplementedException();
		//}

		//public IEnumerable<T> Insert<T>(IEnumerable<T> tList) where T : class
		//{
		//	throw new NotImplementedException();
		//}

		//public void Update<T>(T t) where T : class
		//{
		//	throw new NotImplementedException();
		//}

		//public void Update<T>(IEnumerable<T> tList) where T : class
		//{
		//	throw new NotImplementedException();
		//}
	}
}