using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters;
using myLittleToolsFairy.DbModels.Models;

namespace myLittleToolsFairy.WebApi
{
    //public class ApiResponseMetadataProvider : IApiResponseMetadataProvider
    public class ApiResponseMetadataProvider
    {
        //public int StatusCode { get; set; } = StatusCodes.Status200OK;

        //public Type Type => typeof(ProblemDetails);

        //public void SetResponseMetadata(ActionDescriptor actionDescriptor, ApiResponseMetadata metadata)
        //{
        //    // 設置成功響應
        //    metadata.ResponseTypes.Add(typeof(UserEntity), StatusCodes.Status200OK);
        //    metadata.ResponseTypes.Add(typeof(ProblemDetails), StatusCodes.Status404NotFound);
        //    metadata.ResponseTypes.Add(typeof(ProblemDetails), StatusCodes.Status500InternalServerError);
        //}

        //public void SetContentTypes(MediaTypeCollection contentTypes)
        //{
        //    contentTypes.Add("application/json");
        //}
    }
}