using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using $safeprojectname$.Codes.Core.Factories;
using $safeprojectname$.Models.Response;

namespace $safeprojectname$.Codes.Persistence.Factories
{
    public class IdentityRoleResponseFactory : ResponseFactory<IdentityRoleResponseModel>, IIdentityRoleResponseFactory
    {
        public IdentityRoleResponseFactory(HttpRequestMessage httpRequestMessage)
            :base(httpRequestMessage)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<IdentityRole, IdentityRoleResponseModel>()
                    .ForMember(dest => dest.Url, opt => opt.MapFrom(src => UrlHelper.Link("GetRoleById", new { id = src.Id })));
            });
        }

        public IdentityRoleResponseModel Create(IdentityRole identityRole)
        {
            return Mapper.Map<IdentityRoleResponseModel>(identityRole);
        }

        public IEnumerable<IdentityRoleResponseModel> Create(IEnumerable<IdentityRole> identityRoles)
        {
            return identityRoles.Select(r => this.Create(r));
        }
    }
}