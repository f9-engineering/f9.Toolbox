using f9.Toolbox.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace f9.Toolbox.Tests.Extensions
{
  [TestClass]
  public class IntExtensionsTest
  {
    [TestMethod]
    public void ToLettersTest()
    {
      // Assert
      Assert.AreEqual("A", 0.ToLetters());
      Assert.AreEqual("E", 4.ToLetters());
      Assert.AreEqual("Z", 25.ToLetters());
      Assert.AreEqual("AA", 26.ToLetters());

    }
  }
}
