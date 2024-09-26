using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using myLittleToolsFairy.IBusinessServices;

namespace myLittleToolsFairy.BusinessServices
{
	public class UserService : BaseService, IUserService
	{
		public UserService(DbContext context) : base(context)
		{
		}
	}
}