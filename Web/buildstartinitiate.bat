
@echo off
pushd "%~dp0"
SchTasks /Change /TN "PhoenixBuildInitiate" /DISABLE
popd
        
