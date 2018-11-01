## Technology stack:
* Aspnet MVC 5.2.3
* SQL server
* Owin
* Aspnet Identity.
* Entity Framework Code First
* Razor
* Jquery

## Environments:
* Please make sure .NET Framework 4.6.1 or higher is installed.
* Tested on VS 2017 / VS 2015

## Run the application step by step:
* Replace connect string by your connect string at StookAgencyApp/Web.config file:
![connect-string](https://image.ibb.co/h0prnf/connect-string.png)

* After change connect string just simple run the App - the database will be generated at the first time the app is ran. 
In the case you want to restore database you can find the backup file there [https://github.com/newbiecse/StookAgencyApp/tree/master/db](https://github.com/newbiecse/StookAgencyApp/tree/master/db) - then the app will ignore generate DB if it existed.

* Please login the app with following username/password: admin@stockagency.com / p@ssword
