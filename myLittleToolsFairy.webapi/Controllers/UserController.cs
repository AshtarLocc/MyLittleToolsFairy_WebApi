using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myLittleToolsFairy.DbModels;
using myLittleToolsFairy.DbModels.Models;
using myLittleToolsFairy.WebCore.SwaggerExtend;
using myLittleToolsFairy.IBusinessServices;
using myLittleToolsFairy.BusinessServices;
using myLittleToolsFairy.WebApi.Model.DB;
using System.Linq.Expressions;
using LinqKit;
using LinqKit.Core;
using myLittleToolsFairy.IBusinessServices.Model;
using System.Xml;

namespace myLittleToolsFairy.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.v1))]
    public class UserController : BaseController
    // 改繼承
    {
        private readonly ILogger<UserController> _logger;
        private readonly DbContext _dbContext;
        private readonly IUserService _iuserService;

        public UserController(ILogger<UserController> logger, DbContext dbContext, IUserService iuserService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _iuserService = iuserService;
        }

        /// <summary>
        /// User - 傳入ID查詢
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserEntity), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _iuserService.Find<UserEntity>(id);

                return HandleResponse(user);
            }
            catch (Exception ex)
            {
                return HandleError(ex.Message);
            }
        }

        /// <summary>
        /// User - 查詢全部
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(UserEntity), StatusCodes.Status200OK)]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _iuserService.Set<UserEntity>();
                return HandleResponse(users);
            }
            catch (Exception ex)
            {
                return HandleError(ex.Message);
            }
        }

        /// <summary>
        /// User - 透過 名字 與 使用者類型 查詢
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(UserEntity), StatusCodes.Status200OK)]
        public IActionResult QueryUsersByNameAndUserType([FromQuery] string? name, [FromQuery] int? userType)
        {
            var query = _iuserService.Set<UserEntity>().AsQueryable();

            // 根據條件動態組合查詢
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(u => u.Name.Contains(name));
            }

            if (userType.HasValue)
            {
                query = query.Where(u => u.UserType == userType.Value);
            }
            try
            {
                var users = query.ToList();
                return HandleResponse(users);
            }
            catch (Exception ex)
            {
                return HandleError(ex.Message);
            }
        }

        /// <summary>
        /// User - 透過 使用者類型 與 性別 分頁查詢
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="sex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet("page")]
        [ProducesResponseType(typeof(PagingData<UserEntity>), StatusCodes.Status200OK)]
        public IActionResult QueryPageUsersByNameAndUserType([FromQuery] int? userType, [FromQuery] int? sex, [FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            try
            {
                // 組合搜尋條件
                Expression<Func<UserEntity, bool>> searchCondition = x =>
            (!userType.HasValue || x.UserType == userType) &&
            (!sex.HasValue || x.Sex == sex);

                Expression<Func<UserEntity, string>> orderBy = x => x.Name;
                var result = _iuserService.QueryPage(searchCondition, pageSize, pageIndex, orderBy, true);
                return HandleResponse(result.DataList);
            }
            catch (Exception ex)
            {
                return HandleError(ex.Message);
            }
        }
    }
}