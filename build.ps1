# find vswhere - https://github.com/microsoft/vswhere/wiki/Find-MSBuild
$path = Join-Path -Path ${env:ProgramFiles(x86)} -ChildPath "Microsoft Visual Studio\Installer\vswhere.exe";

# find msbuild
$msbuild = & $path -latest -prerelease -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe | select-object -first 1

# restore packages
& dotnet restore .\App\SwitcherServer.csproj

# build appliction
& $msbuild .\App\SwitcherServer.csproj -t:Publish -p:Configuration=Release
