description of app:
- can input a file of lyrics
- can store and quickly query data on rhymes and word pronunciation
- can assign colors to phonemes in lyrics file based on mapper algorithm
- can color background of syllabized word syllables
- can read lyrics and split them up into words, syllables, pronunciation
- can run off of command line
- once added to path, can call from anywhere
- maybe installation process to automatically install/add to path?


user stories ideas:
1. as an app user, I want to provide the app lyrics to parse so I can see my rhyme scheme.
2. as an app user, I want to be able to input my lyrics as a text file, so I don't have to type them in manually.
3. as an app user, I want to also be able to provide my app lyrics either through a file, or typing them in myself, to allow for flexibility.
4. as an app user, I want to receive error messages when I've inputted the wrong format, so I can fix the error with ease.
5. as an app user, I want to be able to query the app for help with command line arguments and flags in order to use the full extent of the app.
6. as an app user, I want to be able to pass flags into the console app so I can specify what I want it to do.
7. as an app user, I want the app to separate the lyrics into words, syllables, and phonemes using CMUdict so that I can understand the pronunciation structure.
8. as an app user, I want the app to remember what words I've inputted before so it can deliver my rhyme scheme faster.
9. as an app user, I want to be able to see my rhyme scheme visualized with colors to benefit my writing process.
10. as an app user, I want to run this app off of the command line to allow for a faster workflow.
11. as an app user, I want to be able to go through an installation process that makes everything easy to use so I don't have to keep track of all the files.
12. as an app user, I want to be able to look up potential rhymes so I can be assisted in my writing process.
13. as an app user, I don't want to worry about having to remove nonalphanumeric characters, in order to make the process easier.

requirements and specifications:
1. There will be a feature to intake lyrics through the command line.
2. There will be a feature to intake lyrics through the command line as a file. This feature will use .NET library for reading files.
3. There will also be a feature to intake lyrics as the user types them out. This feature will use .NET library for building the string up.
4. There will be a feature to catch errors such as wrong file format or invalid text structure.
5. There will be a feature to ask the app for help, possibly through a '--help' flag.
6. There will be a feature to parse the command line arguments for flags and config.
7. There will be a feature to split the lyrics into lines, words, syllables, and ultimately convert the words and syllables into phonemes for better analyzation. This feature will rely on the public dictionary made by CMU, cmudict.
8. There will be a feature to store words from cmudict and past words inputted and rhymes found in an in-memory sqlite database. This will rely on SQLite databases and the .NET library Entity Framework Core as an ORM.
9. There will be a feature to color the lyrics in the console text. This feature will utilize the open source framework, Spectre.Console, for console text formatting.
10. There will be a feature to allow for command line execution. This feature will be fulfilled through Visual Studio's console app project.
11. There will be a feature to run through an installation process, to keep the storage of the app and it's utilities clean. I'm not sure how this feature will be fulfilled.
12. There will be a feature to query the application on words that rhyme with an inputted word.
13. There will be a feature to parse the lyrics and ignore values that aren't essential to the analyzation of the lyrical content. The app will also put the nonessential characters back to maintain integrity of input.

potential glossary vocab:
- phoneme:
- syllable:
- syllabized word:
- rhyme scheme:
- flags:
- CMUdict:
- .NET:
- mapper algorithm:
- nonalphanumeric:
- sqlite:
- Spectre.Console:
- Visual Studio:
- ?
