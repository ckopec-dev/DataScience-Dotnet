echo Executing postbuild.bat
echo ConfigurationName: %1
echo SolutionDir: %2
echo TargetName: %3
echo Configuration: %4

rem Generate the code coverage data.
dotnet test --collect:"XPlat Code Coverage" --no-build

rem Generate the code coverage report.
rem To install reportgenerator: https://www.nuget.org/packages/dotnet-reportgenerator-globaltool
reportgenerator "-reports:TestResults\*\coverage.cobertura.xml" "-targetdir:TestCoverageReport" -reporttypes:Html