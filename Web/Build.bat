@echo off
pushd "%~dp0"
Call phnx_env.bat

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

set appdir=%CD%

set path=%path%;%appdir%\Utilities\nant-0.85-bin\nant-0.85\bin;
nant /f:Build.build -l:.\log\%logfolder%\Build_%datetime%.log -D:project.release=Phoenix

popd
pause
exit