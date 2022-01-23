# VideoLibraryExample

This small console application uses some of the practices that you often see in real world projects. These practices try to make codes maintainable, extensible, testable, etc.

Something to think about when you code. _"Always code as if the guy (could be yourself) who ends up maintaining your code will be a violent psychopath who knows where you live." ― John Woods_

> Note: There're many articles online give detailed explainations on these practices. This repository is to show some of the practices used in action. It's not for detailed explainations.

## Dependency Injection

As you browse through the project, you will see the dependencies (e.g. `IDataSource`, `IFileWrapper`, etc) are injected via the constructor instead of using `new` to instantiate a class inside another class (except for `Program.cs` where you register the dependencies).

## Coding to Interface

Apart from the entry point of your application (e.g, `Program.cs`, `App.cs`, API Controller, etc), all classes with business logic implemented an interface. This fits well together with Dependency Injection.

E.g. We need to write to a file so we injected `IFileWrapper`. However, we don't care how it does it, all that matter is that it writes to a file. If `IFileWrapper` changed its implementations, we don't need to change the class that uses this dependency (unless it also changed the interface).

## Make your code testable

Sometimes you need to use a 3rd party library and it makes your codes untestable because it writes to a database, make http request, etc. You can use Dependency Injection and Coding to Interface to decouple your codes and make them testable. `IFileWrapper` is an example where the static class `File` has been decoupled to allow writing unit tests without concerning the `File`'s implementations.

Something to try yourself: _Currently the `App.cs` is not testable. Review the codes and try to make it testable._

# License

MIT License

Copyright (c) 2022 Chanh Lu

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
