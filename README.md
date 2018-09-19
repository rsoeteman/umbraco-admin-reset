# Umbraco admin reset
Simple project that allows reset of the admin user(user 0 by default) by adding the UmbracoAdminReset.dll to the ~/bin folder of your Umbraco installation and request the url **/umbraco/adminreset/useractions/reset**. Handy when you inherit a site and didn't receive the credentials or when everything else fails.

The username will be reset to **Admin** and password to **Admin1234!** by default

When the password is reset it will make sure the admin user is unlocked and will delete the dll afterwards so you can login and change the credentials yourself.

If you are concerned about security, you can use the following parameters to change the defaults

    /umbraco/adminreset/useractions/reset?userId=[id of the user]&userName=[new username]&userPassword=[new password]

The DLL will make sure the Membership provider is allow to change passwords and reset the username and password. The DLL will make sure the admin user is unlocked and will delete itself afterwards so you can login and change the credentials yourself.
