using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using $safeprojectname$.Codes.Core.Factories;
using $safeprojectname$.Models.Response;
using Website.Foundation.Core.Aggregates.Identity;
using $safeprojectname$.Models.Response.Aggregates;
using $safeprojectname$.Codes.Core.Factories.Aggregates;
using $safeprojectname$.Codes.Core.Constant;

namespace $safeprojectname$.Codes.Persistence.Factories.Aggregates
{
    public class IdentityRoleResponseFactory : ResponseFactory<IdentityRoleResponseModel>, IIdentityRoleResponseFactory
    {
        private MapperConfiguration _mapperConfiguration;
        public IdentityRoleResponseFactory(HttpRequestMessage httpRequestMessage)
            : base(httpRequestMessage)
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CustomRole, IdentityRoleResponseModel>()
                    .ForMember(dest => dest.Url, opt => opt.MapFrom(src => UrlHelper.Link(UriName.Identity.Roles.GET_ROLE, new { id = src.Id })));
            }); 
        }

        public IdentityRoleResponseModel Create(CustomRole identityRole)
        {
            return _mapperConfiguration.CreateMapper().Map<IdentityRoleResponseModel>(identityRole);
        }

        public ICollection<IdentityRoleResponseModel> Create(ICollection<CustomRole> identityRoles)
        {
            return identityRoles.Select(r => this.Create(r)).ToList();
        }
    }
}