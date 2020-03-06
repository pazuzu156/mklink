@echo off
title mklink Build Tool
color 0A

set CWD=%~dp0
set MSFW=%PROGRAMFILES(X86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin
set PATH=%PATH%;%MSFW%
set CFLAGS=/p:Configuration=Release;AllowUnsafeBlocks=true /p:CLSCompliant=False
set LDFLAGS=/tv:4.0 /p:TargetFrameworkVersion=v4.0 /p:Platform="Any Cpu" /p:OutputPath="%CWD%\build"

:stage
rmdir /S /Q build

:build
echo ------------------------------------------
echo Building package...
MSBuild mklink.sln %CFLAGS% %LDFLAGS%
if errorlevel 1 goto error

:done
echo.
echo ------------------------------------------
echo Complete!
goto exit

:error
echo Failed to compile...

:exit
echo.
pause
