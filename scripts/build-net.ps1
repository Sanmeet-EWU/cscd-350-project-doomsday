$ScriptsDir = Join-Path $PSScriptRoot "..\src\Rhyme\Rhyme"

pushd $ScriptsDir

$ArtifactsPath = Join-Path $PSScriptRoot "..\artifacts\net-app"

try {
    dotnet clean Rhyme.csproj
    dotnet publish Rhyme.csproj `
        -c Release `
        -r win-x64 `
        --self-contained true `
        /p:PublishSingleFile=true `
        /p:UseAppHost=true `
        -o $ArtifactsPath
}
finally {
    popd
}
