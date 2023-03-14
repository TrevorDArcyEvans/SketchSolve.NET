namespace SketchSolve.Tests;

using FluentAssertions;
using NUnit.Framework;

[TestFixture]
public class TestParameters
{
  [Test]
  public void FindingFreeParametersShouldWork()
  {
    var l = new Line(new Point(0, 0, false), new Point(1, 1, false, true));

    var a = l.Where(p => p.free == true).ToArray();

    a.Length.Should().Be(1);
    a[0].GetHashCode().Should().Be(l.p2.y.GetHashCode());
  }
}
