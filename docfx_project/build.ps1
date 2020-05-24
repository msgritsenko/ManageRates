# run benchmark
# run build docfx documentation

Set-Location "$PSScriptRoot/../src/Tests/BenchmarkTest/"
dotnet run -c Release
Set-Location $PSScriptRoot
docfx