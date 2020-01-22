# words-generator-sample
Word concatenations' combinations generator: some kind of a puzzle game, 
but here word chunks are used instead of the images to form the word of fixed length.


E.g.:
``` 
foobar  
fo  
obar
```
should result in the ouput: 
```
fo+obar=foobar

This flexible solution focuses on the following aspects:
1. Clean code/OOP/SOLID/etc.
2. Performance

Here I also added the following:
1. Sorting of words (by which word combinations are constructed) in output;
2. Caching: added loading length combinations from cache

If the cache performance is further optimized, the overall solution performance will be boosted.
For this demo (for the sake of development simplicity), I just used Newtonsoft.JSON serialization/deserialization for caching.

As the result, if the "get all word chunk lenghts from cache" operation execution time is close to 0 ms, 
the total execution time will be equal to 20-25 ms (please see log.txt for more details.)
How to calculate performance:
1. GetWordChunksGroupedByLength - here I group all word chunks by their length, and place them to Hashtable
2. FindAllWordChunksLengthCombinations - caching included
3. GetAllWordsForCombinationsSearch
4. GetWordCombinationsByLengthCombination

