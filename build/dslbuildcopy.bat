set folder1=%USERPROFILE%\AppData\Local\Microsoft\VisualStudio\10.0Exp\Extensions\nHydrate.org\nHydrate Modeler\5.0.0.0
set folder2=%USERPROFILE%\AppData\Local\Microsoft\VisualStudio\10.0\Extensions\nHydrate.org\nHydrate Modeler\5.0.0.0

REM print %2bin\nHydrate.*.dll 
REM print %folder1%

IF NOT EXIST %folder1% GOTO NOWINDIR1
	copy %2bin\nHydrate.*.dll %folder1% /Y
	REM copy %2bin\nHydrate.*.pdb %folder1% /Y
	REM copy %2VSADDIN\*.* %folder1% /Y
:NOWINDIR1

IF NOT EXIST %folder2% GOTO NOWINDIR2
	REM copy %2bin\nHydrate.*.dll %folder1% /Y
	REM copy %2bin\nHydrate.*.pdb %folder1% /Y
	REM copy %2VSADDIN\*.* %folder2% /Y
:NOWINDIR2

pause

