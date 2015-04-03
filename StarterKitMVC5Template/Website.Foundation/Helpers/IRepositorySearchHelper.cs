using System;
namespace $safeprojectname$.Helpers
{
    public interface IRepositorySearchHelper
    {
        bool IsAllPropertyNull<TSearch>(TSearch obj);
    }
}
