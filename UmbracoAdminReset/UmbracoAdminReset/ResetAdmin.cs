using System;
using System.IO;
using System.Reflection;
using System.Web.Configuration;
using Umbraco.Core;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;
using Umbraco.Web;

namespace UmbracoAdminReset
{
    /// <summary>
    /// Simple class to reset the admin user to username Admin and to a configured or default password.
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
                    UmbracoContext.Current.Application.Services.UserService.SavePassword(user, GetNewPassword());
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

        /// <summary>
        /// Gets the new password, which will either come from the web.config, or it will
        /// be the default password.
        /// </summary>
        /// <returns>
        /// The new password.
        /// </returns>
        private string GetNewPassword()
        {
            var defaultPassword = "Admin1234!";
            // For example, your web.config could contain this app setting:
            //   <add key="UmbracoAdminResetPassword" value="Admin1234!" />
            var customPassword = WebConfigurationManager.AppSettings["UmbracoAdminResetPassword"];
            var newPassword = string.IsNullOrWhiteSpace(customPassword)
                ? defaultPassword
                : customPassword;
            return newPassword;
        }
    }
}
