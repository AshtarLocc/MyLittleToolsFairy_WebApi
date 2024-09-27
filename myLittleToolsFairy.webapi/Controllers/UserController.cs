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

        [HttpGet("{name}/{userType}")]
        [ProducesResponseType(typeof(UserEntity), StatusCodes.Status200OK)]
        public IActionResult QueryUsersByNameAndUserType(string? name, int? userType)
        {
            // 使用 nuget 套件 Linq.core 的 PredicateBuilder 來組合多個查詢條件，創建一個新的且預設為true
            var predicate = PredicateBuilder.New<UserEntity>(u => true);
            if (!string.IsNullOrWhiteSpace(name))
            {
                predicate = predicate.And(u => u.Name.Contains(name));
            }
            if (userType.HasValue)
            {
                predicate = predicate.And(u => u.UserType == userType.Value);
            }
            try
            {
                var user = _iuserService.Set<UserEntity>().AsExpandable().Where(predicate);
                return HandleResponse(user);
            }
            catch (Exception ex)
            {
                return HandleError(ex.Message);
            }
        }
    }
}