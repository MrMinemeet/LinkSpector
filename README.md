# LinkSpector
LinkSpector is a C# project for checking the validity of links ion a given website. The console application automatically checks all links on a given website and reports the status of each link.

The project is inspired by [Lychee](https://lycheeorg.github.io/lychee/) that is written in Rust and created by awesome developers such as [Matthias Endler](https://github.com/mre).

## TODOs
* Ignore URIs in HTML comments (they are not functional in the website so no need to check them)
* Handle [HTTP 999](https://http.dev/999#linkedin) from LinkedIn as a "success" (it it is successful in some kind)
 
---

# Course Information
This repository contains the source code for the lecture [Software Development in C#](https://ssw.jku.at/Teaching/Lectures/CSharp/) held by Prof. Mössenböck at [JKU](https://jku.at). The course requires a small C# project implementation with at least 250 lines of code.
