# PaperDeliverySolution

## Framework
This project is a **WPF Desktop Application** coded in **C#**. The current version is **.Net 8.0**.

## Dependency
In this project I am working on some basic patterns using several **NuGet Packages** like:
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Hosting
- Microsoft.Extensions.Logging
- CommunityToolkit.Mvvm
- Serilog

## Coding Pattern
In this project I am implementing some **coding patterns** like:
- MVVM
- SOLID

## Configuration
Currently, the application is tested in developer environment, only.

### UserAccounts
In order to access the application the user has to provide a valid ***Login*** and ***Password***. These login credentials are stored in an MS ACCCESS database.

The user must provide this section in appsettings.json. This section contains the configuration to access the database. The keys should be self-explanatory.
- DatabasePath = the complete path and file-name to the MS ACCESS database, that contains the table to store user account information
- DatabaseTable = the table that stores the user account information
- DatabasePassword = the optional password needed to access the database (the MS ACCESS database must be configured to have a password);  
  heads up! this is not the password to access a specific user account! it is the password to access the database!

```Json
  "DatabaseOptionsUsingAccess": {
    "DatabasePath": "D:\\Net\\PaperDeliveryFiles\\UserRepository.accdb",
    "DatabaseTable": "UserAccount",
    "DatabasePassword": "not needed, yet"
  }
```

The current table setup:
- the table name can vary, but it must correspond to the `DatabaseTable` in `DatabaseOptionsUsingAccess` section of your `appsettings.json` file
- the column names must be identical to this screenshot
- the column `Email` is allowing Null-Values
- all other columns must contain values

![image](https://github.com/karwenzman/PaperDeliverySolution/assets/66565927/39f93bb8-ecaf-422f-acf0-f62e4000224e)

