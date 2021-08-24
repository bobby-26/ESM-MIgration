$Server = "SNSSCHNSVR04"
$Scriptblock = {C:\Windows\System32\inetsrv\appcmd start apppool /apppool.name:"PhoenixTelerik"}

$ComputerName = "elog.executiveship.com"

$UserName = "elog\developer13"

$passwordFile = "E:\PhoenixDB\Phoenix\MyPassword.txt"

$Password = Get-Content $passwordFile | ConvertTo-SecureString -AsPlainText -Force

$Credential = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $UserName , $Password

Invoke-Command -ComputerName $Server -Scriptblock $Scriptblock -Credential $Credential