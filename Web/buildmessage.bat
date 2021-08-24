SchTasks /Delete /TN "PhoenixBuild" /F 
    SCHTASKS /Create /SC DAILY /TN "PhoenixBuild" /TR E:\PhoenixDeployment\PhoenixBuild\PhoenixBuild.bat /ST 20:00
    
