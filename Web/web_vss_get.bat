@echo off
pushd "%~dp0"
cls
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

set path=%path%;%appdir%\Utilities\nant-0.85-bin\nant-0.85\bin;
nant /f:web_vss_get.build -l:.\log\%logfolder%\web_vss_get_%datetime%.log

::set path=%path%;E:\Software\xsd;

::FORFILES /P %drive.letter%\Phoenix\Working\Solution\Web\App_Code\Xsd\ /S /M *.xsd /C "cmd /c XSD @File /CLASSES /language:C#" >> .\log\%logfolder%\phoenix_xsd_class_%datetime%.log

::move /Y *.cs %drive.letter%\Phoenix\Working\Solution\Web\App_Code\reports\ >> .\log\%logfolder%\phoenix_xsd_class_%datetime%.log

pushd "%~dp0"
cls
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

set appdir2=%drive.letter%\Phoenix\Working\Build\Web

set path=%path%;%appdir%\Utilities\nant-0.92-bin\nant-0.92\bin;

echo %logfolder%\web_compile_%datetime%.log > logfile.txt

::nant /f:%appdir2%\web_compile.build -l:.\log\%logfolder%\web_compile_%datetime%.log

popd

