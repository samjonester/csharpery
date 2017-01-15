# C# on a Mac - Getting Over The Hump

This repo is intended to help you get started with C# on a Mac!

Blasphamy you say? So did I. I was initially frustrated, by the process. I felt like the push for dotnet on a Mac was just marketing without substance.

Eventually I got it working with the help of a sparse and hard to find post from Microsoft [here](https://docs.microsoft.com/en-us/dotnet/articles/core/testing/unit-testing-with-dotnet-test). Now that it's working, I plan to do a deep dive into the language and the testing tools available within the C# and .NET ecosystems. Wish me luck!

## The installation process

1. **Install .NET Core**
    * https://www.microsoft.com/net/core#macos
    * .NET Core is the runtime for your applications.
    * Also included, are the base libraries for .NET applications (i.e. `System`)

1. **Install Visual Studio Code**
    * https://code.visualstudio.com/download
    * VS Code is a lightweight editor. It is similar to Atom in that it is an editor built on [Electron](http://electron.atom.io/) that has a large extension ecosystem. It even has a VIM mode!
    * VS Code allows us to write C# libraries or .NET applications, and has a strong adoption rate in the JavaScript community as well.

1. **Install VS Code Extensions**
    1. Press `CMD + SHIFT + X` then search for and install the following plugins.
        * `C#`
        * `C# Extensions`
    1. Other extensions that I reccomend
        * `Terminal`: Provides an integrated terminal in VS Code and also the ability to run commands that have been highlighted in an editor.
        * `VIM`: If you love VIM then, here is your VIM emulation for VS Code.
        * `Trailing Semicolon`: Press `CMD + ;` to insert a semicolon at the end of the current line.
        * `Traling Whitespaces`: Because they suck.

## Your First Project

The following steps will create a "hello world" C# library with a test project. This will allow you to quickly get set up to start learning C# with a TDD workflow.

### Project Creation

Let's start by setting up a typical C# project structure. App/Library projects and Test projects are typically separated in C#.

``` sh
mkdir -p ~/code/experiments/csharpery
cd ~/code/experiments/csharpery
mkdir -p src/Hello
mkdir -p test/HelloTest
```

Now we will create a global file that tells `dotnet` where to find our source and test projects. This file is located at [`global.json`](./global.json)

``` json
{
    "projects": [
        "src",
        "test"
    ]
}
```

Also, here's a [.gitignore](/.gitignore) that comes with a new C# Library created with the [ASP.NET Yeoman Generator](https://github.com/omnisharp/generator-aspnet#readme). It's got A LOT of stuff in it. I don't understand it all, yet, so I won't be talking about it.


### Test Project

We will start with our test project. We will create the test project, ensure we can run our tests with a World Exists test, and set up the test runner in VS code.

``` sh
cd ~/code/experiments/csharpery/test/HelloTest
dotnet new -t xunit
```

This will create a new `project.json` file for our test project, and a World Exists test called `Tests.cs`. The default test framework is xUnit. I'll be switching to NUnit later, but this will enable us to make sure that everything works first.

Next we will restore dependencies for our Test project. If you've written Ruby or used NPM this command functions like `bundle install` or `npm install`.

``` sh
dotnet restore
```

Now we can run our test to ensure that our environment works!

```
dotnet test
```

#### Switching to NUnit

Let's update our test project to use NUnit instead of xUnit. I'm not going to compare the frameworks. That's been done many times, and a quick Google search will help you decide which is right for you.

We will update our Test project's [`project.json`](https://github.com/samjonester/csharpery/blob/91df236c628aefa1d120ffd5e7df8d8d876fe35f/test/HelloTest/project.json) to use NUnit instead of xUnit. Here's what mine looks like. Please make sure you're using the latest and greatest versions of both [NUnit](https://www.nuget.org/packages/NUnit/) and [NUnit .NET Core Runner](https://www.nuget.org/packages/dotnet-test-nunit/3.4.0-beta-3) and not these crusty, old, stale versions. You can find the latest on [NuGet](https://www.nuget.org/).

``` json
{
  "version": "1.0.0-*",
  "buildOptions": {
    "debugType": "portable"
  },
  "dependencies": {
    "System.Runtime.Serialization.Primitives": "4.3.0",
    "NUnit": "3.5.0",
    "dotnet-test-nunit": "3.4.0-beta-3"
  },
  "testRunner": "nunit",
  "frameworks": {
    "netcoreapp1.1": {
      "dependencies": {
        "Microsoft.NETCore.App": {
          "type": "platform",
          "version": "1.1.0"
        }
      },
      "imports": [
        "dotnet5.4",
        "portable-net451+win8"
      ]
    }
  }
}
```

Here is an updated World Exists test at [`test/HelloTest/Tests.cs`](./test/HelloTest/Tests.cs). Notice only the using Directive and the testing Attributes changed to use NUnit for this example.

``` csharp
using System;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1() 
        {
            Assert.True(true);
        }
    }
}
```

Once again we will restore our dependencies and run the test command.

``` sh
dotnet restore
dotnet test
```

#### Setup the VS Code Test Task

To run our tests in VS Code, we need to setup the test task. `CMD + SHIFT + P` will bring up a searchable list of editor commands. Then search for `Run Test Task`. Finally select to configure the task runner for .NET Core. Within the tasks section of the`tasks.json` file, add a test task. The args will qualify the location of our Test project.

``` json
{
    "taskName": "test",
    "args": [ "test/HelloTest" ],
    "isBuildCommand": false,
    "showOutput": "always"
}
```

### Library Creation

Let's create the project that will hold our C# library code. I've chosen a library because I intend to use this starter project to practice and learn C# with a [kata][katas].

We will start by initializing the project.

``` sh
cd ~/code/experiments/csharpery/src/Hello
dotnet new -t Lib
```

Now we have a [`project.json`](./src/Hello/project.json) for our library, and also a sample file `Library.cs` that we can just delete. Now let's restore our dependencies.

``` sh
dotnet restore
```

### Test Driving Hello World

The goal of this experiment is to TDD a [code kata][katas] in C#. First we need to make sure that we can test code in our library. We'll use the classic [Hello World](https://en.wikipedia.org/wiki/%22Hello,_World!%22_program) to do that!

#### Our Test

Let's start by writing a test located at [`test/HelloTest/HelloTests.cs`](test/HelloTest/HelloTests.cs).

``` csharp
using System;

namespace Hello.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class Tests
    {

        private Hello subject;

        [SetUp]
        public void Setup() {
            subject = new Hello();
        }

        [Test]
        public void itShouldSayHi()
        {
            Assert.AreEqual("Hello World", subject.SayHi());
        }
    }
}
```

We run our tests and we get an error that `'Hello' is a namespace but is used like a type`. This class is the subject of our test, but doesn't exist. Let's create the Hello class to get past this error.

#### Creating and referencing our library class.

We will do this with a two step process.

1. Create a new file [`src/Hello/Hello.cs`](https://github.com/samjonester/csharpery/blob/afe33c975de79637445734e1898a0d09dc9bb987/src/Hello/Hello.cs) and create a class in the `Hello` [namespace](https://msdn.microsoft.com/en-us/library/z2kcy19k.aspx).

``` csharp
namespace Hello
{
    public class Hello
    {
        public Hello() {}
    }
}
```

2. Add our library as a dependency to our test project in [`test/HelloTest/project.json`](./test/HelloTest/project.json#L10).

``` json
  "dependencies": {
    ...
    "Hello" : {
      "target": "project"
    }
  }
```

Our dependencies changed, so let's restore them with `dotnet restore`. And now when we run our tests we progress to a new error! The method we are testing doesn't exist yet.

#### Filling in the blanks

We've just got to implement the method `SayHi()` now to make our tests pass. Let's do it!

Here's the final implementation of [`src/Hello/Hello.cs`](./src/Hello/Hello.cs#L7).

``` csharp
namespace Hello
{
    public class Hello
    {
        public Hello() {}

        public string SayHi() {
            return "Hello World";
        }
    }
}
```
Now we run our tests and they pass! Whooo!!!!!!


[katas]: https://github.com/samjonester/awesome-katas/