
echo off

:: To install reportgenerator: https://www.nuget.org/packages/dotnet-reportgenerator-globaltool

set "projectname=UnitTests"

echo Clean started.
dotnet clean d:\code\datascience-dotnet\%projectname%
echo Clean completed.

echo Build started.
dotnet build d:\code\datascience-dotnet\%projectname% --configuration Debug
echo Build completed.

echo Copying settings. 
copy d:\code\secrets\%projectname%\appsettings.json d:\code\datascience-dotnet\%projectname%\bin\Debug\net8.0 /y
echo Copy completed.

echo Deleting old code coverage data.
rmdir /s /q TestResults
echo Old data deleted.

echo Deleting old code coverage report.
rmdir /s /q TestCoverageReport
echo Old report deleted.

echo Generating the code coverage data.
dotnet test --collect:"XPlat Code Coverage" --no-build
echo Data generated.

echo Generating the code coverage report.
reportgenerator "-reports:TestResults\*\coverage.cobertura.xml" "-targetdir:TestCoverageReport" -reporttypes:Html
echo Report generated.