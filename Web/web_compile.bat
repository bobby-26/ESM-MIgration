@echo off
pushd "%~dp0"
set path=%path%;E:\Phoenix\Utilities\xsd;

FORFILES /P %drive.letter%\Phoenix\Working\Build\Web\Build\App_Code\Xsd\ /S /M *.xsd /C "cmd /c XSD @File /CLASSES /language:C#" >> .\log\%logfolder%\phoenix_xsd_class_%datetime%.log

move /Y *.cs %drive.letter%\Phoenix\Working\Build\Web\Build\App_Code\reports\ >> .\log\%logfolder%\phoenix_xsd_class_%datetime%.log

cls
pushd "%~dp0"
set drive.letter=%cd:~0,2%

set dd=%date:~7,2%
set mm=%date:~4,2%
set yy=%date:~10,4%

set hour=%time:~0,2%
if "%hour:~0,1%" == " " set hour=0%hour:~1,1%

set min=%time:~3,2%
if "%min:~0,1%" == " " set min=0%min:~1,1%

set secs=%time:~6,2%
if "%secs:~0,1%" == " " set secs=0%secs:~1,1%

set datetime=%yy%%mm%%dd%_%hour%%min%%secs%
set logfolder=%yy%%mm%%dd%

echo datetime=%datetime%

if not exist (.\log\%logfolder%) md .\log\%logfolder%

set appdir=%drive.letter%\Phoenix
set appdir1=%drive.letter%\Phoenix\Utilities\nant-0.92-bin\nant-0.92\bin
set path=%path%;%appdir%\Utilities\nant-0.92-bin\nant-0.92\bin;
echo %logfolder%\web_compile_%datetime%.log > logfile.txt

::%appdir1%\nant.exe /f:web_compile.build -logger:NAnt.Core.XmlLogger -logfile:.\log\%logfolder%\web_compile_%datetime%.XML

%appdir1%\nant.exe /f:web_compile.build -l:.\log\%logfolder%\web_compile_%datetime%.log

%appdir1%\nant.exe /f:web_buildcheck.build -l:.\log\%logfolder%\web_buildcheck_%datetime%.log

IF EXIST "E:\Phoenix\Control\Build\Buildstatus.txt" (

::start "E:\Phoenix\Control\Build\Web\web_compile.bat"

::start "E:\Phoenix\Control\Build\SQL\db_package.bat"

) 

popd

