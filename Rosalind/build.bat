
echo off

set "projectname=Rosalind"

:: This allows the exe to be called directly from any directory.
echo Updating path.
set PATH=%PATH%;d:\code\datascience-dotnet\%projectname%\bin\Debug\net8.0
echo Path updated.

echo Clean started.
dotnet clean d:\code\datascience-dotnet\%projectname%
echo Clean completed.

echo Build started.
dotnet build d:\code\datascience-dotnet\%projectname% --configuration Debug
echo Build completed.