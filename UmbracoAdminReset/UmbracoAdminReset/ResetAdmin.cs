using System;
using System.IO;
using Umbraco.Core;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;
using Umbraco.Web;

namespace UmbracoAdminReset
{
    /// <summary>
    /// Simpla class to reset the admin user to username Admin and password Admin1234!
    /// </summary>
    public class ResetAdmin :ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            try
            {
                var user = UmbracoContext.Current.Application.Services.UserService.GetUserById(0);
                if (user != null)
                {
                    user.Username = "Admin";
                    user.IsApproved = true;
                    user.IsLockedOut = false;

                    //Save changes
                    UmbracoContext.Current.Application.Services.UserService.Save(user);
                    //Change password
                    UmbracoContext.Current.Application.Services.UserService.SavePassword(user, "Admin1234!");
                }

                //Delete this dll
                var fileName = IOHelper.MapPath("~/bin/UmbracoAdminReset.dll");
                File.Delete(fileName);
            }
            catch (Exception ex)
            {
                LogHelper.Error<ResetAdmin>("Error during password reset", ex);
            }
        }
    }
}
