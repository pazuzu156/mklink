@echo off
set cf=/S /Q
echo.
echo Cleaning...
echo.
rmdir %cf% build
rmdir %cf% "mklink\obj"
rmdir %cf% "mklink\bin"
echo.
echo Completed
