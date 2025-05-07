using Rhyme.FileIO;
using System.IO;

namespace Rhyme.Tests;

[TestClass]
public class FileUtilsTests
{
    #region GetLyrics tests
    [TestMethod]
    public void GetLyrics_ValidPath_ReturnsNonNullNonEmptyString()
    {
        // Arrange

        // Act
        string result = Path.Combine("..", "Rhyme.Tests", "test-file.txt");

        // Assert
        Assert.IsFalse(string.IsNullOrEmpty(result));
    }

    [TestMethod]
    public void GetLyrics_NullPath_ThrowsArgumentNullException()
    {
        // Arrange

        // Act

        // Assert
        Assert.ThrowsException<ArgumentNullException>(() => FileUtils.GetLyrics(null!));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("  ")]
    public void GetLyrics_EmptyOrWhiteSpacePath_ThrowsArgumentException(string path)
    {
        // Arrange

        // Act

        // Assert
        Assert.ThrowsException<ArgumentException>(() => FileUtils.GetLyrics(path));
    }


    [TestMethod]
    public void GetLyrics_InvalidPath_ThrowsFileNotFoundException()
    {
        // Arrange

        // Act
        string path = Path.Combine("..", "Rhyme.Tests", "DOES-NOT-EXIST.txt");

        // Assert
        Assert.ThrowsException<FileNotFoundException>(() => FileUtils.GetLyrics(path));
    }
    #endregion GetLyrics tests

    #region GetWordsList tests

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public void GetWordsList_EmptyOrWhiteSpaceArgument_ThrowsArgumentException(string lyrics)
    {
        // Arrange

        // Act

        // Assert
        Assert.ThrowsException<ArgumentException>(() => FileUtils.GetWordsList(lyrics));
    }

    [TestMethod]
    public void GetWordsList_NullArgument_ThrowsArgumentException()
    {
        // Arrange

        // Act

        // Assert
        Assert.ThrowsException<ArgumentNullException>(() => FileUtils.GetWordsList(null!));
    }

    [TestMethod]
    [DataRow("hello\nim mark", new string[] { "hello", "im", "mark" })]
    [DataRow("hello\nim jimbob the third  ", new string[] { "hello", "im", "jimbob", "the", "third" })]
    [DataRow("hello\nim mark the fourth   \n", new string[] { "hello", "im", "mark", "the", "fourth" })]
    [DataRow("\nhello\nim mark the fourth   \n", new string[] { "hello", "im", "mark", "the", "fourth" })]

    public void GetWordsList_DifferingAmountOfLinesAndWords_ReturnsCorrectWordsAndCount(string lyrics, string[] result)
    {
        // Arrange

        // Act
        var wordsList = FileUtils.GetWordsList(lyrics).ToList();

        // Assert
        CollectionAssert.AreEquivalent(result, wordsList);
    }

    [TestMethod]
    [DataRow("hiya!", new string[] { "hiya" })]
    [DataRow("jimbob, the 3rd?", new string[] { "jimbob", "the", "3rd" })]
    [DataRow("skibididoowop.", new string[] { "skibididoowop" })]
    [DataRow("abcd/=efg", new string[] { "abcdefg" })]
    public void GetWordsList_WithNonAlphanumericCharacters_RemovesThem(string lyrics, string[] result)
    {
        // Arrange

        // Act
        var wordsList = FileUtils.GetWordsList(lyrics).ToList();

        // Assert
        CollectionAssert.AreEquivalent(result, wordsList);
    }

    #endregion GetWordsList tests

    #region GetCmuDict tests
    [TestMethod]
    public void GetCmuDict_Success()
    {
        // Arrange
        string path = Path.Combine("..", "..", "..", "..", "Rhyme", "cmudict-2.txt");

        // Act
        IEnumerable<string> dict = FileUtils.GetCmuDict(path);

        // Assert
        Assert.AreEqual(129482, dict.Count());
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("  ")]
    public void GetCmuDict_EmptyOrWhiteSpacePath_ThrowsArgumentException(string path)
    {
        // Arrange

        // Act

        // Assert
        Assert.ThrowsException<ArgumentException>(() => FileUtils.GetCmuDict(path));
    }


    [TestMethod]
    public void GetCmuDict_InvalidPath_ThrowsFileNotFoundException()
    {
        // Arrange
        string path = Path.Combine("..", "Rhyme.Tests", "DOES-NOT-EXIST.txt");

        // Act

        // Assert
        Assert.ThrowsException<FileNotFoundException>(() => FileUtils.GetCmuDict(path));
    }
    #endregion GetCmuDict tests

    #region GetNumberOfWordsByLineList tests

    [TestMethod]
    [DataRow("hello my name is joshua\n and   my name jimmy\n\n", new int[] {5, 4})]
    [DataRow("\nhellomy name is joshua\n and   my name jimmy\nhallo\n", new int[] { 4, 4, 1})]
    [DataRow("hello my name is jo", new int[] { 5 })]
    public void GetNumberOfWordsByLineList_VariableLength_Success(string lyrics, int[] result)
    {
        // Arrange

        // Act
        int[] numberOfWordsByLine = FileUtils.GetNumberOfWordsByLineList(lyrics);

        // Assert
        CollectionAssert.AreEquivalent(result, numberOfWordsByLine);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("  ")]
    public void GetNumberOfWordsByLineList_EmptyOrWhiteSpaceLyrics_ThrowsArgumentException(string lyrics)
    {
        // Arrange

        // Act

        // Assert
        Assert.ThrowsException<ArgumentException>(() => FileUtils.GetNumberOfWordsByLineList(lyrics));
    }

    [TestMethod]
    public void GetNumberOfWordsByLineList_NullArgument_ThrowsArgumentNullException()
    {
        // Arrange

        // Act

        // Assert
        Assert.ThrowsException<ArgumentNullException>(() => FileUtils.GetNumberOfWordsByLineList(null!));
    }

    #endregion GetNumberOfWordsByLineList tests

    #region GetPlainSyllableDict tests

    [TestMethod]
    [DataRow("")]
    [DataRow("  ")]
    public void GetPlainSyllableDict_EmptyOrWhiteSpacePath_ThrowsArgumentException(string path)
    {
        // Arrange

        // Act

        // Assert
        Assert.ThrowsException<ArgumentException>(() => FileUtils.GetPlainSyllableDict(path));
    }


    [TestMethod]
    public void GetPlainSyllableDict_InvalidPath_ThrowsFileNotFoundException()
    {
        // Arrange
        string path = Path.Combine("..", "Rhyme.Tests", "DOES-NOT-EXIST.txt");

        // Act

        // Assert
        Assert.ThrowsException<FileNotFoundException>(() => FileUtils.GetPlainSyllableDict(path));
    }

    [TestMethod]
    public void GetPlainSyllableDict_CorrectPath_ReturnCorrectListLength()
    {
        // Arrange
        string path = Path.Combine("..", "..", "..", "..", "Rhyme", "syllabledict.txt");

        // Act
        var result = FileUtils.GetPlainSyllableDict(path);

        // Assert
        Assert.AreEqual(24412, result.Count());
    }

    #endregion GetPlainSyllableDict tests
}
