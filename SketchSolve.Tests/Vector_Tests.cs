namespace SketchSolve.Tests;

using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

[TestFixture]
public class Vector_Tests
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

    using (new AssertionScope())
    {
      v2.dx.Value.Should().Be(0);
      v2.dy.Value.Should().Be(10);
    }
  }
}
