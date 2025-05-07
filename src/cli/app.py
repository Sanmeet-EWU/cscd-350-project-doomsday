from textual.app import App, ComposeResult
from textual import events
from textual.widgets import Input, Static
from textual.containers import Container
import subprocess, json

def get_rhyme_scheme():
    # Assumes Backend.exe is next to your script or bundled via PyInstaller
    cmd = ["..\\..\\artifacts\\net-app\\Rhyme.exe"]
    try:
        proc = subprocess.run(cmd, capture_output=True, text=True, check=True)
        return json.loads(proc.stdout)
    except subprocess.CalledProcessError as e:
      print("Exit code  :", hex(e.returncode))
      print("Stdout      :", e.stdout)
      print("Stderr      :", e.stderr)
      raise
        


class MyApp(App):
    CSS_PATH = 'styles.tcss'

    def __init__(self, *, rhyme_scheme, **kwargs):
        super().__init__(**kwargs)
        self.rhyme_scheme = rhyme_scheme

    def compose(self) -> ComposeResult:
        with Container():
            yield Input(placeholder="enter filename", id="my-input")
            yield Static("", markup=True, id="static")

    def on_mount(self):
        static = self.query_one("#static", Static)
        all_syllables = []
        for entry in rhyme_scheme:
            for syl in entry.get("Syllables", []):
                all_syllables.append(
                    f"[{syl['Color']}]{syl['SyllableContent']}[/{syl['Color']}]"
                )
        static.update(" ".join(all_syllables))

if __name__ == '__main__':
    rhyme_scheme = get_rhyme_scheme()
    app = MyApp(rhyme_scheme=rhyme_scheme)
    app.run()
