using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Security.Principal;
using LoginMvcWebApp.Models;

namespace LoginMvcWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            PostAuthenticateRequest += Application_PostAuthenticateRequest;
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                using (LoginDataModelContainer db = new LoginDataModelContainer())
                {
                    if (db.Users.Any(x=>x.Username == User.Identity.Name))
                    {
                        GenericIdentity identity = new GenericIdentity(User.Identity.Name);
                        GenericPrincipal principal = new GenericPrincipal(identity, db.Users.Single(x=>x.Username == User.Identity.Name).Roles.Select(x=>x.Name).ToArray());

                        Context.User = principal;
                        System.Threading.Thread.CurrentPrincipal = principal;
                    }
                }
            }
        }
    }
}
