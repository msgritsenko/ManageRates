
Manage rates
============

[![License MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Build status](https://ci.appveyor.com/api/projects/status/s9rlmu3a06duyshc/branch/master?svg=true)](https://ci.appveyor.com/project/msgritsenko/managerates/branch/master)
[![NuGet](https://img.shields.io/nuget/v/ManageRates.AspnetCore.svg)](https://www.nuget.org/packages/ManageRates.AspnetCore/) 
[![codecov](https://codecov.io/gh/msgritsenko/ManageRates/branch/master/graph/badge.svg)](https://codecov.io/gh/msgritsenko/ManageRates)


ManageRates is an open-source and cross-platform framework for managing throttling from asp.net core applications. In simple case it is an alternative project to [AspNetCoreRateLimit](https://github.com/stefanprodan/AspNetCoreRateLimit). 

[!include[BenchmarkResults](../src/Tests/BenchmarkTest/BenchmarkDotNet.Artifacts/results/Benchmark.Test.SingleVsFirst-report-github.md)]

## Installation

To install ManageRates packange, run the following command in the Nuget Package Manager Console:

```
PM> Install-Package ManageRates.AspnetCore
```

## Samples

[!code-csharp[Main](../../samples/WebApi/Startup.cs?range=22-30&highlight=8)]

## License

ManageRates.AspnetCore licensed under the [MIT License](https://raw.githubusercontent.com/msgritsenko/ManageRates/master/LICENSE).


