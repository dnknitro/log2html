@echo off
@rem %1 should be full version, e.g. `pack-all.cmd 1.0.0.34`

set VERSION=%1

if "%VERSION%"=="" (
    FOR /F "tokens=* USEBACKQ" %%F IN (`MaxVersion .\pack`) DO (
        SET VERSION=%%F
    )
)
echo New version [92m%version%[0m

if "%VERSION%"=="" EXIT /B -1

del .\pack\*.nupkg

dotnet pack src\log2html --include-source --configuration Release --output .\pack\ -p:PackageVersion=%VERSION%

dotnet pack src\log2html.support --include-source --configuration Release --output .\pack\ -p:PackageVersion=%VERSION%

git tag -a "%VERSION%" -m "%VERSION%"

del .\pack\log2html.%VERSION%.nupkg
del .\pack\log2html.Support.%VERSION%.nupkg