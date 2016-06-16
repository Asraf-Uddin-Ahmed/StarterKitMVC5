using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Website.Foundation.Core.Aggregates;
using Website.Foundation.Core.SearchData;
using Website.Foundation.Core.Services;
using $safeprojectname$.Codes.Core.Factories;
using $safeprojectname$.Codes.Core.Identity;
using $safeprojectname$.Configuration;
using $safeprojectname$.Models.Request;

namespace $safeprojectname$.Controllers.Resource
{
    [CustomCorsPolicy]
    [RoutePrefix("api/test")]
    public class TestController : BaseApiController
    {
        private IUserService _userSevice;
        private IUserResponseFactory _userResponseFactory;
        public TestController(IUserService userSevice, IUserResponseFactory userResponseFactory)
        {
            _userSevice = userSevice;
            _userResponseFactory = userResponseFactory;
        }

        [Authorize(Roles = "IncidentResolvers")]
        [HttpPut]
        [Route("{orderId}")]
        public IHttpActionResult Put([FromUri]string orderId)
        {
            return Ok();
        }

        [ClaimsAuthorization(ClaimType = "FTE", ClaimValue = "1")]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok();
        }

        [Route("users")]
        public IHttpActionResult GetMyUsers([FromUri] RequestSearchModel<User, UserSearch> searchModel)
        {
            return Ok(_userResponseFactory.Create(
                _userSevice.GetUserBy(searchModel.Pagination, searchModel.OrderBy), 
                searchModel.Pagination,
                searchModel.SortBy,
                _userSevice.GetTotal()));
        }

        [Route("globallog")]
        [HttpGet]
        public void TestGlobalLog()
        {
            int I = 0;
            int J = 10 / I;
        }
    }
}
