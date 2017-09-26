using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Xml;
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
                    //Make sure the provider supports change password
                    ForceAllowChangePassword();

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

        private void ForceAllowChangePassword()
        {
            try
            {
                //Create a new xml document
                XmlDocument document = new XmlDocument();

                //Keep current indentions format
                document.PreserveWhitespace = true;

                //Load the web.config file into the xml document
                var webconfigFile = HttpContext.Current.Server.MapPath("~/web.config");
                document.Load(webconfigFile);
                var userMembershipNode =
                    document.SelectSingleNode(
                        "//configuration/system.web/membership/providers/add[@name='UsersMembershipProvider']");
                if (userMembershipNode == null) return;
                var att = userMembershipNode.Attributes["allowManuallyChangingPassword"];
                if (att == null)
                {
                    att = document.CreateAttribute("allowManuallyChangingPassword");
                    userMembershipNode.Attributes.Append(att);
                }
                if (!att.Value.Equals("true"))
                {
                    att.Value = "true";
                }

                document.Save(webconfigFile);
            }
            catch (Exception ex)
            {
                LogHelper.Error<ResetAdmin>("Error during allowManuallyChangingPassword",ex);
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
