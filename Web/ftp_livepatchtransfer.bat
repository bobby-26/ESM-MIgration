@echo off

pushd "%~dp0"

set path=%path%;C:\Phoenix\Utilities\nant-0.85-bin\nant-0.85\bin;C:\Phoenix\Utilities\nantcontrib-0.85-bin\nantcontrib-0.85\bin

set var="%cd%"
ftp -s:%var%\ftpscript.txt

popd
