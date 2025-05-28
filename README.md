# DOOMSDAY

## About

There are many videos on YouTube of lyrics being color coded according to their rhyme scheme. ([Here's a not so SFW example](https://www.youtube.com/watch?v=wmRdMyaqsTE)). I have not been able to find an open source implementation (though Wall Street Journal formed an algorithm to study the rhyme scheme of the play Hamilton, [found here](https://graphics.wsj.com/hamilton-methodology/)).

So the problem I am solving is creating my own open-source rhyme scheme colorer. From what I hear on online forums, those that color code lyrics on YouTube do it from scratch. So this problem hasn't been solved in a great, publicly available way (yet). These creators on YouTube would have their work cut down for them, as I want to programatically solve this, as opposed to mapping rhymes from scratch.

### Specifications

I plan to use C#, as that's my favorite language and LINQ is amazing with databases (which I plan to use). I think making this a console application would make it easier, so I'm going to use a console library (Spectre.Console) to help me with making the format pretty and colored. The database will probably be SQLite.


### Testing

Run 'dotnet test' in the src/Rhyme solution to run C# tests and 'pytest' (with pytest pip installed) to run tests/ui/app\_test.py tests


### Developer guidelines:

How to obtain source code: copy url and git clone on local machine
Directory structure:
- coverage : contains info about code coverage
- doc : contains documentation
- scripts : contains scripts
- src : contains source code
    - cli : contains frontend code
    - Rhyme : contains backend .NET code
- tests : contains some tests for frontend code
- start.ps1 : startup script in powershell

how to build software (run start.ps1)
how to test: see testing section above
how to add new tests: add new dotnet tests in src/Rhyme/Rhyme.Tests
- add new pytest tests in the tests/ folder

no guide to building release version, yet.

#### Coding style
Generally there is no specific coding style decided. Use the style of the surrounding code.
Use spaces as tabs. Tabs are usually inconvenient when used in multi-developer environment because often each person uses different size of tabs. Formatting of code will usually look wrong when tabs and spaces are mixed. See this picture on how to setup Lazarus IDE. If you use different editor than IDE set it up accordingly.
#### Writing comments
Comments should be written in English. It doesn't have to be perfect English only just so that other people can understand. If you don't know a word of English maybe it is possible to write comments in a different language but they might be translated into English anyway by other developers.
Comments describing functions, classes in the interface section are usually written with {en ...} marking. The en stands for English language. It is used by FPDoc to generate some documentation. In the implementation section use normal comments.
Write comments for code that might not be immediately understood by others. If you fix a not obvious bug, use code for specific circumstances, make assumptions that are not easily deductable write a few words explaining why, not neccessarily what the code does. This might be a helpful hint to whoever will be completing the code on how to do it.

#### Git commits:
Use git rebase to have linear history
Use prefixes in commit messages
When adding a new function
`ADD: < what has been added >`
When correcting an error
`FIX: Bug < [bug number from bugtracker] > [description]`
When removing code, files
`DEL: < what has been removed >`
When updating algorithm, function etc.
`UPD: < what has been updated >`
It is possible to commit even if added function (module) doesn't yet fully work, however the code must compile successfully and shouldn't prevent other functions (modules) from working correctly.
