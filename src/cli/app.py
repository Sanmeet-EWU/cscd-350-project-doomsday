from textual.app import App, ComposeResult
from textual.screen import Screen
from textual import events
from textual.widgets import Input, Static, DirectoryTree
from textual.containers import Center
import subprocess, json
from textual.message import Message
from textual.events import Key
from pathlib import Path

class PathSelected(Message):
    """Custom message to carry the selected Path."""
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
    def compose(self):
        yield DirectoryTree('/')

    async def on_directory_tree_node_selected(
        self, event: DirectoryTree.NodeSelected
    ) -> None:
        """Triggered whenever a node is (single-)clicked or Enter is pressed."""
        selected_path: Path = event.node.data  # .data holds the Path
        await self.post_message(PathSelected(selected_path))

    async def on_path_selected(self, message: PathSelected) -> None:
        """Handle our custom PathSelected message."""
        path = message.path
        if path.is_dir():
            tree = self.query_one(DirectoryTree)
            await tree.root.expand_path(path, recursive=False)
        else:
            self.dismiss(path)


    async def on_key(self, event: Key) -> None:
        tree = self.query_one(DirectoryTree)
        if tree.has_focus:
            if event.key == "j":
                tree.action_cursor_down()
                event.stop()
            elif event.key == "k":
                tree.action_cursor_up()
                event.stop()

def json_to_markup(rhyme_scheme):
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
        def get_path(path: str | None):
            if path:
                # rs = get_rhyme_scheme(path)
                # markup = json_to_markup(rs)
                self.app.push_screen(MarkupScreen(path))


        self.push_screen('dir', get_path)
        self.push_screen('title')

if __name__ == '__main__':
    app = MyApp()
    app.run()
