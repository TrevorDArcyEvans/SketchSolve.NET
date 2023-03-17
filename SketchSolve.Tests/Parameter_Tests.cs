using SketchSolve.Model;

namespace SketchSolve.Tests;

using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

[TestFixture]
public class Parameter_Tests
{
  [Test]
  public void FindingFreeParametersShouldWork()
  {
    var line = new Line(new Point(0, 0, false), new Point(1, 1, false, true));

    var parameters = line.Where(p => p.Free).ToArray();

    using (new AssertionScope())
    {
      parameters.Length.Should().Be(1);
      parameters[0].GetHashCode().Should().Be(line.P2.Y.GetHashCode());
    }
  }
}
