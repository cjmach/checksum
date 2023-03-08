# checksum

A .NET Core command-line application to print and check hash checksums.

# Requirements

- .NET Core 3.1 runtime.

# Usage

```console
Usage: checksum [<FILEs>...] [options]

Arguments:
  <FILEs>  A list of files to process. If the list is empty (or first FILE is -), data is read from standard input.

Options:
  -a, --algorithm <algorithm>  The hash algorithm implementation to use. Supported values: sha, sha1, md5, sha256, sha384, sha512. [default: sha256]
  -c, --check                  Read hash sums from the FILEs and check them.
  --version                    Show version information
  -?, -h, --help               Show help and usage information
```
