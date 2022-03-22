@rem %1 should be full version, e.g. `pack-all.cmd 1.0.0.34`

set VERSION=%1

del .\pack\*.nupkg

dotnet pack src\log2html --include-source --configuration Release --output .\pack\ -p:PackageVersion=%VERSION%

dotnet pack src\log2html.support --include-source --configuration Release --output .\pack\ -p:PackageVersion=%VERSION%

del .\pack\log2html.%VERSION%.nupkg
del .\pack\log2html.Support.%VERSION%.nupkg