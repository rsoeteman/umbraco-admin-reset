# Umbraco admin reset
Simple project that allows reset of the admin user(user 0) username and password by adding the UmbracoAdminReset.dll to the ~/bin folder of your Umbraco installation. Handy when you inherit a site and didn't receive the credentials.

The username will be reset to **Admin** and password to **Admin1234!**

During the startup of the site UmbracoAdminReset will reset the username and password, will make sure the admin user is unlocked and will delete itself afterwards so you can login and change the credentials yourself.

If you are concerned about security, you can specify a custom password in the app settings of your web.config before you add the DLL. For example:

```
<add key="UmbracoAdminResetPassword" value="MyCustomPassword1234!" />
```

The DLL will make sure the Membership provider is allow to change passwords and reset the username and password. The DLL will make sure the admin user is unlocked and will delete itself afterwards so you can login and change the credentials yourself.
