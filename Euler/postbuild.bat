
echo Executing postbuild.bat
echo ConfigurationName: %1
echo SolutionDir: %2
echo TargetName: %3
echo Configuration: %4

if "%1"=="Debug" ( 
	echo Copying output to debug directory.
) else (
	echo Configuration not implemented in postbuild.
)