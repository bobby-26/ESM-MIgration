@echo off
pushd "%~dp0"
cls

call web_vss_get.bat  

call web_compile.bat

call web_buildcheck.bat

popd
exit
