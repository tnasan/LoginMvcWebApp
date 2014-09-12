using System;
using System.Web;
using System.Web.Security;
using System.Threading.Tasks;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using LoginMvcWebApp.Models;
using System.Linq;

[assembly: OwinStartup(typeof(LoginMvcWebApp.Startup))]

namespace LoginMvcWebApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            #region Redirect to Login if required
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Account/Login")
            });
            #endregion

            #region DB Setup
            using (LoginDataModelContainer db = new LoginDataModelContainer())
            {
                if (!db.Roles.Any())
                {
                    db.Roles.Add(new Role { Name = "Admin", Description = "Administrator" });
                    db.Roles.Add(new Role { Name = "IT", Description = "IT" });
                    db.Roles.Add(new Role { Name = "User", Description = "User" });
                    db.SaveChanges();
                }

                if (!db.Users.Any())
                {
                    db.Users.Add(new User { Firstname = "Admin", Lastname = "Admin", Username = "admin", Password = Helper.GetSHA512Hash("admin"), Roles = db.Roles.Where(x => x.Name == "Admin" || x.Name == "IT").ToList() });
                    db.Users.Add(new User { Firstname = "IT", Lastname = "IT", Username = "it", Password = Helper.GetSHA512Hash("it"), Roles = db.Roles.Where(x => x.Name == "IT").ToList() });
                    db.Users.Add(new User { Firstname = "User", Lastname = "User", Username = "user", Password = Helper.GetSHA512Hash("user"), Roles = db.Roles.Where(x => x.Name == "User").ToList() });
                    db.SaveChanges();
                }
            }
            #endregion
        }
    }
}
