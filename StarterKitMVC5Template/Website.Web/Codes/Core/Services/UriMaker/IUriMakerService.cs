using System;
namespace $safeprojectname$.Codes.Core.Services.UriMaker
{
    // Director of UriBuilder
    public interface IUriMakerService
    {
        /// <summary>
        /// Root URL of the site
        /// </summary>
        string SiteUrl { get; }
        string GetFullUri(IUriBuilder uriBuilder);
        string GetRelativeUri(IUriBuilder uriBuilder);
    }
}
