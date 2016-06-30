using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Website.Foundation.Core.Aggregates;
using Website.Foundation.Core.SearchData;
using $safeprojectname$.Codes.Core.Factories;
using $safeprojectname$.Models.Common;
using $safeprojectname$.Models.Request;
using $safeprojectname$.Models.Response;

namespace $safeprojectname$.Codes.Persistence.Factories
{
    public class UserResponseFactory : ResponseFactory<UserResponseModel>, IUserResponseFactory
    {
        public UserResponseFactory(HttpRequestMessage httpRequestMessage)
            :base(httpRequestMessage)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<User, UserResponseModel>()
                    .ForMember(dest => dest.Url, opt => opt.UseValue<string>("#NoUrl"));//opt.MapFrom(src => UrlHelper.Link("#NoUrl", new { id = src.ID }))
            });
        }

        public UserResponseModel Create(User user)
        {
            return Mapper.Map<UserResponseModel>(user);
        }

        public ResponseCollectionModel<UserResponseModel> Create(IEnumerable<User> users, Pagination pagination, SortBy sortBy, int totalItem)
        {
            return base.Create(users.Select(r => this.Create(r)), pagination, sortBy, totalItem);
        }
    }
}