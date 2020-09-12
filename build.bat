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
call dotnet publish -f net46 -c Release -v d
if not exist Release\net46 mkdir Release\net46
copy bin\Release\net46\win-x86\publish\mklink.exe Release\net46\mklink.exe
call dotnet publish -f netcoreapp3.1 -c Release /p:PublishSingleFile=True /p:PublishTrimmed=True -v d
if not exist Release\netcoreapp3.1 mkdir Release\netcoreapp3.1
copy bin\Release\netcoreapp3.1\win-x86\publish\mklink.exe Release\netcoreapp3.1\mklink.exe
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
