using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace $safeprojectname$.Codes.Service
{
    public interface IValidationMessageService
    {
        string GetErrorMessage(ICollection<ModelState> modelStateCollection);
        void StoreActionResponseMessageError(string message);
        void StoreActionResponseMessageError(ICollection<ModelState> modelStateCollection);
        void StoreActionResponseMessageInfo(string message);
        void StoreActionResponseMessageSuccess(string message);
        void StoreActionResponseMessageWarning(string message);
    }
}
