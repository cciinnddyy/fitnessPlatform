using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Web.Api;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.Net.Http.Formatting;
namespace HMS.Modules.dashboard
{
    public class ServiceController:DnnApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage HelloWorld()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Hello World!");
        }

        /* 加密 */
        [RequireHost]
        [HttpGet]
        public HttpResponseMessage HelloWorldForSecurity()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "You Got It!");
        }

    }
}