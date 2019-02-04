using System.Xml.Linq;
using f9.Toolbox.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace f9.Toolbox.Tests.Extensions
{
  [TestClass]
  public class XElementExtensionTests
  {
    [TestMethod]
    public void NormalizeTest()
    {
      // Prepare
      var xmlTxt = "<AnalyseParameterSet Name=\"timoteo\"><RatioParameters MaxPerturbation=\"6\" IncSlopeTemp=\"2.5\" IncSlopeTime=\"10\" IncSlopeMaxTime=\"50\" IncSlopeMaxTemp=\"50\" DecSlopeTemp=\"8\" DecSlopeTime=\"6\" DecSlopeMaxTime=\"78\" DecSlopeMaxTemp=\"78\" DimSlidingAverage=\"3\" MaximumIncreaseDuration=\"18\" MaximumSlopeEndDuration=\"15\" />  <SlopeCorrectionParameters CorrectionLowestSlope=\"1\" CorrectionLevel1Temperature=\"90\" CorrectionLevel2Temperature=\"130\" />  <CBA IsEnabled=\"true\" IncreasePercentage=\"75\" />  <AlarmParameters BreakoutTimeMin=\"5\" BreakoutTimeMax=\"35\" ActivationSpeed=\"0.5\" DeactivationSpeed=\"0.3\" />  <AlarmLevel1>    <Rule Description=\"Alarm Level 1\" SerializedThermoCoupleRules=\"0 0 1 0\" />  </AlarmLevel1>  <AlarmLevel2>    <Rule Description=\"Alarm Level 2\" SerializedThermoCoupleRules=\"0 0 1 0.2, 1 0 0.2 0\" />  </AlarmLevel2>  <Dynamic numtc11=\"3\" numtc12=\"2\" numtc13=\"1\" numtc21=\"2\" numtc22=\"2\" numtc23=\"1\" numtc31=\"2\" numtc32=\"1\" numtc33=\"1\" LimitDec1=\"3.00\" LimitDec2=\"6.00\" LimitInc1=\"1.70\" LimitInc2=\"2.40\">    <NumTc1>      <Rule Description=\"Alarm Level 3 - 1TC - + Montée inf\" SerializedThermoCoupleRules=\"0 0 0 0\" Id=\"\" />    </NumTc1>    <NumTc2>      <Rule Id=\"\" Description=\"Alarm Level 3 - 2TC - Right Side\" SerializedThermoCoupleRules=\"1 0 1 1, 0 1 1 0\" />      <Rule Id=\"\" Description=\"Alarm Level 3 - 2TC - Left Side\" SerializedThermoCoupleRules=\"1 0 1 1, 0 -1 1 0\" />    </NumTc2>    <NumTc3>      <Rule Id=\"\" Description=\"Alarm Level 3 - 3TC - Right Side\" SerializedThermoCoupleRules=\"1 0 1 1, 0 1 1 1\" />      <Rule Id=\"\" Description=\"Alarm Level 3 - 3TC - Left Side\" SerializedThermoCoupleRules=\"1 0 1 1, 0 -1 1 1\" />    </NumTc3>  </Dynamic>  <Static>    <Rule Description=\"Hydrogène - droite\" SerializedThermoCoupleRules=\"1 0 2.4 1, 1 1 2.4 0\" />    <Rule Description=\"Hydrogène - gauche\" SerializedThermoCoupleRules=\"1 0 2.4 1, 1 -1 2.4 0\" />  </Static></AnalyseParameterSet>";
      var xElement = XElement.Parse(xmlTxt);

      Assert.IsFalse(xElement.ToString().IndexOf(@"AlarmLevel1") < xElement.ToString().IndexOf(@"RatioParameters"));

      xElement.Normalize();
      Assert.IsTrue(xElement.ToString().IndexOf(@"AlarmLevel1") < xElement.ToString().IndexOf(@"RatioParameters"));
    }
  }
}
