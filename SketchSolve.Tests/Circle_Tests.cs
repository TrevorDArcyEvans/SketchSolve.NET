namespace SketchSolve.Tests;

using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

[TestFixture]
public class Circle_Tests
{
  [Test]
  public static void CenterToTest()
  {
    var circle = new Circle
    {
      Center = new Point(0, 0),
      Rad = new Parameter(10)
    };
    var line = new Line(new Point(-10, -10), new Point(-10, 10));

    var line2 = circle.CenterTo(line);

    using (new AssertionScope())
    {
      line2.P1.X.Value.Should().Be(0);
      line2.P1.Y.Value.Should().Be(0);
    }
  }
}
