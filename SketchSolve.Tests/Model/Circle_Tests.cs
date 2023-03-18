namespace SketchSolve.Tests.Model;

using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SketchSolve.Model;

[TestFixture]
public class Circle_Tests
{
  [Test]
  public void CenterToTest()
  {
    var circle = new Circle(new Point(0, 0), new Parameter(10));
    var line = new Line(new Point(-10, -10), new Point(-10, 10));

    var line2 = circle.CenterTo(line);

    using (new AssertionScope())
    {
      line2.P1.X.Value.Should().Be(0);
      line2.P1.Y.Value.Should().Be(0);
      line2.P2.X.Value.Should().Be(-10);
      line2.P2.Y.Value.Should().Be(0);
      line2.Vector.Length.Should().Be(10);
    }
  }
}
