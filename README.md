# TextMate Bundle Installer

[![Build status](https://ci.appveyor.com/api/projects/status/qpd0qtdvpsnmygy0?svg=true)](https://ci.appveyor.com/project/madskristensen/textmatebundleinstaller)

<!-- Update the VS Gallery link after you upload the VSIX-->
Download this extension from the [VS Gallery](https://visualstudiogallery.msdn.microsoft.com/[GuidFromGallery])
or get the [CI build](http://vsixgallery.com/extension/4773ce75-6f30-4269-9557-1f7c30a47be2/).

---------------------------------------

Adds basic language support for a wide variety of programming languages
such as Clojure, Go, Jade, Lua, Swift, Ruby and many more

See the [change log](CHANGELOG.md) for changes and road map.

## TextMate bundles
This extension uses Visual Studio's capability to load TextMate bundles.

It works by registering TextMate bundles so that Visual Studio will
automatically load them when a file is opened that matches one of the
bundles and no other language service is available for the specific
file type.

For instance, this extension contains a TextMate bundle for YAML files,
but if you decide to install the more powerful
[YAML Editor extension](https://visualstudiogallery.msdn.microsoft.com/34423c06-f756-4721-8394-bc3d23b91ca7)
then that extension will win take over.

## Supported languages and file types

- Batch
- Clojure
- CMake
- Dockerfile
- Go
- Groovy
- Ini
- Jade
- Java
- JavaDoc
- Lua
- Make
- Objective-C
- Perl
- PHP
- PowerShell
- Ruby
- Rust
- Shaderlab
- ShellScript (Bash)
- Swift
- YAML

## File Icons
For the best experience, it is recommended that you also install the
free 
[File Icons extension](https://visualstudiogallery.msdn.microsoft.com/5e1762e8-a88b-417c-8467-6a65d771cc4e).

## Contribute
Check out the [contribution guidelines](.github/CONTRIBUTING.md)
if you want to contribute to this project.

For cloning and building this project yourself, make sure
to install the
[Extensibility Tools 2015](https://visualstudiogallery.msdn.microsoft.com/ab39a092-1343-46e2-b0f1-6a3f91155aa6)
extension for Visual Studio which enables some features
used by this project.

## License
[Apache 2.0](LICENSE)