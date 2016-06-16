using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Website.Foundation.Core.Aggregates;
using Website.Foundation.Core.Identity;
using $safeprojectname$.Codes.Core.Factories;
using $safeprojectname$.Models.Response;

namespace $safeprojectname$.Codes.Persistence.Factories
{
    public class ApplicationUserResponseFactory : ResponseFactory<ApplicationUserResponseModel>, IApplicationUserResponseFactory
    {
        private ApplicationUserManager _applicationUserManager;
        public ApplicationUserResponseFactory(HttpRequestMessage httpRequestMessage, 
            ApplicationUserManager applicationUserManager)
            :base(httpRequestMessage)
        {
            _applicationUserManager = applicationUserManager;
        }

        public ApplicationUserResponseModel Create(ApplicationUser applicationUser)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ApplicationUser, ApplicationUserResponseModel>()
                    .ForMember(dest => dest.Url, opt => opt.MapFrom(src => UrlHelper.Link("GetUserById", new { id = src.Id })))
                    .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => _applicationUserManager.GetRolesAsync(src.Id).Result))
                    .ForMember(dest => dest.Claims, opt => opt.MapFrom(src => _applicationUserManager.GetClaimsAsync(src.Id).Result));
            });
            return Mapper.Map<ApplicationUserResponseModel>(applicationUser);
        }

        public IEnumerable<ApplicationUserResponseModel> Create(IEnumerable<ApplicationUser> applicationUsers)
        {
            return applicationUsers.Select(u => this.Create(u));
        }
    }
}