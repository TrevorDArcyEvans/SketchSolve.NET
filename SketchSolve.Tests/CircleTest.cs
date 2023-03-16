namespace SketchSolve.Tests;

using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

[TestFixture]
public class CircleTest
{
  [Test]
  public static void CenterToTest()
  {
    var circle = new Circle
    {
      center = new Point(0, 0),
      rad = new Parameter(10)
    };
    var line = new Line(new Point(-10, -10), new Point(-10, 10));

    var line2 = circle.CenterTo(line);

    using (new AssertionScope())
    {
      line2.p1.x.Value.Should().Be(0);
      line2.p1.y.Value.Should().Be(0);
    }
  }
}
