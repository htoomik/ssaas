# Overview

SSaaS stands for "ScreenShot as a Service" and is a service for taking screenshots of web sites.

It has two main components: SSaaS.UI and SSaaS.Worker.

# SSaaS.UI

The console application that users interact with, to queue new requests and get the status of previous requests.

## Command line usage examples
```
dotnet run queue -url https://www.google.com/
dotnet run queue -file /Users/helen/temp/urls.txt
dotnet run status 5
```

# SSaaS.Worker

The application that does the actual work. Watches the database for new, unprocessed requests and processes them.

Start it using `dotnet run` and keep it running in the background, so it can process the requests queued via SSaaS.UI.

# Technologies used

SQLite for the database.

Selenium WebDriver with chromedriver for taking screenshots.
