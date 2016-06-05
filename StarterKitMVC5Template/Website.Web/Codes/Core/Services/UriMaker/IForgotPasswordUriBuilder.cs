using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Codes.Core.Services.UriMaker
{
    public interface IForgotPasswordUriBuilder : IUriBuilder
    {
        void Build(string verificationCode);
    }
}
