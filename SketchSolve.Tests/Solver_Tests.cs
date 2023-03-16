﻿namespace SketchSolve.Tests;

using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

[TestFixture]
public class Solver_Tests
{
  [Test]
  public void HorizontalConstraintShouldWork()
  {
    var line = new Line(new Point(0, 1, false), new Point(2, 3, false, true));

    var error = Solver.Solve(true, line.IsHorizontal());

    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);
      line.p1.y.Value.Should().BeApproximately(line.p2.y.Value, 0.001);
    }
  }

  [Test]
  public void VerticalConstraintShouldWork()
  {
    var line = new Line(new Point(0, 1, false), new Point(2, 3, true, false));

    var r = Solver.Solve(true, line.IsVertical());

    Console.WriteLine(line);
    using (new AssertionScope())
    {
      r.Should().BeApproximately(0, 0.0001);
      line.p1.x.Value.Should().BeApproximately(line.p2.x.Value, 0.001);
    }
  }

  [Test]
  public void PointOnPointConstraintShouldWork()
  {
    var line1 = new Line(new Point(0, 1), new Point(2, 3, false));
    var line2 = new Line(new Point(10, 100, false), new Point(200, 300, false));

    var error = Solver.Solve(true, line1.p1.IsColocated(line2.p1));

    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);
      line1.p1.x.Value.Should().BeApproximately(line2.p1.x.Value, 0.001);
      line1.p1.y.Value.Should().BeApproximately(line2.p1.y.Value, 0.001);
    }
  }

  [Test]
  public void InternalAngleConstraintShouldWork()
  {
    for (var i = 1; i < 10; i++)
    {
      var line1 = new Line(new Point(0, 0, false), new Point(10, 0, false, true));
      var line2 = new Line(new Point(0, 0, false), new Point(10, -1, false));

      Console.WriteLine(i);
      const double angle = Math.PI / 2 / 3;   // 30 deg

      var error = Solver.Solve(true, line1.HasInternalAngle(line2, new Parameter(angle, false)));

      using (new AssertionScope())
      {
        error.Should().BeApproximately(0, 0.0001);
        line1
          .Vector
          .Cosine(line2.Vector)
          .Should().BeApproximately(Math.Cos(angle), 0.001);
      }
    }
  }

  [Test]
  public void ExternalAngleConstraintShouldWork()
  {
    for (var i = 1; i < 10; i++)
    {
      var line1 = new Line(new Point(0, 0, false), new Point(10, 0, false, true));
      var line2 = new Line(new Point(0, 0, false), new Point(10, -1, false));

      Console.WriteLine(i);
      const double angle = Math.PI / 2 / 3;   // 30 deg

      var error = Solver.Solve(true, line1.HasExternalAngle(line2, new Parameter(angle, false)));

      using (new AssertionScope())
      {
        error.Should().BeApproximately(0, 0.0001);
        line1
          .Vector
          .Cosine(line2.Vector)
          .Should().BeApproximately(Math.Cos(Math.PI - angle), 0.001);
      }
    }
  }

  [Test]
  public void PerpendicularLineConstraintShouldWork()
  {
    for (var i = 1; i < 10; i++)
    {
      var line1 = new Line(new Point(0, 0, false), new Point(10, 0, false, true));
      var line2 = new Line(new Point(0, 0, false), new Point(10, 10, true, false));

      var error = Solver.Solve(true, line1.IsPerpendicularTo(line2));

      Console.WriteLine(line1);
      Console.WriteLine(line2);

      using (new AssertionScope())
      {
        error.Should().BeApproximately(0, 0.0001);
        line1
          .Vector
          .Dot(line2.Vector)
          .Should().BeApproximately(0, 0.001);
      }
    }
  }

  [Test]
  public void TangentToCircleConstraintShouldWork()
  {
    // Create a fully constrained circle at 0,0 with radius 1
    var circle = new Circle
    {
      center = new Point(0, 0, false),
      rad = new Parameter(1, false)
    };

    var v = 1 / Math.Sin(Math.PI / 4);  // sqrt(2)

    var line = new Line(new Point(0, -v, false, false), new Point(35, 0, true, false));

    var error = Solver.Solve(true, line.IsTangentTo(circle));

    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);
      line.p2.x.Value.Should().BeApproximately(v, 0.001);
    }
  }

  /// <summary>
  /// TODO
  /// Weird problem when only one degree of freedom with horizontal initial conditions
  /// </summary>
  [Test]
  public void TangentToCircleConstraintWithLineInitiallyThroughCenter()
  {
    // Create a fully constrained circle at 0,0 with radius 1
    var circle = new Circle
    {
      center = new Point(0, 0, false),
      rad = new Parameter(1, false)
    };

    var v = 1 / Math.Sin(Math.PI / 4);  // sqrt(2)

    var line = new Line(new Point(0, -v, false, false), new Point(0, v, true, false));

    var error = Solver.Solve(true, line.IsTangentTo(circle));

    Console.WriteLine(line);
    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);
      (circle.CenterTo(line).Vector.LengthSquared - 1).Should().BeApproximately(0, 0.001);
    }
  }

  /// <summary>
  /// TODO
  /// Weird problem when only one degree of freedom with horizontal initial conditions
  /// </summary>
  [Test]
  public void TangentToCircleConstraintWithLineInitiallyHorizontal()
  {
    // Create a fully constrained circle at 0,0 with radius 1
    var circle = new Circle
    {
      center = new Point(0, 0, false),
      rad = new Parameter(1, false)
    };

    var v = 1 / Math.Sin(Math.PI / 4);  // sqrt(2)

    var line = new Line(new Point(0, -v, false, false), new Point(10, -v, true, false));

    var error = Solver.Solve(true, line.IsTangentTo(circle));

    Console.WriteLine(line);
    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);
      (circle.CenterTo(line).Vector.LengthSquared - 1).Should().BeApproximately(0, 0.001);
    }
  }

  /// <summary>
  /// TODO
  /// Not sure how to fix this one. My guess is that we have a local maximum due to the initial conditions
  /// </summary>
  [Test]
  public void TangentToCircleConstraintWithLineInitiallyHorizontalShouldWork()
  {
    // Create a fully constrained circle at 0,0 with radius 1
    var circle = new Circle
    {
      center = new Point(0, 0, false),
      rad = new Parameter(10, false)
    };

    const int v = -15;

    var line = new Line(new Point(-100, -v, false, true), new Point(100, -v * 2.1, false, true));

    var error = Solver.Solve(true, line.IsTangentTo(circle), line.IsHorizontal());

    Console.WriteLine(line);
    Console.WriteLine(circle);

    using (new AssertionScope())
    {
      error.Should().BeApproximately(0, 0.0001);
      (circle.CenterTo(line).Vector.LengthSquared - circle.rad.Value * circle.rad.Value)
        .Should().BeApproximately(0, 0.001);
    }
  }

  [Test]
  public void SquareAroundCircle()
  {
    var circle = new Circle
    {
      center = new Point(0, 0, false),
      rad = new Parameter(10, false)
    };

    // We want a box around the circle where all lines touch the 
    // circle and line0 is vertical. We arrange the lines roughly
    // in the correct placement to get the search off to a good
    // start
    var line0 = new Line(new Point(-21, 0), new Point(0, 23));
    var line1 = new Line(new Point(0, 22), new Point(22, 0));
    var line2 = new Line(new Point(21, 0), new Point(0, -29));
    var line3 = new Line(new Point(0, -27), new Point(-25, 0));

    var angle = new Parameter(Math.PI / 2, false);

    var error = Solver.Solve(
      true,
      line0.IsTangentTo(circle),
      line1.IsTangentTo(circle),
      line2.IsTangentTo(circle),
      line3.IsTangentTo(circle),
      line0.HasInternalAngle(line1, angle),
      line1.HasInternalAngle(line2, angle),
      line2.HasInternalAngle(line3, angle),
      line3.HasInternalAngle(line0, angle),
      line0.p2.IsColocated(line1.p1),
      line1.p2.IsColocated(line2.p1),
      line2.p2.IsColocated(line3.p1),
      line3.p2.IsColocated(line0.p1),
      line0.IsVertical());

    Console.WriteLine(line0);
    Console.WriteLine(line1);
    Console.WriteLine(line2);
    Console.WriteLine(line3);

    Console.WriteLine(circle.CenterTo(line0));

    error.Should().BeApproximately(0, 0.0001);
  }
}
