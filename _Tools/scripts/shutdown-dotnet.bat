@echo off
echo Stopping .NET processes...

:: Stop .NET runtime and its processes
taskkill /F /IM dotnet.exe /T

:: Stop IIS/ASP.NET processes
taskkill /F /IM w3wp.exe /T
taskkill /F /IM iisexpress.exe /T

:: Stop other potential .NET processes
taskkill /F /IM WebDev.WebServer.exe /T
taskkill /F /IM MSBuild.exe /T

echo Stopping .NET processes completed.
