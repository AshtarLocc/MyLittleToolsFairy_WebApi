using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myLittleToolsFairy.DbModels.Models;
using myLittleToolsFairy.WebCore.SwaggerExtend;
using myLittleToolsFairy.IBusinessServices;
using myLittleToolsFairy.Commons.Controllers;
using myLittleToolsFairy.Commons.Model;
using System.Linq.Expressions;
using myLittleToolsFairy.IBusinessServices.Model;
using myLittleToolsFairy.WebApi.Model.DB;

namespace myLittleToolsFairy.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.v2))]
    public class UserController : CommonController
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
        [ProducesResponseType(typeof(ApiDataResult<UserEntity>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiDataResult<UserEntity>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _iuserService.Find<UserEntity>(id);

                return new JsonResult(new ApiDataResult<UserEntity>()
                {
                    Success = true,
                    Message = "用戶查詢(id)",
                    Data = user,
                    OValue = null
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiDataResult<UserEntity>()
                {
                    Success = false,
                    Message = $"用戶查詢(id): {ex.Message}",
                    Data = null,
                    OValue = null
                });
            }
        }

        /// <summary>
        /// User - 查詢全部
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiDataResult<UserEntity>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiDataResult<UserEntity>), StatusCodes.Status500InternalServerError)]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _iuserService.Set<UserEntity>().ToList();
                return new JsonResult(new ApiDataResult<List<UserEntity>>()
                {
                    Success = true,
                    Message = "用戶查詢",
                    Data = users,
                    OValue = null
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiDataResult<UserEntity>()
                {
                    Success = false,
                    Message = $"用戶查詢: {ex.Message}",
                    Data = null,
                    OValue = null
                });
            }
        }

        /// <summary>
        /// User - 透過 名字 與 使用者類型 查詢
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiDataResult<UserEntity>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiDataResult<UserEntity>), StatusCodes.Status500InternalServerError)]
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
                return new JsonResult(new ApiDataResult<List<UserEntity>>()
                {
                    Success = true,
                    Message = "用戶查詢(姓名/使用者類型)",
                    Data = users,
                    OValue = null
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiDataResult<UserEntity>()
                {
                    Success = false,
                    Message = $"用戶查詢(姓名/使用者類型): {ex.Message}",
                    Data = null,
                    OValue = null
                });
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
        [ProducesResponseType(typeof(ApiDataResult<PagingData<UserEntity>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiDataResult<PagingData<UserEntity>>), StatusCodes.Status500InternalServerError)]
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
                return new JsonResult(new ApiDataResult<PagingData<UserEntity>>()
                {
                    Success = true,
                    Message = "分頁查詢(使用者類型/性別)",
                    Data = result,
                    OValue = null
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiDataResult<PagingData<UserEntity>>()
                {
                    Success = false,
                    Message = $"分頁查詢(使用者類型/性別): {ex.Message}",
                    Data = null,
                    OValue = null
                });
            }
        }
    }
}