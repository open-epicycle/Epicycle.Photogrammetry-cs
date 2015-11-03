@echo off

rmdir NuGetPackage /s /q
mkdir NuGetPackage
mkdir NuGetPackage\Epicycle.Photogrammetry-cs.0.1.2.0
mkdir NuGetPackage\Epicycle.Photogrammetry-cs.0.1.2.0\lib

copy package.nuspec NuGetPackage\Epicycle.Photogrammetry-cs.0.1.2.0\Epicycle.Photogrammetry-cs.0.1.2.0.nuspec
copy README.md NuGetPackage\Epicycle.Photogrammetry-cs.0.1.2.0\README.md
copy LICENSE NuGetPackage\Epicycle.Photogrammetry-cs.0.1.2.0\LICENSE

xcopy bin\net35\Release\Epicycle.Photogrammetry_cs.dll NuGetPackage\Epicycle.Photogrammetry-cs.0.1.2.0\lib\net35\
xcopy bin\net35\Release\Epicycle.Photogrammetry_cs.pdb NuGetPackage\Epicycle.Photogrammetry-cs.0.1.2.0\lib\net35\
xcopy bin\net35\Release\Epicycle.Photogrammetry_cs.xml NuGetPackage\Epicycle.Photogrammetry-cs.0.1.2.0\lib\net35\
xcopy bin\net40\Release\Epicycle.Photogrammetry_cs.dll NuGetPackage\Epicycle.Photogrammetry-cs.0.1.2.0\lib\net40\
xcopy bin\net40\Release\Epicycle.Photogrammetry_cs.pdb NuGetPackage\Epicycle.Photogrammetry-cs.0.1.2.0\lib\net40\
xcopy bin\net40\Release\Epicycle.Photogrammetry_cs.xml NuGetPackage\Epicycle.Photogrammetry-cs.0.1.2.0\lib\net40\
xcopy bin\net45\Release\Epicycle.Photogrammetry_cs.dll NuGetPackage\Epicycle.Photogrammetry-cs.0.1.2.0\lib\net45\
xcopy bin\net45\Release\Epicycle.Photogrammetry_cs.pdb NuGetPackage\Epicycle.Photogrammetry-cs.0.1.2.0\lib\net45\
xcopy bin\net45\Release\Epicycle.Photogrammetry_cs.xml NuGetPackage\Epicycle.Photogrammetry-cs.0.1.2.0\lib\net45\

cd NuGetPackage
nuget pack Epicycle.Photogrammetry-cs.0.1.2.0\Epicycle.Photogrammetry-cs.0.1.2.0.nuspec -Properties version=0.1.2.0
7z a -tzip Epicycle.Photogrammetry-cs.0.1.2.0.zip Epicycle.Photogrammetry-cs.0.1.2.0 Epicycle.Photogrammetry-cs.0.1.2.0.nupkg

echo nuget push Epicycle.Photogrammetry-cs.0.1.2.0.nupkg > push.cmd
echo pause >> push.cmd

pause