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
using myLittleToolsFairy.ModelDto;
using AutoMapper;
using SqlSugar;

namespace myLittleToolsFairy.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.v2))]
    public class UserController : CommonController
    {
        private readonly ILogger<UserController> _logger;
        private readonly DbContext _dbContext;
        private readonly IUserService _iuserService;
        private readonly IMapper _IMapper;

        public UserController(ILogger<UserController> logger, DbContext dbContext, IUserService iuserService, IMapper iMapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _iuserService = iuserService;
            _IMapper = iMapper;
        }

        /// <summary>
        /// User - 傳入ID查詢
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiDataResult<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiDataResult<UserDto>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                // 此處 user 為 UserEntity 類型
                var user = await _iuserService.Find<UserEntity>(id);

                // UserDto userDto = 使用 IMapper 將 UserEntity類型的 user 轉換為 UserDto 類型
                UserDto userDto = _IMapper.Map<UserEntity, UserDto>(user);

                // 改回傳的型態為 ApiDataResult<UserDto>，Data中改為 userDto
                return new JsonResult(new ApiDataResult<UserDto>()
                {
                    Success = true,
                    Message = "用戶查詢(id)",
                    Data = userDto,
                    OValue = null
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiDataResult<UserDto>()
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
        [ProducesResponseType(typeof(ApiDataResult<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiDataResult<UserDto>), StatusCodes.Status500InternalServerError)]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _iuserService.Set<UserEntity>().ToList();

                List<UserDto> userDtoList = _IMapper.Map<List<UserEntity>, List<UserDto>>(users);

                return new JsonResult(new ApiDataResult<List<UserDto>>()
                {
                    Success = true,
                    Message = "用戶查詢",
                    Data = userDtoList,
                    OValue = null
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiDataResult<UserDto>()
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
        [ProducesResponseType(typeof(ApiDataResult<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiDataResult<UserDto>), StatusCodes.Status500InternalServerError)]
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

                List<UserDto> userDtoList = _IMapper.Map<List<UserEntity>, List<UserDto>>(users);

                return new JsonResult(new ApiDataResult<List<UserDto>>()
                {
                    Success = true,
                    Message = "用戶查詢(姓名/使用者類型)",
                    Data = userDtoList,
                    OValue = null
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiDataResult<UserDto>()
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
        [ProducesResponseType(typeof(ApiDataResult<PagingData<UserDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiDataResult<PagingData<UserDto>>), StatusCodes.Status500InternalServerError)]
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

                // 必須先在 AutoMapConfig 規則中再加入一條 PagingData<>的泛型映射 : CreateMap(typeof(PagingData<>), typeof(PagingData<>));
                PagingData<UserDto> userDto = _IMapper.Map<PagingData<UserDto>>(result);

                return new JsonResult(new ApiDataResult<PagingData<UserDto>>()
                {
                    Success = true,
                    Message = "分頁查詢(使用者類型/性別)",
                    Data = userDto,
                    OValue = null
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiDataResult<PagingData<UserDto>>()
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