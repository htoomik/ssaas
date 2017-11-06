# Overview

SSaaS stands for "ScreenShot as a Service" and is a service for taking screenshots of web sites.

# SSaaS.UI

The console application that users interact with, to queue new requests and get the status of previous requests.

## Command line usage examples
```
dotnet run --project SSaaS.UI/SSaaS.UI.csproj queue -url https://www.google.com/
dotnet run --project SSaaS.UI/SSaaS.UI.csproj queue -file /Users/helen/temp/urls.txt
dotnet run --project SSaaS.UI/SSaaS.UI.csproj status 5
```

# SSaaS.Worker

The application that does the actual work. Watches the database for new, unprocessed requests and processes them.

# Technologies used

SQLite for the database.

Selenium WebDriver with chromedriver for taking screenshots.