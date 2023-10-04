# watchex

[![CI](https://github.com/francWhite/watchex/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/francWhite/watchex/actions/workflows/ci.yml)
[![GitHub release](https://img.shields.io/github/v/release/francWhite/watchex)](https://github.com/francWhite/watchex/releases)
[![licence](https://img.shields.io/github/license/francWhite/watchex)](https://github.com/francWhite/watchex/blob/main/LICENSE)

Evaluates all files in a given .csproj file, including its dependencies, which are copied to the output directory during the build process.
These files are then observed and if any change is detected, they are copied to the output directory. This avoids having to rebuild the project after changes in content files like translations or non standard configurations.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [License](#license)

## Installation

### Install manually

Download the latest release from the [releases page](https://github.com/francWhite/watchex/releases) and extract the archive to a folder of your choice.

## Usage

If no project file is specified, the tool searches for a .csproj file in the current directory.

```
USAGE:
    watchex [PROJECT]
EXAMPLES:
    watchex
    watchex MyProject.csproj
```

## License

Distributed under the [MIT license](https://github.com/francWhite/watchex/blob/main/LICENSE)