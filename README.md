# watchex

[![CI](https://github.com/francWhite/watchex/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/francWhite/watchex/actions/workflows/ci.yml)
[![GitHub release](https://img.shields.io/github/v/release/francWhite/watchex)](https://github.com/francWhite/watchex/releases)
[![licence](https://img.shields.io/github/license/francWhite/watchex)](https://github.com/francWhite/watchex/blob/main/LICENSE)

Evaluates all files in a given .csproj file, including its dependencies, which are copied to the output directory during the build process.
These files are then observed and if any changes are detected, they are copied to the output directory. This avoids having to rebuild the project after changes in content files like translations or non standard configurations.

*Why not just use __dotnet watch__?* Even though `dotnet watch` is a great tool, it has its limitations. If you have some non-compiled files in a dependency which are included with a wildcard like `translations\**\*.json`, dotnet watch will not detect changes in these files because their paths are evaluated incorrectly; it will look for them in the startup project directory instead of the dependency in question.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [License](#license)

## Installation

### Install manually

Download the latest release from the [releases page](https://github.com/francWhite/watchex/releases), extract the archive to a folder of your choice and add said folder to your PATH environment variable.

## Usage

```
USAGE:
    watchex [OPTIONS]

OPTIONS:
    -h, --help                 Prints help information
    -v, --verbose              Prints verbose log output
    -V, --version              Prints version information
    -p, --project <PROJECT>    Path to the project file. Defaults to the first *.csproj file found in the current directory
```

## License

Distributed under the [MIT license](https://github.com/francWhite/watchex/blob/main/LICENSE)