using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myLittleToolsFairy.DbModels;
using myLittleToolsFairy.DbModels.Models;
using myLittleToolsFairy.WebCore.SwaggerExtend;
using myLittleToolsFairy.IBusinessServices;
using myLittleToolsFairy.BusinessServices;

namespace myLittleToolsFairy.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.v1))]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly DbContext _dbContext;
        private readonly IUserService _iuserService;

        public TestController(ILogger<TestController> logger, DbContext dbContext, IUserService iuserService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _iuserService = iuserService;
        }

        [HttpGet]
        public object Get()
        {
            UserEntity user = _dbContext.Set<UserEntity>().OrderByDescending(x => x.UserId).FirstOrDefault();
            return new JsonResult(user);
        }
    }
}