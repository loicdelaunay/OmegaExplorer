cd ../../OmegaExplorer.Server/
dotnet build .\OmegaExplorer.Server.csproj

echo Starting the server in the background ...
cd bin/Debug/net9.0

:: Launch the server in the background with a captured PID
start /b cmd /c dotnet ./OmegaExplorer.Server.dll nswag && echo Server started successfully

:: Wait until the server is ready
echo Waiting for the start of the server ...
TIMEOUT /T 5

echo Download the swagger.json ...
cd ../../../../_Tools/nswag-converter

set MAX_TRIES=5
set CURRENT_TRY=0

:DOWNLOAD_LOOP
set /a CURRENT_TRY+=1
echo Attempt %CURRENT_TRY%/%MAX_TRIES%...

curl -v -o swagger.json http://localhost:5000/swagger/v1/swagger.json 2>&1
if %errorlevel% equ 0 (
    echo Swagger.json file successfully downloaded.
    goto DOWNLOAD_SUCCESS
) else (
    echo Failure of the attempt %CURRENT_TRY%
    if %CURRENT_TRY% geq %MAX_TRIES% (
        echo All attempts have failed.
        goto END
    )
    echo New attempt in 2 seconds ...
    TIMEOUT /T 2 >nul
    goto DOWNLOAD_LOOP
)

:DOWNLOAD_SUCCESS
Net90\dotnet-nswag.exe run nswag.json
echo Generation completed, server stop ...

:END
:: Stop the server using Taskkill with counter
set CLOSED_PROCESSES=0
for /f "tokens=2 delims=," %%p in ('tasklist /fi "imagename eq dotnet.exe" /fo csv ^| findstr /i "OmegaExplorer.Server"') do (
    taskkill /PID %%p /F
    if %errorlevel% equ 0 set /a CLOSED_PROCESSES+=1
)

echo %CLOSED_PROCESSES% process(es) stopped related to OmegaExplorer.
echo Server stopped.
echo Completed.
