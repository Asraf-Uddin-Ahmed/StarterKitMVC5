using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace $safeprojectname$.Codes.Core.Services
{
    public interface IValidationMessageService
    {
        string GetErrorMessage(ICollection<ModelState> modelStateValues);
        void StoreActionResponseMessageError(string message);
        void StoreActionResponseMessageError(ModelStateDictionary modelState);
        void StoreActionResponseMessageInfo(string message);
        void StoreActionResponseMessageSuccess(string message);
        void StoreActionResponseMessageWarning(string message);
    }
}
