using BloodSearch.Core.Models;
using BloodSearch.Models.Api.Models.Auth;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Web.Services;

namespace Web.Infrastructure {

    public class AuthFilter : ActionFilterAttribute {

        public override void OnActionExecuting(HttpActionContext actionContext) {

            var model = (AuthRequest)actionContext.ActionArguments["model"];

            if (new UserServices().UserIsNotAuthorized(model.Token)) {
                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.OK,
                    new BaseResponse() {
                        Success = false,
                        ErrMessages = new List<KeyMsg>() {
                            ResponseError.GetError(TypeError.UserIsNotAuthorized)
                        }
                    },
                    actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                );
            }
            base.OnActionExecuting(actionContext);
        }
    }
}