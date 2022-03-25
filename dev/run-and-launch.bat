cd ..\src\LogAnalysis
dotnet build
cd bin\Debug\net6.0
LogAnalysis.exe
start chrome %cd%\error.html
start chrome %cd%\success.html
start chrome %cd%\referrers.html
pause