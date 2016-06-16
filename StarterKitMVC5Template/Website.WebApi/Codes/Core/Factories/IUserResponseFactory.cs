using System;
using System.Collections.Generic;
using Website.Foundation.Core.Aggregates;
using Website.Foundation.Core.SearchData;
using $safeprojectname$.Models.Common;
using $safeprojectname$.Models.Request;
using $safeprojectname$.Models.Response;
namespace $safeprojectname$.Codes.Core.Factories
{
    public interface IUserResponseFactory
    {
        ResponseCollectionModel<UserResponseModel> Create(IEnumerable<User> users, Pagination pagination, SortBy sortBy, int totalItem);
        UserResponseModel Create(User user);
    }
}
