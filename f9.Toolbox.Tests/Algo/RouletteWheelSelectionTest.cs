using System.Collections.Generic;
using System.Linq;
using f9.Toolbox.Algo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace f9.Toolbox.Tests.Algo
{
  [TestClass]
  public class RouletteWheelSelectionTest
  {
    public string Test { get; set; } = "Test";

    [TestMethod]
    public void GetTest()
    {
      // Prepare
      var selection = new RouletteWheelSelection<int>();
      selection.Add(1, 1);
      selection.Add(2, 2);
      selection.Add(3, 3);

      // Act
      var results = new List<int>();
      for (var i = 0; i < 10000; i++)
      {
        results.Add(selection.Get());
      }

      // Assert
      var ones = (double) results.Count(r => r == 1);
      Assert.AreEqual(2.0, results.Count(r => r == 2) / ones, 0.1);
      Assert.AreEqual(3.0, results.Count(r => r == 3) / ones, 0.1);
    }
  }
}
