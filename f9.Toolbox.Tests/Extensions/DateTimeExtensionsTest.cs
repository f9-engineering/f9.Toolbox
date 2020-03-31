using System;
using f9.Toolbox.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace f9.Toolbox.Tests.Extensions
{
  [TestClass]
  public class DateTimeExtensionsTest
  {
    [TestMethod]
    public void ToReadableTimeStamp()
    {
      Console.WriteLine("Time stamp : " + DateTime.Now.ToReadableTimeStamp());
    }
  }
}
