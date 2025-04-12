# DOOMSDAY

## About

There are many videos on YouTube of lyrics being color coded according to their rhyme scheme. ([Here's a not so SFW example](https://www.youtube.com/watch?v=wmRdMyaqsTE)). I have not been able to find an open source implementation (though Wall Street Journal formed an algorithm to study the rhyme scheme of the play Hamilton, [found here](https://graphics.wsj.com/hamilton-methodology/)).

So the problem I am solving is creating my own open-source rhyme scheme colorer. From what I hear on online forums, those that color code lyrics on YouTube do it from scratch. So this problem hasn't been solved in a great, publicly available way (yet). These creators on YouTube would have their work cut down for them, as I want to programatically solve this, as opposed to mapping rhymes from scratch.

### Specifications

I plan to use C#, as that's my favorite language and LINQ is amazing with databases (which I plan to use). I think making this a console application would make it easier, so I'm going to use a console library (Spectre.Console) to help me with making the format pretty and colored. The database will probably be SQLite.
