@echo off

set source=https://api.nuget.org/v3/index.json

for /r %%f in (pack\*.symbols.nupkg) do (
	echo Pushing [92m%%f[0m to [92m%source%[0m
	dotnet nuget push "%%f" --api-key %NUGET_ORG_API_KEY% --source %source% --skip-duplicate
)