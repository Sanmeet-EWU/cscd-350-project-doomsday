import pytest
from pathlib import Path
import importlib.util

from textual.widgets import DirectoryTree

app_path = Path(__file__).parent.parent.parent / "src" / "cli" / "app.py"
spec = importlib.util.spec_from_file_location("app", str(app_path))
app = importlib.util.module_from_spec(spec)
spec.loader.exec_module(app)

def test_json_to_markup_empty():
    assert app.json_to_markup([]) == "[red]No rhyme scheme data available[/]"


def test_json_to_markup_basic():
    scheme = [
        {"Syllables": [
            {"SyllableContent": "Foo", "Color": "red"},
            {"SyllableContent": "NEWLINECHAR", "Color": ""},
            {"SyllableContent": "Bar", "Color": "blue"},
        ]}
    ]
    result = app.json_to_markup(scheme)
    assert "[black on red]Foo[/]" in result
    assert "\n" in result
    assert "[black on blue]Bar[/]" in result
