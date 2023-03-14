using NUnit.Framework;

namespace SketchSolve.Tests;

[TestFixture]
public class CircleTest
{
  [Test]
  public static void CenterToTest()
  {
    var c = new Circle() { center = new Point(0, 0), rad = new Parameter(10) };
    var line = new Line(new Point(-10, -10), new Point(-10, 10));
    Console.WriteLine(line);
    var line2 = c.CenterTo(line);
    Console.WriteLine(line2);
  }
}
