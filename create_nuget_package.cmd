@echo off

rmdir NuGetPackage /s /q
mkdir NuGetPackage
mkdir NuGetPackage\Epicycle.Photogrammetry-cs.0.1.0.0
mkdir NuGetPackage\Epicycle.Photogrammetry-cs.0.1.0.0\lib

copy package.nuspec NuGetPackage\Epicycle.Photogrammetry-cs.0.1.0.0\Epicycle.Photogrammetry-cs.0.1.0.0.nuspec
copy README.md NuGetPackage\Epicycle.Photogrammetry-cs.0.1.0.0\README.md
copy LICENSE NuGetPackage\Epicycle.Photogrammetry-cs.0.1.0.0\LICENSE

xcopy bin\net35\Release\Epicycle.Photogrammetry_cs.dll NuGetPackage\Epicycle.Photogrammetry-cs.0.1.0.0\lib\net35\
xcopy bin\net35\Release\Epicycle.Photogrammetry_cs.pdb NuGetPackage\Epicycle.Photogrammetry-cs.0.1.0.0\lib\net35\
xcopy bin\net35\Release\Epicycle.Photogrammetry_cs.xml NuGetPackage\Epicycle.Photogrammetry-cs.0.1.0.0\lib\net35\
xcopy bin\net40\Release\Epicycle.Photogrammetry_cs.dll NuGetPackage\Epicycle.Photogrammetry-cs.0.1.0.0\lib\net40\
xcopy bin\net40\Release\Epicycle.Photogrammetry_cs.pdb NuGetPackage\Epicycle.Photogrammetry-cs.0.1.0.0\lib\net40\
xcopy bin\net40\Release\Epicycle.Photogrammetry_cs.xml NuGetPackage\Epicycle.Photogrammetry-cs.0.1.0.0\lib\net40\
xcopy bin\net45\Release\Epicycle.Photogrammetry_cs.dll NuGetPackage\Epicycle.Photogrammetry-cs.0.1.0.0\lib\net45\
xcopy bin\net45\Release\Epicycle.Photogrammetry_cs.pdb NuGetPackage\Epicycle.Photogrammetry-cs.0.1.0.0\lib\net45\
xcopy bin\net45\Release\Epicycle.Photogrammetry_cs.xml NuGetPackage\Epicycle.Photogrammetry-cs.0.1.0.0\lib\net45\

pause