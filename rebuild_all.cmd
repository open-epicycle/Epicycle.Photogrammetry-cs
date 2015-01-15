@echo off

cd projects
msbuild Epicycle.Photogrammetry.net35.sln /t:Clean,Build /p:Configuration=Debug
msbuild Epicycle.Photogrammetry.net35.sln /t:Clean,Build /p:Configuration=Release
msbuild Epicycle.Photogrammetry.net40.sln /t:Clean,Build /p:Configuration=Debug
msbuild Epicycle.Photogrammetry.net40.sln /t:Clean,Build /p:Configuration=Release
msbuild Epicycle.Photogrammetry.net45.sln /t:Clean,Build /p:Configuration=Debug
msbuild Epicycle.Photogrammetry.net45.sln /t:Clean,Build /p:Configuration=Release

pause
