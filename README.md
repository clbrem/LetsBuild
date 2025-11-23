# LetsBuild

This is a simple project to practice using [Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/).

## Prerequisites
* [.NET SDK](https://dotnet.microsoft.com/download) (version 10.0 or later)
* [Docker](https://www.docker.com/get-started) or [Rancher Desktop](https://rancherdesktop.io/) (for containerization)

## Getting Started
1. Clone the repository
2. Make sure that the Docker or Rancher daemon is running
3. From the root of the project, run the following command:
```bash
dotnet run --project AppHost/AppHost.csproj --launch-profile http
```
4. (Optional) You may also run the application with https, but this requires additional configuration in .NET.

## Resources Created
* A mySQL database server hosted in a docker container.
* A mySQL database named `LetsBuild`, populated with a few tables and some sample data.


