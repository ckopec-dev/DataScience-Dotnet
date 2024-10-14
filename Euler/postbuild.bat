
echo Executing postbuild.bat
echo ConfigurationName: %1
echo SolutionDir: %2
echo TargetName: %3
echo Configuration: %4

if "%1"=="Debug" ( 
	echo Copying output to debug directory.
) else if "%1" == "Release" (
	echo Copying output to //rm001/sata08/intranet/euler directory.
	xcopy "%2%3\bin\%4\net8.0" "p:\euler\" /s /i /y 
) else (
	echo Configuration not implemented in postbuild.
)