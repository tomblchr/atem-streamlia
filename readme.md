# ATEM Switcher

Making the ATEM Switcher accessible and conventional.

This application lives along side the ATEM Software Control software and provides access to the most needed controls through a browser.

## Features

* Input Switching
* Tally Light
* Run Macros
* Run betwen Key Frames
* Upstream and Downstream Keys

## Building

This project relies on a COM reference to the BMD interop DLL. Building the software using the .NET Framework version of MSBuild, which contains the [ResolveComReference build task](https://docs.microsoft.com/en-ca/visualstudio/msbuild/resolvecomreference-task).

Use the `build.ps1` script to build from the command line.

## Getting Going

1. Run the `SwitcherServer.exe` console application
1. Open https://localhost:5001 in a browser
1. Connect the application to the ATEM
    1. The application will first attempt to connect to the ATEM via USB.
    1. To connect the ATEM over a network use the Setup option to enter the IP address of the ATEM.

## Inspirations

Looking for an alternative to operate your ATEM? Try these projects...

* https://github.com/filiphanes/atem-live-controller
    * A similar concept implemented using Node.js. Uses the [applest-atem](https://github.com/applest/node-applest-atem) library to communicate with the ATEM.
* https://github.com/LibAtem/LibAtem
    * A .NET Core library not shackled to the ATEM SDK.
* https://github.com/bitfocus/companion
    * Highly customizable for many types of hardware.

## Contributing

1. Fork it ( https://github.com/tbetomblchr/atem-hope-full )
2. Create your feature branch (`git checkout -b my-new-feature`)
3. Implement your feature
4. Commit your changes (`git commit -am 'Add some feature'`)
5. Push to the branch (`git push origin my-new-feature`)
6. Create new Pull Request
