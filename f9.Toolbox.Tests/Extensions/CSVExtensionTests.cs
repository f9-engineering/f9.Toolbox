using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using f9.Toolbox.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace f9.Toolbox.Tests.Extensions
{
  [TestClass]
  public class CsvExtensionTests
  {
    class MockData
    {
      public TimeSpan TimeSpan { get; set; }
    }

    [TestMethod]
    public void ExportToCsvTest()
    {
      // Prepare
      var datas = new List<MockData>();
      for (var i = 0; i < 10; i++)
      {
        datas.Add(new MockData{TimeSpan = TimeSpan.FromSeconds(i)});
      }

      // Act
      var testFile = new FileInfo("test.csv");
      datas.ExportToCsv(testFile);

      var importedData = testFile.ImportCsv<MockData>().ToArray();

      // Assert
      for (var i = 0; i < datas.Count; i++)
      {
        Assert.AreEqual(datas[i].TimeSpan, importedData[i].TimeSpan);
      }
    }
  }
}
