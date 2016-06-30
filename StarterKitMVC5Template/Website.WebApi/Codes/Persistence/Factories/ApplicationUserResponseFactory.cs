using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Website.Foundation.Core.Aggregates;
using Website.Identity.Managers;
using Website.Identity.Aggregates;
using $safeprojectname$.Codes.Core.Factories;
using $safeprojectname$.Models.Response;

namespace $safeprojectname$.Codes.Persistence.Factories
{
    public class ApplicationUserResponseFactory : ResponseFactory<ApplicationUserResponseModel>, IApplicationUserResponseFactory
    {
        public ApplicationUserResponseFactory(HttpRequestMessage httpRequestMessage)
            :base(httpRequestMessage)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ApplicationUser, ApplicationUserResponseModel>()
                    .ForMember(dest => dest.Url, opt => opt.MapFrom(src => UrlHelper.Link("GetUserById", new { id = src.Id })))
                    .ForMember(dest => dest.RoleUrl, opt => opt.MapFrom(src => UrlHelper.Link("GetRoleByUserID", new { userID = src.Id })))
                    .ForMember(dest => dest.ClaimUrl, opt => opt.MapFrom(src => UrlHelper.Link("GetClaimByUserID", new { userID = src.Id })));
            });
        }

        public ApplicationUserResponseModel Create(ApplicationUser applicationUser)
        {
            return Mapper.Map<ApplicationUserResponseModel>(applicationUser);
        }

        public IEnumerable<ApplicationUserResponseModel> Create(IEnumerable<ApplicationUser> applicationUsers)
        {
            return applicationUsers.Select(u => this.Create(u));
        }
    }
}