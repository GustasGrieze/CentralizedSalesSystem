# CentralSalesSystem

## Development Setup
For any platforms different from windows, you may choose to install the MSSQL server properly instead of using LocalDB, so you have to adjust the connection string. Exemplary *appsettings.Development.json*

```
{
  "JWT": {
    "Key": "this_is_a_super_long_dev_secret_key_12",
    "Issuer": "CentralizedSalesSystem",
    "Audience": "CentralizedSalesSystemClients"
  },

  "DatabaseProvider": "LocalDB",

  //here for windows
  "ConnectionStrings": { 
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CentralizedSalesDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },

  // or if you have mssql proper server setup, something like this:
  "ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1,1433;Database=CentralizedSalesDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
  },

  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }


}

```

## Testing
To test the endpoints in a manual way, before we have adjusted the authorization, make sure to comment `[Authorize]` annotations on controller. DO NOT REMOVE THEM FROM THE REPO CODE!

Before we can write integration tests, we'll have to wait for the authorization to be fixed

## Testing part 2
Hit the login enpoint with

```

{

&nbsp; "email": "admin@ex.com",

&nbsp; "password": "pass"

}

```

to obtain the jwt token. 

Then add into the authorize input as just the raw token with strings and numbers eyJhbGciOiJIUzI1NiIsIn.... ect
