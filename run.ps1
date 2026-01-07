pushd .\src\cli

try {
    python app.py
}
finally {
    popd
}