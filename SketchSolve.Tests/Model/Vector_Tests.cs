namespace SketchSolve.Tests.Model;

using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SketchSolve.Model;

[TestFixture]
public class Vector_Tests
{
  [Test]
  public void DotProductShouldWork()
  {
    var v0 = new Vector(0, 1);

    v0.Dot(v0.UnitNormal).Should().Be(0);
  }

  [Test]
  public void ProjectShouldWork()
  {
    var v0 = new Vector(0, 20);
    var v1 = new Vector(10, 10);
    var v2 = v1.ProjectOnto(v0);

    using (new AssertionScope())
    {
      v2.dX.Value.Should().Be(0);
      v2.dY.Value.Should().Be(10);
    }
  }
}
