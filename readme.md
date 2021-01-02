# ATEM Switcher

## Building

This project relies on a COM reference to the BMD interop DLL. Building the software using the .NET Framework version of MSBuild, which contains the [ResolveComReference build task](https://docs.microsoft.com/en-ca/visualstudio/msbuild/resolvecomreference-task).

> ...MSBuild.exe -p:Configuration=release -t:publish

## Inspirations

https://github.com/filiphanes/atem-live-controller
https://github.com/LibAtem/LibAtem

## Contributing

1. Fork it ( https://github.com/tbetomblchr/atem-hope-full )
2. Create your feature branch (`git checkout -b my-new-feature`)
3. Implement your feature
4. Commit your changes (`git commit -am 'Add some feature'`)
5. Push to the branch (`git push origin my-new-feature`)
6. Create new Pull Request