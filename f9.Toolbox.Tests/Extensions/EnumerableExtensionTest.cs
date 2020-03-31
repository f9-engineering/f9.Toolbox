using System.Xml.Linq;
using f9.Toolbox.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace f9.Toolbox.Tests.Extensions
{
  [TestClass]
  public class EnumerableExtensionTest
  {
    public string Test { get; set; } = "Test";

    [TestMethod]
    public void IsNullOrEmptyTest()
    {
      // Prepare
      var array = new int[5];

      Assert.IsFalse(array.IsNullOrEmpty());

      array = null;
      Assert.IsTrue(array.IsNullOrEmpty());

      array = new int[0];
      Assert.IsTrue(array.IsNullOrEmpty());
    }

  }
}
