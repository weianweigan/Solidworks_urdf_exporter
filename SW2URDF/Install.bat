set path=%~d0
cd %path%
cd /d %~dp0

RegAsm.exe SW2URDF.dll /codebase
