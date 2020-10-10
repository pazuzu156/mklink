@echo off
title MKLink Build Script

set PWD=%~dp0

if "%1" NEQ "" (
  if "%1" EQU "-c" goto clean
  if "%1" EQU "-d" goto restore
  if "%1" EQU "-h" goto help
  if "%1" EQU "-r" goto run
)

:build
rem build for net46, then for netcoreapp3.1
rem start x86 then x64 after

rem x86 net46
call dotnet publish -f net46 -c Release -v d -r win-x86
if not exist "%PWD%\Release\net46\win-x86" mkdir "%PWD%\Release\net46\win-x86"
copy "%PWD%\bin\Release\net46\win-x86\publish\mklink.exe" "%PWD%\Release\net46\win-x86\mklink.exe"

rem x64 net46
call dotnet publish -f net46 -c Release -v d -r win-x64
if not exist "%PWD%\Release\net46\win-x64" mkdir "%PWD%\Release\net46\win-x64"
copy "%PWD%\bin\Release\net46\win-x64\publish\mklink.exe" "%PWD%\Release\net46\win-x64\mklink.exe"

rem x86 netcoreapp3.1
call dotnet publish -f netcoreapp3.1 -c Release -v d -r win-x86 /p:PublishSingleFile=True /p:PublishTrimmed=True
if not exist "%PWD%\Release\netcoreapp3.1\win-x86" mkdir "%PWD%\Release\netcoreapp3.1\win-x86"
copy "%PWD%\bin\Release\netcoreapp3.1\win-x86\publish\mklink.exe" "%PWD%\Release\netcoreapp3.1\win-x86\mklink.exe"

rem x64 netcoreapp3.1
call dotnet publish -f netcoreapp3.1 -c Release -v d -r win-x64 /p:PublishSingleFile=True /p:PublishTrimmed=True
if not exist "%PWD%\Release\netcoreapp3.1\win-x64" mkdir "%PWD%\Release\netcoreapp3.1\win-x64"
copy "%PWD%\bin\Release\netcoreapp3.1\win-x64\publish\mklink.exe" "%PWD%\Release\netcoreapp3.1\win-x64\mklink.exe"
echo build complete
goto end

:restore
dotnet restore
goto end

:run
dotnet run -f netcoreapp3.1 -v d

:clean
if exist bin rmdir /s /q bin
if exist obj rmdir /s /q obj
if exist Release rmdir /s /q Release

:help
echo MKLink Build Script 1.0
echo Usage: build.bat [option]
echo Supplying no option will cause the build tool to build the app
echo.
echo Options
echo    -c  - Runs the binary cleanup
echo    -d  - Runs the dotnet restore command
echo    -h  - Shows this help text
echo    -r  - Runs a debug build of the app
goto end

:end
