@echo off
taskkill /F /IM chrome.exe
start "" "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" "file:///C:/Projects/Prime_3/Prime3_Original/Prime3_Sample_Webpage.html" --kiosk --fullscreen