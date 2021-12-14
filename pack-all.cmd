@rem %1 should be full version, e.g. `pack-all.cmd 1.0.0.34`

del .\pack\*.nupkg

dotnet pack src\log2html --include-source --configuration Release --output .\pack\ -p:PackageVersion=%1

dotnet pack src\log2html.support --include-source --configuration Release --output .\pack\ -p:PackageVersion=%1