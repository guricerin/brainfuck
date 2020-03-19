# brainfuck

## Requirements
.NET Core 3.1  
https://dotnet.microsoft.com/download  

## CLI

### Restoring after Clone
```shell
$ dotnet tool restore
```

### Build
```shell
$ dotnet fake build # Build all projects as Release
```

### Run
```shell
$ dotnet run -p src/Brainfuck [-c {Debug|Release}]
$ #or
$ dotnet run -p src/Brainfuck <path-to-bf-file>
```