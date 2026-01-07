from textual.app import App, ComposeResult
from textual.screen import Screen
from textual.widgets import Static, DirectoryTree
from textual.containers import Center
import subprocess, json
from textual.message import Message
from pathlib import Path

class PathSelected(Message):
    def __init__(self, path: Path) -> None:
        self.path = path
        super().__init__()


class TitleScreen(Screen):
    BINDINGS = [("enter", "app.pop_screen", "Next")]

    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.rhyme_scheme = get_rhyme_scheme()

    def compose(self) -> ComposeResult:
       title = """
[black] ____   ___   ___  __  __ ____  ____    _ __   __
|  _ \\ / _ \\ / _ \\|  \\/  / ___||  _ \\  / \\\\ \\ / /
| | | | | | | | | | |\\/| \\___ \\| | | |/ _ \\\\ V /
| |_| | |_| | |_| | |  | |___) | |_| / ___ \\| |
|____/ \\___/ \\___/|_|  |_|____/|____/_/   \\_\\_|
[/black]

"""
       with Center():
           yield Static(title)
           yield Static("", markup=True, id="static")
           yield Static("\n\n[black]Press [green]Enter[/] to continue...[/]")

    def on_mount(self):
        static = self.query_one("#static", Static)
        result = json_to_markup(self.rhyme_scheme)
        static.update(result)



def get_rhyme_scheme(lyricsPath: str = None):
    cmd = ["..\\..\\artifacts\\net-app\\Rhyme.exe"]
    if lyricsPath:
        cmd.append(lyricsPath)
    try:
        proc = subprocess.run(cmd, capture_output=True, text=True, check=True)
        return json.loads(proc.stdout)
    except subprocess.CalledProcessError as e:
      print("Exit code  :", hex(e.returncode))
      print("Stdout      :", e.stdout)
      print("Stderr      :", e.stderr)
      raise

class DirectoryScreen(Screen):
    BINDINGS = [
        ("j", "cursor_down", "Down"),
        ("k", "cursor_up", "Up"),
        ("enter", "select_path", "Select"),
    ]

    def compose(self):
        yield DirectoryTree('/')

    def action_cursor_down(self) -> None:
        tree = self.query_one(DirectoryTree)
        tree.action_cursor_down()

    def action_cursor_up(self) -> None:
        tree = self.query_one(DirectoryTree)
        tree.action_cursor_up()

    def on_directory_tree_directory_selected(
        self, event: DirectoryTree.DirectorySelected
    ) -> None:
        tree = self.query_one(DirectoryTree)
        static = self.query_one(Static)
        if tree.cursor_node is not None:
            path = event.path
            static.update(str(path))

    def on_directory_tree_file_selected(
        self, event: DirectoryTree.FileSelected
    ) -> None:
        tree = self.query_one(DirectoryTree)
        static = self.query_one(Static)
        if tree.cursor_node is not None:
            path = event.path
            static.update(str(path))
            self.dismiss(path)

def json_to_markup(rhyme_scheme):
    if not rhyme_scheme:
        return "[red]No rhyme scheme data available[/]"

    all_syllables = []
    is_newline = False
    for entry in rhyme_scheme:
        for syl in entry.get("Syllables", []):
            if syl['SyllableContent'] == 'NEWLINECHAR':
                is_newline = True
                all_syllables.append('\n')
            else:
                all_syllables.append(
                    f"[black on {syl['Color']}]{syl['SyllableContent']}[/]"
            )
        if not is_newline:
            all_syllables.append(" ")
        else:
            is_newline = False
    return "".join(all_syllables)

class MarkupScreen(Screen):
    def __init__(self, markup: str) -> None:
        super().__init__()
        self.markup = markup

    def compose(self):
        with Center():
            yield Static(self.markup)

class MyApp(App):
    CSS_PATH = 'styles.tcss'
    SCREENS = {'title': TitleScreen, 'dir': DirectoryScreen}

    def on_mount(self):
        self.push_screen('dir', self.handle_file_selection)
        self.push_screen('title')

    def handle_file_selection(self, path: Path | None) -> None:
        if path and path.is_file():
            print(f"Processing file: {path}")
            try:
                rs = get_rhyme_scheme(str(path))
                markup = json_to_markup(rs)
                self.push_screen(MarkupScreen(markup))
            except Exception as e:
                error_message = f"[red]Error processing file:[/]\n{type(e).__name__}: {str(e)}"
                self.push_screen(MarkupScreen(error_message))
        else:
            print(f"Invalid path received: {path}")

if __name__ == '__main__':
    app = MyApp()
    app.run()
