namespace SketchSolve.Tests;

using FluentAssertions;
using NUnit.Framework;

[TestFixture]
public class VectorTest
{
  [Test]
  public static void DotProductShouldWork()
  {
    var v0 = new Vector(0, 1);
    v0.Dot(v0.UnitNormal).Should().Be(0);
  }

  [Test]
  public static void ProjectShouldWork()
  {
    var v0 = new Vector(0, 20);
    var v1 = new Vector(10, 10);
    var v2 = v1.ProjectOnto(v0);

    v2.dx.Value.Should().Be(0);
    v2.dy.Value.Should().Be(10);
  }
}

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
