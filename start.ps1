$ScriptsDir = Join-Path $PSScriptRoot "scripts"

echo $ScriptsDir

pushd $ScriptsDir

try {
    .\build-net.ps1
}
finally {
    popd
}

pushd .\src\cli

try {
    python app.py
}
finally {
    popd
}
