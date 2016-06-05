using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Aggregates;

namespace $safeprojectname$.Core.Services.Email
{
    public interface IConfirmUserMessageBuilder : IMessageBuilder
    {
        void Build(User newUser, string url);
    }
}
