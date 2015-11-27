# Umbraco admin reset
Simple project that allows reset of the admin user(user 0) username and password by adding a dll to the ~/bin folder of your Umbraco installation. Handy when you inherit a site and didn't receive the credentials.

Username will be reset to **Admin** and password to **Admin1234!**

The DLL will reset the username and password, will make sure the admin user is unlocked and will delete itself afterwards so you can login and change the credentials yourself.


