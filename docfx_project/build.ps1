# update efficiency.png to current state https://swharden.com/scottplot/cookbook 
dotnet build -c Release -o "./obj/artifacts/" "../src/Tests/EfficencyTest/" 
$fullPath = Join-Path -Path $PSScriptRoot -ChildPath "/articles/images/efficiency.png"
./obj/artifacts/Efficiency.Test.exe 2 10 7 $fullPath

# run benchmarks with .md output format
Set-Location "$PSScriptRoot/../src/Tests/BenchmarkTest/"
dotnet run -c Release

# return back and build documentation
Set-Location $PSScriptRoot
docfx