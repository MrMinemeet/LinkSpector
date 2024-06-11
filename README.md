# LinkSpector
LinkSpector is a C# project for checking the validity of links ion a given website. The console application automatically checks all links on a given website and reports the status of each link.

The project is inspired by [Lychee](https://lycheeorg.github.io/lychee/) that is written in Rust and created by awesome developers such as [Matthias Endler](https://github.com/mre).

## Requirements
* .NET 8.0
* Internet connection

## Usage
The application is a console application and can be run from the command line. The following command line arguments are supported:
* `-r` - Toggles recursive mode. If enabled, the application will also check links on linked pages (only applies to the same domain). Default is false.
* `-i` - Allow insecure/no TLS certificates. Default is false.
* `-t <timeout>` - Set the timeout for each request in milliseconds. Default is 20 seconds.
* `-l <file>` - Log output to a file. Default is console output.

Example:
```
LinkSpector.exe -r -i -t 5000 http://example.com
```

## TODOs
Some stuff is written into source-code with `// TODO` so check that as well. Other more general things are written here:
* Seems a bit empty here, maybe you find some other stuff that has to be done
 
---

# Course Information
This repository contains the source code for the lecture [Software Development in C#](https://ssw.jku.at/Teaching/Lectures/CSharp/) held by Prof. Mössenböck at [JKU](https://jku.at). The course requires a small C# project implementation with at least 250 lines of code.
