using Ratul.Mvc.Bootstrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using $safeprojectname$.Codes;
using $safeprojectname$.Codes.Core.Services;

namespace $safeprojectname$.Codes.Persistence.Services
{
    public class ValidationMessageService : IValidationMessageService
    {
        public string GetErrorMessage(ICollection<ModelState> modelStateValues)
        {
            string errorMessage = "";
            foreach (ModelState modelState in modelStateValues)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    errorMessage += ("<br>" + error.ErrorMessage);
                }
            }
            return errorMessage;
        }

        public void StoreActionResponseMessageError(ModelStateDictionary modelState)
        {
            string message = this.GetErrorMessage(modelState.Values);
            UserSession.ActionResponseMessage = new ActionResponse(ActionResponseMessageType.Error, message);
        }

        public void StoreActionResponseMessageError(string message)
        {
            UserSession.ActionResponseMessage = new ActionResponse(ActionResponseMessageType.Error, message);
        }

        public void StoreActionResponseMessageSuccess(string message)
        {
            UserSession.ActionResponseMessage = new ActionResponse(ActionResponseMessageType.Success, message);
        }

        public void StoreActionResponseMessageInfo(string message)
        {
            UserSession.ActionResponseMessage = new ActionResponse(ActionResponseMessageType.Info, message);
        }

        public void StoreActionResponseMessageWarning(string message)
        {
            UserSession.ActionResponseMessage = new ActionResponse(ActionResponseMessageType.Warning, message);
        }
    }
}