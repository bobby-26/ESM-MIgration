
@echo off
pushd "%~dp0"
SchTasks /RUN /TN "PhoenixBuild"
SchTasks /Change /TN "PhoenixBuildInitiate" /DISABLE
popd
        
