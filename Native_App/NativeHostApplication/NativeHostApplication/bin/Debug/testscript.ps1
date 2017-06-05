param($Step="A")
# -------------------------------------
# Imports
# -------------------------------------
$script = $myInvocation.MyCommand.Definition
$scriptPath = Split-Path -parent $script
. (Join-Path $scriptpath functions.ps1)

<# If (-NOT ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator"))

{   
$arguments = "& '" + $myinvocation.mycommand.definition + "'"
Start-Process powershell -Verb runAs -ArgumentList $arguments
Break
} #>

Clear-Any-Restart

if (Should-Run-Step "A") 
{
	Write-Host "A"
	Wait-For-Keypress "The test script will continue after a reboot, press any key to reboot..." 
	Restart-And-Resume $script "B"
}

if (Should-Run-Step "B") 
{
	#Write-Host "B"
	cmd.exe "/c C:\Users\Mrignayni\Desktop\reboot-and-resume-src\reboot-and-resume\startChrome.bat"
}

if (Should-Run-Step "C") 
{
	Write-Host "C"
}

Wait-For-Keypress "Test script Complete, press any key to exit script..."