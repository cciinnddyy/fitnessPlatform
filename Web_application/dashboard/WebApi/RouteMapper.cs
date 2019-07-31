using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Web.Api;
namespace HMS.Modules.dashboard
{
    public class RouteMapper:IServiceRouteMapper
    {
       
            public void RegisterRoutes(IMapRoute mapRouteManager)
            {
                mapRouteManager.MapHttpRoute("dashboard", "default", "{controller}/{action}", new[] { "HMS.Modules.dashboard" });
            }
        //modulefolder Name, route name, ,
        
    }
}