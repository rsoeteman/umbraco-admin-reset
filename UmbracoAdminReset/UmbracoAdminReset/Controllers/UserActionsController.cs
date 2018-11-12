using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Xml;
using Umbraco.Core.IO;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace UmbracoAdminReset.Controllers
{
    [PluginController("adminreset")]
    public class UserActionsController :UmbracoApiController
    {
        [HttpGet]
        public HttpResponseMessage Reset(int userId = -1, string userName = "Admin", string userPassword = "Admin1234!")
        {
            try
            {
                var user = Services.UserService.GetUserById(userId);
                if (user != null)
                {
                    //Make sure the provider supports change password
                    ForceAllowChangePassword();

                    user.Username = userName;
                    user.IsApproved = true;
                    user.IsLockedOut = false;

                    //Save changes
                    Services.UserService.Save(user);
                
                    //Change password
                    Services.UserService.SavePassword(user,userPassword);
                }

                //Delete this dll
                var fileName = IOHelper.MapPath("~/bin/UmbracoAdminReset.dll");
                File.Delete(fileName);
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(UserActionsController),ex,"Error during password reset") ;
            }

            var response = new HttpResponseMessage
            {
                Content = new StringContent("<html><body><h1>Password is reset</h1><p>And dll is removed from the /bin folder</p></body></html>")
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        private void ForceAllowChangePassword()
        {
            try
            {
                //Create a new xml document
                var document = new XmlDocument
                {

                    //Keep current indentions format
                    PreserveWhitespace = true
                };

                //Load the web.config file into the xml document
                var webconfigFile = IOHelper.MapPath("~/web.config");
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
                Logger.Error(typeof(UserActionsController),ex,"Error during allowManuallyChangingPassword") ;
            }
        }
    }
}
