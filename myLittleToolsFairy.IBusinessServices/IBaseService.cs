using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myLittleToolsFairy.IBusinessServices
{
    public interface IBaseService
    {
        #region Query

        /// <summary>
        /// 約束泛型只能接受 class 類型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Find<T>(int id) where T : class;

        // Find<T>代表可以查找任何類型的物件，根據你傳入的類別來決定

        #endregion Query

        //public void Add();

        //public void Delete();

        //public void Update();

        //public void Query();
    }
}